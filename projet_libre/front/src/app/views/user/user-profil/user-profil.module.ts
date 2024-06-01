import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { UserProfilComponent } from './user-profil.component';



@NgModule({
  declarations: [UserProfilComponent],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
  ]
})
export class UserProfilModule { }
