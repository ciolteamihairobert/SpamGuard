import { CommonModule } from '@angular/common';
import { Component, Injectable, OnInit, Type } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { UserLogin } from './models/userLogin';
import { FormsModule } from '@angular/forms';
import { SignupComponent } from './signup/signup.component';
import { MatDialog, MatDialogConfig, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { SigninComponent } from './signin/signin.component';
import { ModalSize } from './sizeConfig';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule, MatButtonModule, MatDialogModule, ToastrModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent implements OnInit {

  public loggedIn: boolean = false; 
  public userLogin = new UserLogin();
  constructor(private authService: AuthService,
    private modal: MatDialog,
    private size: ModalSize,
    private toastr: ToastrService) {}

  async ngOnInit(): Promise<void> {
    const isServerRunning = await this.authService.checkServerStatus();
    if(!isServerRunning) {
      this.toastr.error('Server is not running');
    }
  }

  openModal(componentType: Type<any>, configurationSmall?: MatDialogConfig<any>, configurationLarge?: MatDialogConfig<any>) {
    if (window.innerWidth >= 1300) {
      this.modal.open(componentType, configurationLarge);
    } else if (window.innerWidth < 1300) {
      this.modal.open(componentType, configurationSmall);
    }
  }
    
  openSignUpModal(){
    this.openModal(SignupComponent, this.size.configSmallSU, this.size.configLargeSU);
  }

  openSignInModal(){
    this.openModal(SigninComponent, this.size.configSmallSI, this.size.configLargeSI);
  }

  openPasswordResetModal(){
    this.openModal(ResetPasswordComponent, this.size.configSmallSI, this.size.configLargeSI);
  }

  openUserMenuModal() {
    throw new Error('Method not implemented.');
    }
}
