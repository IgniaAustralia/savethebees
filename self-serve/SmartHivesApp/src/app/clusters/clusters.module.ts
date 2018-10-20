import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { ClustersPage } from './clusters.page';
import { ClusterDetailsPage } from './cluster-details.page';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule,
        RouterModule.forChild([
            {
                path: '',
                component: ClustersPage
            },
            {
                path: ':id',
                component: ClusterDetailsPage
            }
        ])
    ],
    declarations: [ClustersPage, ClusterDetailsPage]
})
export class ClustersModule { }