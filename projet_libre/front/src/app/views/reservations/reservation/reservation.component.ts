import { Component } from '@angular/core';
import { reservation } from 'src/backend/reservation/reservation.interface';
import { ReservationService } from 'src/backend/reservation/reservation.service';
import { AuthService } from 'src/backend/user/auth.service';
import { User } from 'src/backend/user/user.interface';
import { MatDialog } from '@angular/material/dialog';
import { UpdateReservationComponent } from '../update-reservation/update-reservation.component';
import { ModalExempleComponent } from 'src/app/extra_composant/modal-exemple/modal-exemple.component';

@Component({
  selector: 'app-reservation',
  templateUrl: './reservation.component.html',
  styleUrls: ['./reservation.component.scss']
})
export class ReservationComponent {
  reservations: reservation[]=[];
  currentUser: User | null = null;
  constructor(private reservationService: ReservationService, private authService : AuthService, public dialog: MatDialog){
    this.authService.currentUser$.subscribe(user => this.currentUser = user);
  }

  ngOnInit(): void {
    if(this.currentUser?.role == "ROLE_OWNER"){
      this.reservationService.listAllByOwner(this.currentUser.id).subscribe({
        next: (data: reservation[]) => this.reservations = data,
        error: (error: any) => console.error(error)
      });
    }
  }
  viewReservationDetails(reservation : reservation): void{
    const dialogRef = this.dialog.open(UpdateReservationComponent, {
      width: '500px',
      data: { reservation }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.openModal('Succès', 'La reservation a été modifié avec succès !');
      }
    });
  }
  openModal(title: string, message: string) {
    this.dialog.open(ModalExempleComponent, {
      data: {
        title: title,
        message: message
      }
    });
  }
}