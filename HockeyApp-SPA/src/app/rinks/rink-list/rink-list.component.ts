import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-rink-list',
  templateUrl: './rink-list.component.html',
  styleUrls: ['./rink-list.component.css']
})
export class RinkListComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  user: User = JSON.parse(localStorage.getItem('user'));
  positionList = [{value: 'Forward', display: 'Forward'},
                  {value: 'Defence', display: 'Defence'} ];
  userParams: any = {};

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  resetFilters(){
    this.userParams.playerPosition = this.user.playerPosition === 'Forward' ? 'Defence' : 'Forward';
    this.loadUsers();
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.userParams.playerPosition = this.user.playerPosition === 'Forward' ? 'Defence' : 'Forward';
    this.userParams.orderBy = 'lastActive'; 
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    // subscribe to observable, need to tell it what to return.
    this.userService
      .getUsers(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.userParams)
      .subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
      this.alertify.success('Search Complete!');
    }, error => {
      this.alertify.error(error);
    });
  }

}
