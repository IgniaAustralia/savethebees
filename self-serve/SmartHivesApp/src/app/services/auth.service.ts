import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { settings } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private refreshToken: string;
    constructor(private httpClient: HttpClient) { }

    loginUser(username: string, password: string): Promise<string> {
        return new Promise((resolve, reject) => {
            const url = settings.baseApiUrl + settings.endPoints.authToken;
            this.httpClient.post(url, this.buildAuthTokenPasswordRequest(username, password), {
                headers: new HttpHeaders({
                    'Content-Type': 'application/x-www-form-urlencoded'
                }),
                observe: 'response'
            }).pipe(catchError(this.handleError)).subscribe(response => {
                // Check if the response does not have a body, access and refresh token
                if (!response.body || !response.body['access_token'] || !response.body['refresh_token']) {
                    reject('Cannot log into application');
                    return;
                }

                // Retrieve access and refresh tokens
                this.refreshToken = response.body['refresh_token'];
                resolve(response.body['access_token']);
            }, error => {
                reject(error);
            });
        });
    }

    private buildAuthTokenPasswordRequest(username: string, password: string): string {
        const parameters = {
            'client_id': settings.authentication.clientId,
            'client_secret': settings.authentication.clientSecret,
            'grant_type': 'password',
            'username': username,
            'password': password
        };

        const formData = [];
        for (const prop in parameters) {
            const key = encodeURIComponent(prop);
            const value = encodeURIComponent(parameters[prop]);
            formData.push(key + '=' + value);
        }

        return formData.join('&');
    }

    private handleError(error: HttpErrorResponse): Observable<never> {
        if (error.error instanceof ErrorEvent) {
            return throwError(error.error.message);
        } else {
            if (error.error.error && error.error.error === 'invalid_client') {
                return throwError('Authentication settings for this environment are incorrect. Please contact your administrator.');
            } else if (error.error.error && error.error.error === 'invalid_grant' && error.error.error_description === 'invalid_username_or_password') {
                return throwError('Invalid username and/or password. Please try again.')
            }
            return throwError('There was an error logging into the app. Please contact your administrator.');
        }
    }
}