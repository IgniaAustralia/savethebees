import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { HivesPage } from './hives.page';
import { HiveDetailsPage } from './hive-details.page';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule,
        RouterModule.forChild([
            {
                path: '',
                component: HivesPage
            },
            {
                path: ':id',
                component: HiveDetailsPage
            }
        ])
    ],
    declarations: [HivesPage, HiveDetailsPage]
})
export class HivesModule { }