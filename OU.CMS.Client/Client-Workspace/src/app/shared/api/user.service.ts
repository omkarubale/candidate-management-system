import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {
  UserDto
} from '../models/UserModels';

@Injectable()
export class UserService {

  public API = 'https://localhost:44305/api';
  public USERS_API = `${this.API}/user`;

  constructor(private http: HttpClient) {}

  getIdentityUser(): Observable<UserDto> {
    return this.http.get<UserDto>(
      `${this.USERS_API}/GetIdentityUser`
    );
  }

  saveIdentityUser(user: UserDto): Observable<UserDto> {
    return this.http.post<UserDto>(
      `${this.USERS_API}/SaveIdentityUser`,
      user
    );
  }
}
