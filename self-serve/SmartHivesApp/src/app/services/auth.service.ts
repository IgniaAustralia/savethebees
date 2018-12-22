import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { settings } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
    constructor(private httpClient: HttpClient) { }

    retrieveAuthToken(username, password) {
        var url = settings.baseApiUrl + settings.endPoints.authToken;
        return this.httpClient.post(url, this.buildAuthTokenRequest(username, password), {
            headers: new HttpHeaders({
                'Content-Type': 'application/x-www-form-urlencoded'
            }),
            observe: 'response'
        }).pipe(catchError(this.handleError));
    }

    private buildAuthTokenRequest(username, password) {
        var parameters = {
            'client_id': settings.authentication.clientId,
            'client_secret': settings.authentication.clientSecret,
            'grant_type': 'password',
            'username': username,
            'password': password
        };

        var formData = [];
        for (var prop in parameters) {
            var key = encodeURIComponent(prop);
            var value = encodeURIComponent(parameters[prop]);
            formData.push(key + '=' + value);
        }

        return formData.join('&');
    }

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            return throwError(error.error.message);
        } else {
            if (error.error.error && error.error.error === 'invalid_client') {
                return throwError('Authentication settings for this environment are incorrect. Please contact your administrator.');
            } else if (error.error.error && error.error.error === 'invalid_grant' && error.error.error_description == 'invalid_username_or_password') {
                return throwError('Invalid username and/or password. Please try again.')
            }
            return throwError('There was an error logging into the app. Please contact your administrator.');
        }
    }
}