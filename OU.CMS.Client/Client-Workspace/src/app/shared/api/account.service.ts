import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { SignInDto, SignUpDto } from '../models/AccountModels';
import { UserInfo } from '../models/AuthenticationModels';

@Injectable()
export class AccountService {
  public API = 'https://localhost:44305/api';
  public ACCOUNT_API = `${this.API}/account`;

  constructor(private http: HttpClient) {}

  login(loginDto: SignInDto): Observable<UserInfo> {
    return this.http.post<UserInfo>(`${this.ACCOUNT_API}/Login`, loginDto);
  }

  register(signUpDto: SignUpDto) {
    return this.http.post(`${this.ACCOUNT_API}/Register`, signUpDto);
  }
}
