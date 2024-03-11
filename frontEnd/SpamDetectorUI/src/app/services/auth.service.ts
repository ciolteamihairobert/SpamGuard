import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegister } from '../models/userRegister';
import { Observable, catchError, first, interval, map, of, switchMap, take } from 'rxjs';
import { User } from '../models/user';
import { UserLogin } from '../models/userLogin';
import { UserPasswordReset } from '../models/userResetPassword';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseAuthUrl: string =  'https://localhost:7062/api/Auth/';
  private source = interval(5 * 1000);

  constructor(private httpClient: HttpClient) { }

  public register(user: UserRegister) : Observable<User> {
    return this.httpClient.post<User>(
      this.baseAuthUrl+'register',
      user
    );
  }

  public login(user: UserLogin) : Observable<string> {
    return this.httpClient.post(
      this.baseAuthUrl+'login',
      user, 
      { responseType: 'text' }
    );
  }

  public forgotPassword(email: string) : Observable<any> {
    const params = { email: email };
    return this.httpClient.post(
      this.baseAuthUrl+'forgot-password',
      {},
      { params }
    );
  }

  public resetPassword(user: UserPasswordReset) : Observable<any> {
    return this.httpClient.post<UserPasswordReset>(
      this.baseAuthUrl+'reset-password',
      user
    );
  }

  public refreshToken(user: UserLogin) : Observable<any> {
    return this.httpClient.post(
      this.baseAuthUrl+'refresh-token',
      user
    );
  }

  public async checkServerStatus(): Promise<boolean> {
    try {
      const resp: any = await this.httpClient.get(this.baseAuthUrl + 'check-server-status').pipe(
        take(1),
        catchError(() => of({ status: 500 }))
      ).toPromise();

      return resp.status === 200;
    } catch (error) {
      return false;
    }
  }
}
