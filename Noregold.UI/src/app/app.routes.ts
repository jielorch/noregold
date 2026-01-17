import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path:'',
        loadComponent: () => import('./features/pages/home/home').then(c => c.Home)
    }
];
