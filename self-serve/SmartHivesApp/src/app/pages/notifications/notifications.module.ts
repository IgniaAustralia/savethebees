import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NotificationsPage } from './notifications.page';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule
    ],
    declarations: [NotificationsPage]
})
export class NotificationsModule { }