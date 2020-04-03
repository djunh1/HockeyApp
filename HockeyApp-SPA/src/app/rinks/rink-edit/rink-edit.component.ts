import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-rink-edit',
  templateUrl: './rink-edit.component.html',
  styleUrls: ['./rink-edit.component.css']
})
export class RinkEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm', { static: true }) editForm: NgForm;
  // If the user closes a browser tab
  @HostListener('window:beforeunload', ['$event'])

  unloadNotification($event: any) {
    if (this.editForm.dirty){
      $event.returnValue = true;
    }
  }
  // STEP 4 - Updating Information, bring in the UserService
  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private userService: UserService,
    private authService: AuthService ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  // STEP 5 - Updating information (Last section)
  updateUser(){
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('Profile updated successfully.');
      // Reset the form, by default it will wipe the forms out which we don't want
      // If we have existing persisted data.
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }
}
