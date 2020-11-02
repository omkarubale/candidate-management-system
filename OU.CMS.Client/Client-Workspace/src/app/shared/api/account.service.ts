import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { SignInDto, SignUpDto } from '../models/AccountModels';
import { UserInfo } from '../models/AuthenticationModels';
import { map } from 'rxjs/operators';
import { Subject } from 'rxjs';
// import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AccountService {
  public API = 'https://localhost:44305/api';
  public ACCOUNT_API = `${this.API}/account`;

  isAuthenticated: boolean = false;
  isCandidateLogin : boolean;
  userInfo: UserInfo;

  isAuthenticatedChange: Subject<boolean> = new Subject<boolean>();
  isCandidateLoginChange: Subject<boolean> = new Subject<boolean>();
  userInfoChange: Subject<UserInfo> = new Subject<UserInfo>();
  // decodedToken: any;

  constructor(private http: HttpClient) {
    var token = localStorage.getItem('token');
    this.isAuthenticated = localStorage.getItem('token') !== null;
    if(this.isAuthenticated)
    {
      var userInfo = this.getUserInfo();
      this.userInfo = userInfo;
      this.isCandidateLogin = userInfo.IsCandidateLogin;
    }
  }

  login(loginDto: SignInDto) {
    return this.http.post<UserInfo>(`${this.ACCOUNT_API}/Login`, loginDto).pipe(
      map((response: UserInfo) => {
        if (response) {
          localStorage.setItem('token', response.Token);
          localStorage.setItem('userInfo', JSON.stringify(response));

          this.isAuthenticated = true;
          this.isCandidateLogin = response.IsCandidateLogin;
          this.userInfo = response;

          this.isAuthenticatedChange.next(this.isAuthenticated);
          this.isCandidateLoginChange.next(this.isCandidateLogin);
          this.userInfoChange.next(this.userInfo);
        }
      })
    );
  }

  register(signUpDto: SignUpDto) {
    return this.http.post(`${this.ACCOUNT_API}/Register`, signUpDto);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userInfo');

    this.isAuthenticated = false;
    this.isCandidateLogin = false;
    this.userInfo = undefined;

    this.isAuthenticatedChange.next(this.isAuthenticated);
    this.isCandidateLoginChange.next(this.isCandidateLogin);
    this.userInfoChange.next(this.userInfo);

    return;
  }

  getUserInfo() : any {
    var userInfo = JSON.parse(localStorage.getItem('userInfo'));
    return userInfo;
  }

}
