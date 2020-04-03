import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()

export class RinkDetailResolver implements Resolve<User>{
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService){}

    // Fetch users data, than catch error and return out of method if there is a problem.
    // Must provide a resolver to app.ts, and routes.
    //  This helps get the data before we get the route itself, so we dont need to use ? in the views.
    resolve(route: ActivatedRouteSnapshot) : Observable<User> {
        // This automatically subscribes to the method, so we do not need to subscripe to it.
        return this.userService.getUser(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error('Problem Retrieving Data');
                this.router.navigate(['/members']);
                return of(null); // of is an observable
            })
        );
    }
}