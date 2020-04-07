import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-rink-card',
  templateUrl: './rink-card.component.html',
  styleUrls: ['./rink-card.component.css']
})
export class RinkCardComponent implements OnInit {
  // Pass down rink component from the parent
  @Input() user: User;

  constructor(private authService: AuthService,
              private userService: UserService, private alterify: AlertifyService) { }

  ngOnInit() {
  }

  sendFollow(id: number) {
    this.userService
      .sendFollow(this.authService.decodedToken.nameid, id)
      .subscribe(data => {
        this.alterify.success('You are now following: ' + this.user.knownAs);
      }, error => {
        this.alterify.error(error);
      });
  }

}
