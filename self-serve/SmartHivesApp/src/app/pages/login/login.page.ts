import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NavController } from '@ionic/angular';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: 'login.page.html',
    styleUrls: ['login.page.scss'],
})
export class LoginPage implements OnInit {
    loginForm: FormGroup;
    loginError: string;

    constructor(private navCtrl: NavController, private formBuilder: FormBuilder, private authService: AuthService) { }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: new FormControl('', { validators: Validators.required, updateOn: 'blur' }),
            password: new FormControl('', { validators: Validators.required, updateOn: 'blur' })
        });
    }

    logIntoApp() {
        this.loginError = null;
        if (this.loginForm.valid) {
            this.authService.retrieveAuthToken(this.loginForm.get('username').value, this.loginForm.get('password').value).subscribe(response => {
                this.loginForm.get('username').setValue('');
                this.loginForm.get('username').markAsPristine();
                this.loginForm.get('username').markAsUntouched();
                this.loginForm.get('password').setValue('');
                this.loginForm.get('password').markAsPristine();
                this.loginForm.get('password').markAsUntouched();
                this.navCtrl.navigateForward('home');
            }, error => {
                this.loginError = error;
                this.loginForm.get('password').setValue('');
                this.loginForm.get('password').markAsPristine();
                this.loginForm.get('password').markAsUntouched();
            });
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
}