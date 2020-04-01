import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

// Decerator.  Can inject anything into the service.
// Components injecatble by default, services are not.
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';

constructor(private http: HttpClient) { }

login(model: any){
  return this.http.post(this.baseUrl + 'login', model)
    .pipe(
      map((response: any) => {
        const user = response;
        if(user){
          localStorage.setItem('token', user.token);
        }
      })
    );
}

register(model: any){
  // Must subscribe to the observable in the component which uses this.
  return this.http.post(this.baseUrl + 'register', model);
}

}
