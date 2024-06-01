import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ModalExempleComponent } from 'src/app/extra_composant/modal-exemple/modal-exemple.component';
import { Apartment } from 'src/backend/appartement/appartement.interface';
import { AppartementService } from 'src/backend/appartement/appartement.service';
import { CreateReservationComponent } from '../../reservations/create-reservation/create-reservation.component';
import { AuthService } from 'src/backend/user/auth.service';

@Component({
  selector: 'app-view-appartement',
  templateUrl: './view-appartement.component.html',
  styleUrls: ['./view-appartement.component.scss']
})
export class ViewAppartementComponent implements OnInit {
  apartment: Apartment | null = null;

  constructor(
    private route: ActivatedRoute,
    public authService: AuthService,
    private appartementService: AppartementService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    const apartmentId = this.route.snapshot.paramMap.get('id');
    if (apartmentId) {
      this.getApartmentDetails(parseInt(apartmentId));
    }
  }

  getApartmentDetails(id: number) {
    this.appartementService.getAppartementById(id).subscribe({
      next: (data: Apartment) => this.apartment = data,
      error: (error) => console.error(error)
    });
  }

  getImageUrls(imageUrls: string): string[] {
    try {
      return JSON.parse(imageUrls);
    } catch (e) {
      console.error('Error parsing imageUrls:', e);
      return [];
    }
  }

  openModal(title: string, message: string) {
    this.dialog.open(ModalExempleComponent, {
      data: {
        title: title,
        message: message
      }
    });
  }

  openCreateReservationModal(appartement: Apartment): void {
    const dialogRef = this.dialog.open(CreateReservationComponent, {
      width: '500px',
      data: { appartement }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.openModal('Succès', 'Réservation effectuée avec succès !');
      }
    });
  }


}
