import { Component, OnInit } from '@angular/core';
import { LoginMdl } from '../../../models/login-mdl';
import { LoginService } from '../../../Services/Auth/login.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { authMessageConstants } from '../../../Constants/AuthConstants';
import { globalErrorMessage } from '../../../Constants/globalConstants';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginObj: LoginMdl = {} as LoginMdl;

  constructor(private loginService: LoginService
    , private router: Router
  ) {}

  ngOnInit(): void {
  }

  login() {
    this.loginService.login(this.loginObj).subscribe({
      next: (res) => {
        console.log(authMessageConstants.successMessage, res);
        this.router.navigate(['/customer']);
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === 401) {
          alert(authMessageConstants.error401Message);
        } else if (err.status === 0) {
          alert(authMessageConstants.errorServerNotResponding);
        } else {
          alert(`${globalErrorMessage.errorMessage}: ` + err.message);
        }
      }
    });
  }
}
