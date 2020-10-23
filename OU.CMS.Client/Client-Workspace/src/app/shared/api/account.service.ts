import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { SignInDto, SignUpDto } from '../models/AccountModels';
import { UserInfo } from '../models/AuthenticationModels';
import { map } from 'rxjs/operators';
// import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AccountService {
  public API = 'https://localhost:44305/api';
  public ACCOUNT_API = `${this.API}/account`;

  isAuthenticated: boolean = false;
  userInfo: UserInfo;
  isCandidateLogin : boolean;
  // decodedToken: any;

  constructor(private http: HttpClient) {}

  login(loginDto: SignInDto) {
    return this.http.post<UserInfo>(`${this.ACCOUNT_API}/Login`, loginDto).pipe(
      map((response: UserInfo) => {
        if (response) {
          localStorage.setItem('token', response.Token);
          localStorage.setItem('userInfo', JSON.stringify(response));

          // this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.userInfo = response;
          this.isCandidateLogin = response.IsCandidateLogin;
          // this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      })
    );
  }

  register(signUpDto: SignUpDto) {
    return this.http.post(`${this.ACCOUNT_API}/Register`, signUpDto);
  }
}
