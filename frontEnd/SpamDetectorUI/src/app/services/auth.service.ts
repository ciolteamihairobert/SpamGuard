import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegister } from '../models/userRegister';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { UserLogin } from '../models/userLogin';

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
}
