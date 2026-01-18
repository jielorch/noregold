import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path:'',
        loadComponent: () => import('./features/pages/home/home').then(c => c.Home)
    },
    {
        path:'inventory',
        loadComponent:() => import('./features/pages/inventory/inventory').then(c => c.Inventory)
    }
];
