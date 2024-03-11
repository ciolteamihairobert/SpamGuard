import { Component } from '@angular/core';
import { MatDialogActions, MatDialogRef } from '@angular/material/dialog';
import { MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { AppComponent } from '../app.component';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserRegister } from '../models/userRegister';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [MatDialogActions, MatButtonModule, MatDialogModule, ReactiveFormsModule, ToastrModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  public signUpForm: FormGroup<any> = new FormGroup<any>({});

  constructor(private appComponent: AppComponent,
    private modalRef: MatDialogRef<SignupComponent>,
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService
    ) {}

  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  register(){
    if (this.signUpForm) {
      const user: UserRegister = new UserRegister();
      user.firstName = this.signUpForm.get('firstName')?.value;
      user.lastName = this.signUpForm.get('lastName')?.value;
      user.email = this.signUpForm.get('email')?.value;
      user.password = this.signUpForm.get('password')?.value;
      user.role = 'User';
      if(user.email && user.password && user.firstName && user.lastName){
        this.authService.register(user).subscribe({
          next: () => {
            console.log('user registred');
            this.toastr.success('Registration Success!');
            this.modalRef.close();
          },
          error: (error : HttpErrorResponse) => {
            this.toastr.error(error.error,'Registration Failed!');
          }
        }); 
      }
      else{
        this.toastr.error('All fileds are required!');
      }
    }
  }
  
  openSignin() {
    this.appComponent.openSignInModal();
    this.modalRef.close();
  }
}
