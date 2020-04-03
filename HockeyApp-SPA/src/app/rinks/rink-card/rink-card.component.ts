import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-rink-card',
  templateUrl: './rink-card.component.html',
  styleUrls: ['./rink-card.component.css']
})
export class RinkCardComponent implements OnInit {
  // Pass down rink component from the parent
  @Input() user: User;

  constructor() { }

  ngOnInit() {
  }

}
