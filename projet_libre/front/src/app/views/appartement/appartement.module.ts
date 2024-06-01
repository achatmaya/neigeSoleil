import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CreateAppartementComponent } from './create-appartement/create-appartement.component';
import { ViewAppartementComponent } from './view-appartement/view-appartement.component';
import { AppartementRoutingModule } from './appartement-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { UpdateAppartementComponent } from './update-appartement/update-appartement.component';



@NgModule({
  declarations: [CreateAppartementComponent,ViewAppartementComponent, UpdateAppartementComponent],
  imports: [
    CommonModule,
    FormsModule,
    AppartementRoutingModule,
    MatDialogModule,
    MatButtonModule
  ]
})
export class AppartementModule { }
