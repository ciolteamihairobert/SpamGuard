import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegister } from '../models/userRegister';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { UserLogin } from '../models/userLogin';
import { UserPasswordReset } from '../models/userResetPassword';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient: HttpClient) { }

  public register(user: UserRegister) : Observable<User> {
    return this.httpClient.post<User>(
      'https://localhost:7062/api/Auth/register',
       user
    );
  }

  public login(user: UserLogin) : Observable<string> {
    return this.httpClient.post(
      'https://localhost:7062/api/Auth/login', user, {
        responseType: 'text'
    });
  }

  public forgotPassword(email: string) : Observable<any> {
    const url = 'https://localhost:7062/api/Auth/forgot-password';
    const params = { email: email };

    return this.httpClient.post(url, {}, { params });
  }

  public resetPassword(user: UserPasswordReset) : Observable<any> {
    return this.httpClient.post(
      'https://localhost:7062/api/Auth/reset-password', user);
  }

  public refreshToken(user: UserLogin) : Observable<any> {
    return this.httpClient.post(
      'https://localhost:7062/api/Auth/refresh-token', user);
  }

}
