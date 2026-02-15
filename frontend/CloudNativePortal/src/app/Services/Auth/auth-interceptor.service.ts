import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { LoginService } from './login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(private authService: LoginService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getAccessToken();
    let authReq = req;

    if (token && !req.url.includes('/login') && !req.url.includes('/refresh')) {
      authReq = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
    }

    return next.handle(authReq).pipe(
      catchError((err: HttpErrorResponse) => {
        // Only attempt refresh for 401 on protected API requests
        if (err.status === 401 && !req.url.includes('/login') && !req.url.includes('/refresh')) {

          // Prevent multiple refresh requests at the same time
          if (!this.isRefreshing) {
            this.isRefreshing = true;

            return this.authService.refreshToken().pipe(
              switchMap((res: any) => {
                this.isRefreshing = false;
                if (res?.accessToken) {
                  this.authService.setAccessToken(res.accessToken);

                  // Retry original request with new token
                  const newReq = req.clone({
                    setHeaders: { Authorization: `Bearer ${res.accessToken}` }
                  });
                  return next.handle(newReq);
                } else {
                  // Refresh failed → force logout
                  this.authService.logout();
                  return throwError(() => err);
                }
              }),
              catchError(refreshErr => {
                this.isRefreshing = false;
                this.authService.logout(); // refresh token invalid or expired
                return throwError(() => refreshErr);
              })
            );
          } else {
            // Already refreshing → block further retries
            return throwError(() => err);
          }
        }

        // 401 for login/refresh endpoints or other errors → pass through
        return throwError(() => err);
      })
    );
  }
}


