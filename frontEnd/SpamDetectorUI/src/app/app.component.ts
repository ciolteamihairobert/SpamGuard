import { CommonModule } from '@angular/common';
import { Component, Injectable, Type } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { UserLogin } from './models/userLogin';
import { FormsModule } from '@angular/forms';
import { SignupComponent } from './signup/signup.component';
import { MatDialog, MatDialogConfig, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { SigninComponent } from './signin/signin.component';
import { ModalSize } from './sizeConfig';

@Injectable({
  providedIn: 'root',
})

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule, MatButtonModule, MatDialogModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent{
  public userLogin = new UserLogin();
  constructor(private authService: AuthService,
    private modal: MatDialog,
    private size: ModalSize) {}

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

  login(user: UserLogin){
    this.authService.login(user).subscribe((token : string) =>{
      localStorage.setItem('authToken', token);
      console.log(token);
    });  
  }
}
