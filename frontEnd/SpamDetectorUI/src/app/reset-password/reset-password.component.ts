import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AppComponent } from '../app.component';
import { AuthService } from '../services/auth.service';
import { UserPasswordReset } from '../models/userResetPassword';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [MatDialogActions, MatButtonModule, MatDialogModule, ReactiveFormsModule, ToastrModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  public resetPasswordForm: FormGroup<any> = new FormGroup<any>({});

  constructor(private appComponent: AppComponent,
    private modalRef: MatDialogRef<ResetPasswordComponent>,
    private authService: AuthService,
    private fb: FormBuilder,
    private toastr: ToastrService) {}

  ngOnInit(): void {
    this.resetPasswordForm = this.fb.group({
      resetCode: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      passwordConfirmation: ['', Validators.required]
    });
  }

  resetPassword() {
    if (this.resetPasswordForm) {
      const user: UserPasswordReset = new UserPasswordReset();
      user.email = this.resetPasswordForm.get('email')?.value;
      user.password = this.resetPasswordForm.get('password')?.value;
      user.passwordConfirmation = this.resetPasswordForm.get('passwordConfirmation')?.value;
      user.resetCode = this.resetPasswordForm.get('resetCode')?.value;

      if(user.password && user.email && user.passwordConfirmation && user.resetCode) {
        this.authService.resetPassword(user).subscribe({
          next: () => {
            this.toastr.success('Password succesfully reset!');
            this.modalRef.close();
          },
          error: (error : HttpErrorResponse) => {
            this.toastr.error(error.error,'Password reset failed!!');
            console.log(error);
          }
        }); 
      }
      else {
        this.toastr.error('All fileds are required!');
      }
    }
  }

}
