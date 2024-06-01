import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './views/auth/auth.component';
import { UserListComponent } from './views/user/user-list/user-list.component';
import { AuthGuard } from './auth.guard';
import { UserProfilComponent } from './views/user/user-profil/user-profil.component';
import { CreateReservationComponent } from './views/reservations/create-reservation/create-reservation.component';
import { HomeComponent } from './views/home/home.component';
import { AdminGuard } from './admin.guard';
import { CreateAppartementComponent } from './views/appartement/create-appartement/create-appartement.component';
import { ReservationComponent } from './views/reservations/reservation/reservation.component';

const routes: Routes = [
  { path: 'login', component: AuthComponent },
  {
    path: 'user-list',
    component: UserListComponent,
    canActivate:[AdminGuard]
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path : 'profil',
    component : UserProfilComponent
  },
  {
    path: 'reservations',
    component: ReservationComponent
  },
  {
    path: 'appartement',
    loadChildren: () =>
      import('./views/appartement/appartement.module').then((m) => m.AppartementModule),
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
