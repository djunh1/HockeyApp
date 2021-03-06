import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { tap } from 'rxjs/internal/operators/tap';


@Component({
  selector: 'app-rink-messages',
  templateUrl: './rink-messages.component.html',
  styleUrls: ['./rink-messages.component.css']
})
export class RinkMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};

  constructor(private userService: UserService,
              private authService: AuthService,
              private altertify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages(){
    const currentUserId = +this.authService.decodedToken.nameid;
    this.userService.getMessageThread(this.authService.decodedToken.nameid, this.recipientId)
        .pipe(
          tap(messages => {
            for (let i=0; i < messages.length; i++){
              if (messages[i].isRead === false && messages[i].recipientId === currentUserId){
                  this.userService.markAsRead(currentUserId, messages[i].id);
              }
            }
          })
        )
        .subscribe(messages => {
           this.messages = messages;
        }, error => {
            this.altertify.error(error);
        });
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService
      .sendMessage(this.authService.decodedToken.nameid, this.newMessage)
      .subscribe(
        (message: Message) => {
          this.messages.unshift(message);
          this.newMessage.content = '';
        },
        error => {
          this.altertify.error(error);
        }
      );
  }

}
