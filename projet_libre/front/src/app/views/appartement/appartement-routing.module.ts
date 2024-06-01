import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateAppartementComponent } from './create-appartement/create-appartement.component';
import { OwnerGuard } from 'src/app/owner.guard';
import { ViewAppartementComponent } from './view-appartement/view-appartement.component';

const routes: Routes = [
  { path: 'new', component: CreateAppartementComponent, canActivate:[OwnerGuard] },
  { path: ':id', component: ViewAppartementComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppartementRoutingModule { }
