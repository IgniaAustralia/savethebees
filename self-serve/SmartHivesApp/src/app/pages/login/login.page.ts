import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NavController } from '@ionic/angular';

@Component({
    selector: 'app-login',
    templateUrl: 'login.page.html',
    styleUrls: ['login.page.scss'],
})
export class LoginPage implements OnInit {
    loginForm: FormGroup    

    constructor(private navCtrl: NavController, private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: new FormControl('', { validators: Validators.required, updateOn: 'blur' }),
            password: new FormControl('', { validators: Validators.required, updateOn: 'blur' })
        });
    }

    logIntoApp() {
        if (this.loginForm.valid) {
            this.navCtrl.navigateForward('home');
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