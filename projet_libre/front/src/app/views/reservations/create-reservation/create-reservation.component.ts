import { ReservationService } from 'src/backend/reservation/reservation.service';
import { AuthService } from 'src/backend/user/auth.service';
import { Component, Inject ,OnInit} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { reservation } from 'src/backend/reservation/reservation.interface';
import { User } from 'src/backend/user/user.interface';


@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.scss']
})


export class CreateReservationComponent implements OnInit {
  showModal = false;
  currentUser: User | null = null;
  reservation: reservation= {
    status: '',
    reservationDate: new Date(),
    startDate: new Date(),
    endDate: new Date(),
    numberOfPeople: 0,
    amount: 0,
    reservationNumber: '',
    ApartmentId: 0,
    UserId: 0,
    id : 0
  };
  router: any;

  constructor(
    public dialogRef: MatDialogRef<CreateReservationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private reservationService: ReservationService,
    private authService: AuthService 
  ) { }

  ngOnInit(): void {
    
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (user) {
        this.reservation. UserId= user.id;
      }
    });
    console.log(this.data.appartement.id)
    this.reservation.ApartmentId = this.data.appartement.id;
  }
  onSubmit(): void {
    this.reservationService.addReservation(this.reservation).subscribe(
      response => {
        console.log('Réservation créée avec succès', response);
        this.showModal = true;
        this.dialogRef.close(true);
      },
      error => {
        console.error('Erreur lors de la création de la réservation', error);
      }
    );
  }
  closeCancel():void{
    this.dialogRef.close(true)
  }

  closeModal(): void {
    this.showModal = false;
    this.router.navigate(['/home']); // Naviguez vers la page d'accueil

  }
}






