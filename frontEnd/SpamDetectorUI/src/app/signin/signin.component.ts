import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { AppComponent } from '../app.component';
import { UserLogin } from '../models/userLogin';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';


@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [MatDialogActions, MatButtonModule, MatDialogModule, ReactiveFormsModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css'
})
export class SigninComponent implements OnInit {
  public signInForm: FormGroup<any> = new FormGroup<any>({});

  constructor(private appComponent: AppComponent,
    private modalRef: MatDialogRef<SigninComponent>,
    private authService: AuthService,
    private fb: FormBuilder) {}

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
    throw new Error('Method not implemented.');
  }

  login(){
    if (this.signInForm) {
      const user: UserLogin = new UserLogin();
      user.email = this.signInForm.get('email')?.value;
      user.password = this.signInForm.get('password')?.value;

      this.authService.login(user).subscribe((token: string) => {
        sessionStorage.setItem('authToken', token);
        console.log(token);
      }); 
    }
  }
}
