import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { settings } from '../../environments/environment';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class UserService {
    currentUser: User;
    constructor(private httpClient: HttpClient) { }

    retrieveUser(username: string, accessToken: string): Promise<User> {
        return new Promise((resolve, reject) => {
            const url = settings.baseApiUrl + settings.endPoints.getUsers + '?userName=' + username;
            const rejectMessage = 'There was an error retrieving user details. Please contact your administrator.';
            this.httpClient.get(url, {
                headers: new HttpHeaders({
                    'Authorization': 'Bearer ' + accessToken
                }),
                observe: 'response'
            }).subscribe(response => {
                // Check if the response does not have a body
                if (!response.body) {
                    reject(rejectMessage);
                    return;
                }
                // Convert response into array of users and return if there is one
                const users = response.body as Array<User>;
                if (!users || users.length === 0) {
                    reject(rejectMessage);
                    return;
                }
                resolve(users[0]);
            }, () => {
                reject(rejectMessage);
            })
        });
    }
}