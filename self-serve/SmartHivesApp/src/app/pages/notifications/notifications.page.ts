import { Component } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
    selector: 'app-notifications',
    templateUrl: 'notifications.page.html',
    styleUrls: ['notifications.page.scss'],
})
export class NotificationsPage {
    constructor(public navCtrl: NavController) { }
}
