import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { AppComponent } from '../app.component';
import { UserLogin } from '../models/userLogin';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [MatDialogActions, MatButtonModule, MatDialogModule, ReactiveFormsModule, ToastrModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css'
})
export class SigninComponent implements OnInit {
  public signInForm: FormGroup<any> = new FormGroup<any>({});
  public showSpinner: boolean = false;

  constructor(private appComponent: AppComponent,
    private modalRef: MatDialogRef<SigninComponent>,
    private authService: AuthService,
    private fb: FormBuilder,
    private toastr: ToastrService) {}

  ngOnInit(): void {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  openSignup() {
    this.appComponent.openSignUpModal();
    this.modalRef.close();
  }

  openForgotPassword() {
    if (this.signInForm) {
      const email = this.signInForm.get('email')?.value;
      if (email) {
        this.authService.forgotPassword(email).subscribe({
          next: () => {
            this.toastr.success('You have been sent an email with instructions!');
            this.appComponent.openPasswordResetModal();
            this.modalRef.close();
          },
          error: (error: HttpErrorResponse) => {
            this.toastr.error(error.error);
            console.log(error);
          }
        });
      }
      else {
        this.toastr.error('Email is required!');
      }
    }
  }

  login(){
    if (this.signInForm) {
      const user: UserLogin = new UserLogin();
      user.email = this.signInForm.get('email')?.value;
      user.password = this.signInForm.get('password')?.value;
      if(user.password && user.email){
        this.authService.login(user).subscribe({
          next: (token: string) => {
            sessionStorage.setItem('authToken', token);
            this.toastr.success('Log in Success!');
            this.appComponent.loggedIn = true;
            this.modalRef.close();
            console.log(token);
          },
          error: (error : HttpErrorResponse) => {
            this.toastr.error(error.error,'Log in Failed!');
          }
        }); 
      }
      else {
        const email = user.email === '';
        const password = user.password === '';
        if(email && password){
          this.toastr.error('Email and password are required!');
        }
        else{
          if(email){
            this.toastr.error('Email is required!');
          }
          else{
            this.toastr.error('Password is required!');
          }
        }
      }
    }
  }
}
