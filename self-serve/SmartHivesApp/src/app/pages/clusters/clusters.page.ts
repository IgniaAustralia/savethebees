import { Component } from '@angular/core';
import { NavController } from '@ionic/angular';
import { ActionSheetController } from '@ionic/angular';

@Component({
    selector: 'app-clusters',
    templateUrl: 'clusters.page.html',
    styleUrls: ['clusters.page.scss'],
})
export class ClustersPage {
    constructor(
        private navCtrl: NavController,
        private actionSheetController: ActionSheetController) { }
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

    async presentActionSheet() {
        const actionSheet = await this.actionSheetController.create({
            header: 'Action menu',
            buttons: [{
                text: 'Edit details',
                role: 'destructive',
                icon: 'create',
                handler: () => {
                    console.log('Edit clicked');
                }
            }, {
                text: 'Change status',
                icon: 'share',
                handler: () => {
                    console.log('Change status clicked');
                }
            }, {
                text: 'Log event',
                icon: 'add-circle',
                handler: () => {
                    console.log('Log event clicked');
                }
            }, {
                text: 'Cancel',
                icon: 'close',
                role: 'cancel',
                handler: () => {
                    actionSheet.dismiss();
                }
            }]
        });
        await actionSheet.present();
    }
}
