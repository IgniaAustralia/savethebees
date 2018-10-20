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
            name: 'Cluster 1'
        },
        {
            id: 2,
            name: 'Cluster 2'
        }
    ];
    goToDetails(id: string) {
        this.navCtrl.navigateForward('/clusters/' + id);
    }
}
