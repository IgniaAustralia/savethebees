import { Component } from '@angular/core';
import { ActionSheetController } from '@ionic/angular';

@Component({
    selector: 'app-hive-details',
    templateUrl: 'hive-details.page.html',
    styleUrls: ['hives.page.scss'],
})

export class HiveDetailsPage {
    name = "Hive 27";

    constructor(public actionSheetController: ActionSheetController) { }

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
