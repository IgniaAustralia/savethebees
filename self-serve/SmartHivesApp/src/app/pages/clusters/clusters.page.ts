import { Component } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
    selector: 'app-clusters',
    templateUrl: 'clusters.page.html',
    styleUrls: ['clusters.page.scss'],
})
export class ClustersPage {
    constructor(public navCtrl: NavController) { }
    clusters = [
        {
            id: 1,
            name: 'Pinjarrah Apiary'
        },
        {
            id: 2,
            name: 'Max\'s place'
        }
    ];
    goToDetails(id: string) {
        this.navCtrl.navigateForward('/home/(clusters:clusters/' + id + ')');
    }
}
