import { Component } from '@angular/core';
import { MatDialogActions, MatDialogRef } from '@angular/material/dialog';
import { MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [MatDialogActions, MatButtonModule, MatDialogModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  constructor(private appComponent: AppComponent,
    private modalRef: MatDialogRef<SignupComponent>) {}
  
  openSignin() {
    this.appComponent.openSignInModal();
    this.modalRef.close();
  }
}
