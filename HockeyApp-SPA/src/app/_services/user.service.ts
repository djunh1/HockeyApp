import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

export class UserService {
  baseUrl = environment.apiUrl;

  // Now are able to access methods from http object
  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(this.baseUrl + 'users');
    // get returns observable with type object, not type user as the function requires.
  }

  getUser(id): Observable<User>{
    // Inside options, add headers to send bearer token to the server
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

}
