import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { UserLogin } from './models/userLogin';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  constructor(private authService: AuthService) {}
  public userLogin = new UserLogin();
  login(user: UserLogin){
    this.authService.login(user).subscribe((token : string) =>{
      localStorage.setItem('authToken', token);
      console.log(token);
    });
    
  }

}
