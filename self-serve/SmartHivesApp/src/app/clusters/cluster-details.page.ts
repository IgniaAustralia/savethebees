import { Component } from '@angular/core';
import { NavController } from '@ionic/angular';

@Component({
    selector: 'app-cluster-details',
    templateUrl: 'cluster-details.page.html',
    styleUrls: ['clusters.page.scss'],
})
export class ClusterDetailsPage {
    constructor(public navCtrl: NavController) { }
    name = "Pinjarrah Apiary";
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
        this.navCtrl.navigateForward('/hives/' + id);
    }
}
