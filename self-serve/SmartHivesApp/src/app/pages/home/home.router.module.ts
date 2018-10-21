import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomePage } from './home.page';
import { ClustersPage } from '../clusters/clusters.page';
import { ClusterDetailsPage } from '../clusters/cluster-details.page';
import { HivesPage } from '../hives/hives.page';
import { HiveDetailsPage } from '../hives/hive-details.page';

const routes: Routes = [
    {
        path: 'home',
        component: HomePage,
        children: [
            {
                path: '',
                redirectTo: '/home/(clusters:clusters)',
                pathMatch: 'full',
            },
            {
                path: 'clusters',
                outlet: 'clusters',
                component: ClustersPage
            },
            {
                path: 'clusters/:id',
                outlet: 'clusters',
                component: ClusterDetailsPage
            },
            {
                path: 'hives/:id',
                outlet: 'clusters',
                component: HiveDetailsPage
            }
            {
                path: 'hives',
                outlet: 'hives',
                component: HivesPage
            },
            {
                path: 'hives/:id',
                outlet: 'hives',
                component: HiveDetailsPage
            }
        ]
    },
    {
        path: '',
        redirectTo: '/home/(clusters:clusters)',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule { }
