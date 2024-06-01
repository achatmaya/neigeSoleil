import { ReservationService } from 'src/backend/reservation/reservation.service';
import { AuthService } from 'src/backend/user/auth.service';
import { Component, Inject ,OnInit} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { reservation } from 'src/backend/reservation/reservation.interface';
import { User } from 'src/backend/user/user.interface';

@Component({
  selector: 'app-update-reservation',
  templateUrl: './update-reservation.component.html',
  styleUrls: ['./update-reservation.component.scss']
})
export class UpdateReservationComponent {
  reservation:reservation;
  originalReservation: reservation;
  constructor(
    public dialogRef: MatDialogRef<UpdateReservationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private reservationService: ReservationService,
    private authService: AuthService 
  ) 
  {
    this.reservation = { ...data.appartement };
    this.originalReservation = { ...data.apartment };
  }

  // onUpdate(): void {
  //   const updatedFields = this.getUpdatedFields();
  //   if (Object.keys(updatedFields).length > 0) {
  //     this.reservationService.updateAppartement(this.appartement.id.toString(), updatedFields).subscribe({
  //       next: (updatedApartment) => {
  //         this.dialogRef.close(updatedApartment);
  //       },
  //       error: (error) => console.error(error)
  //     });
  //   } else {
  //     this.dialogRef.close();
  //   }
  // }
  // private getUpdatedFields(): Partial<Apartment> {
  //   const updatedFields: Partial<Apartment> = {};
  //   (Object.keys(this.apartment) as (keyof Apartment)[]).forEach(key => {
  //     if (this.apartment[key] !== this.originalApartment[key]) {
  //       updatedFields[key] = this.apartment[key] as any; // Assert type any to handle string | number
  //     }
  //   });
  //   return updatedFields;
  // }

  onNoClick(): void {
    this.dialogRef.close();
  }
   

}
