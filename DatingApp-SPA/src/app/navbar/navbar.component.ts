import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) {}

  ngOnInit() {}
  login() {
    this.authService.login(this.model).subscribe(
      next => {
        console.log('Login successful');
      },
      error => {
        console.log('failed to log it');
      }
    );
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    console.log('Token?', !!token);
    return !!token;
  }
  logout() {
    localStorage.removeItem('token');
    console.log('Logged Out');
  }
}
