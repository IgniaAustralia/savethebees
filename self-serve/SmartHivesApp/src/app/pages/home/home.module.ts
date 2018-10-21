import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { HomeRoutingModule } from './home.router.module';

import { HomePage } from './home.page';
import { ClustersModule } from '../clusters/clusters.module';
import { HivesModule } from '../hives/hives.module';
import { NotificationsModule } from '../notifications/notifications.module';
import { SettingsModule } from '../settings/settings.module';

@NgModule({
    imports: [
        IonicModule,
        CommonModule,
        FormsModule,
        HomeRoutingModule,
        ClustersModule,
        HivesModule,
        NotificationsModule,
        SettingsModule
    ],
    declarations: [HomePage]
})
export class HomeModule { }
