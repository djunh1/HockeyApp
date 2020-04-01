import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Injectable } from '@angular/core';

// Similar to a service, it needs to be injectable
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(
        req: import('@angular/common/http').HttpRequest<any>,
        next: import('@angular/common/http').HttpHandler
        ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(errorClient => {

                if (errorClient.status === 401){
                    return throwError(errorClient.statusText);
                }

                if (errorClient instanceof HttpErrorResponse) {
                    const applicationError = errorClient.headers.get('Application-Error');
                    if (applicationError){
                        return throwError(applicationError);
                    }

                    const serverError = errorClient.error;
                    let modelStateErrors = '';

                    if (serverError.errors && typeof serverError.errors === 'object'){
                        for (const key in serverError.errors){
                            if (serverError.errors[key]){
                                // build up list of strings for each model state error from server
                                modelStateErrors += serverError.errors[key] + '\n';
                            }
                        }
                    }
                    return throwError(modelStateErrors || serverError || 'Server Error');
                }
            })
        );
    }

}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
