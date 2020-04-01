import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // Store username and password
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login(){
    // Retruns an observble, which we must subscribe to.
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in Successfully.');
    }, error => {
      this.alertify.error(error);
    }, () => { 
      this.router.navigate(['rinks']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
    console.log(this.authService.loggedIn());
  }

  logout(){
    localStorage.removeItem('token');
    this.alertify.message('Logged out Successfully.');
    this.router.navigate(['/home']);
  }

}
