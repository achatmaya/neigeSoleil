import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './views/auth/auth.component';
import { AuthModule } from './views/auth/auth.module';
import { CoreHttpModule } from 'src/core/http/core-http.module';
import { HttpClientModule } from '@angular/common/http';
import { UserListModule } from './views/user/user-list/user-list.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserProfilModule } from './views/user/user-profil/user-profil.module';
import { CreateReservationComponent } from './views/reservations/create-reservation/create-reservation.component';
import { ReservationComponent } from './views/reservations/reservation/reservation.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { HomeModule } from './views/home/home.module';
import { AppartementModule } from './views/appartement/appartement.module';
import { ModalExempleComponent } from './extra_composant/modal-exemple/modal-exemple.component';
import { UpdateReservationComponent } from './views/reservations/update-reservation/update-reservation.component';


@NgModule({
  declarations: [
    AppComponent,
    CreateReservationComponent,
    ReservationComponent,
    ModalExempleComponent,
    UpdateReservationComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    HttpClientModule,
    CoreHttpModule,
    UserListModule,
    FormsModule,
    RouterModule,
    UserProfilModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatButtonModule,
    CommonModule,
    HomeModule,
    AppartementModule
  ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
