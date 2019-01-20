import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';

@Component({
    selector: 'app-login',
    templateUrl: 'login.page.html',
    styleUrls: ['login.page.scss'],
})
export class LoginPage implements OnInit {
    loginForm: FormGroup;
    loginError: string;

    constructor(
        private router: Router,
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private userService: UserService) { }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: new FormControl('', { validators: Validators.required, updateOn: 'blur' }),
            password: new FormControl('', { validators: Validators.required, updateOn: 'blur' })
        });
    }

    logIntoApp() {
        this.loginError = null;
        if (this.loginForm.valid) {
            this.authService.loginUser(this.loginForm.get('username').value, this.loginForm.get('password').value).then(accessToken => {
                // Retrieve user and set the current user
                this.userService.retrieveUser(this.loginForm.get('username').value, accessToken).then(user => {
                    this.userService.currentUser = user;
                    this.loginForm.get('username').setValue('');
                    this.loginForm.get('username').markAsPristine();
                    this.loginForm.get('username').markAsUntouched();
                    this.loginForm.get('password').setValue('');
                    this.loginForm.get('password').markAsPristine();
                    this.loginForm.get('password').markAsUntouched();
                    this.router.navigate(['/home']);                    
                }, error => this.handleLoginError(error));
            }, error => this.handleLoginError(error));
        } else {
            this.loginForm.get('username').markAsTouched();
            this.loginForm.get('password').markAsTouched();
        }
    }

    errorMessages = {
        username: [
            { type: 'required', message: 'Please enter your username.' }
        ],
        password: [
            { type: 'required', message: 'Please enter your password.' }
        ]
    }

    private handleLoginError(errorMessage) {
        this.loginError = errorMessage;
        this.loginForm.get('password').setValue('');
        this.loginForm.get('password').markAsPristine();
        this.loginForm.get('password').markAsUntouched();
    }
}