import { Component } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
    selector: 'app-hives',
    templateUrl: 'hives.page.html',
    styleUrls: ['hives.page.scss'],
})
export class HivesPage {
    constructor(public navCtrl: NavController) { }
    hives = [
        {
            id: 1,
            name: 'Hive 27'
        },
        {
            id: 2,
            name: 'Hive 36'
        }
    ];
    goToDetails(id: string) {
        this.navCtrl.navigateForward('/home/(hives:hives/' + id + ')');
    }
}
