import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { LoginMdl } from '../../models/login-mdl';
import { globalConstants } from '../../Constants/globalConstants';
import { authConstants } from '../../Constants/AuthConstants';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private tokenKey = globalConstants.tokenKey;
  constructor(private http: HttpClient) { }

  login(model: LoginMdl) {
    return this.http.post<{ accessToken: string }>(authConstants.loginUrl, model,
      { withCredentials: true }).pipe( //mandatory to pass for cookies
        tap(res => {
          if (res?.accessToken) {
            this.setAccessToken(res.accessToken);
          }
        }),
        catchError((err: HttpErrorResponse) => {
          // rethrow to component
          return throwError(() => err);
        })
      );
  }

  setAccessToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  refreshToken(): Observable<any> {
    // Sends HttpOnly cookie automatically
    return this.http.post(authConstants.refreshUrl, {},
      { withCredentials: true }).pipe( //mandatory to pass for cookies
      tap((res: any) => {
        this.setAccessToken(res.accessToken);
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }
}
