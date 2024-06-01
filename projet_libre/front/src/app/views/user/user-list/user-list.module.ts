import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { UserListComponent } from './user-list.component';



@NgModule({
  declarations: [UserListComponent],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
  ]
})
export class UserListModule { }
