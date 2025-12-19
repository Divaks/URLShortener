import { Routes } from '@angular/router';
import { LoginComponent } from './featured/auth/login/login.component';
import {DashboardComponent} from './featured/dashboard/dashboard.component';
import {UrlDetailsComponent} from './featured/url-details/url-details.component';
import {authGuard} from './core/guards/auth-guard';
import {RegisterComponent} from './featured/register/register.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'urls', component: DashboardComponent },
  {
    path: 'urls/:id',
    component: UrlDetailsComponent,
    canActivate: [authGuard]
  },
  { path: 'register', component: RegisterComponent },
];
