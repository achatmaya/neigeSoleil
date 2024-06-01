import { Component, OnInit } from '@angular/core';
import { Apartment } from 'src/backend/appartement/appartement.interface';
import { AppartementService } from 'src/backend/appartement/appartement.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateReservationComponent } from '../reservations/create-reservation/create-reservation.component';
import { UpdateAppartementComponent } from '../appartement/update-appartement/update-appartement.component';
import { ModalExempleComponent } from 'src/app/extra_composant/modal-exemple/modal-exemple.component';
import { AuthService } from 'src/backend/user/auth.service';
import { reservation } from 'src/backend/reservation/reservation.interface';
import { UpdateReservationComponent } from '../reservations/update-reservation/update-reservation.component';

interface SearchCriteria {
  postalCode?: string;
  size?: number;
  capacity?: number;
  country?: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  appartements: Apartment[] = [];
  filteredAppartements: Apartment[] = [];
  searchCriteria: SearchCriteria = {};

  constructor(private serviceAppartement: AppartementService, public dialog: MatDialog, public authService: AuthService) { }

  ngOnInit() {
    this.getListAppartement();
  }

  getListAppartement() {
    this.serviceAppartement.getlistAppartement().subscribe({
      next: (data: Apartment[]) => {
        this.appartements = data;
        this.filteredAppartements = data; // Initialize filteredAppartements with all apartments
      },
      error: (error) => console.error(error)
    });
  }

  getFirstImageUrl(imageUrls: string): string {
    try {
      const urls = JSON.parse(imageUrls);
      return urls.length > 0 ? urls[0] : '';
    } catch (e) {
      console.error('Error parsing imageUrls:', e);
      return '';
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

  openUpdateAppartementModal(appartement: Apartment): void {
    const dialogRef = this.dialog.open(UpdateAppartementComponent, {
      width: '500px',
      data: { appartement }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.openModal('Succès', 'L\'appartement a été modifié avec succès !');
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

  onSearch(): void {
    this.filteredAppartements = this.appartements.filter(appartement => {
      const matchesSize = this.searchCriteria.size ? appartement.area >= this.searchCriteria.size : true;
      const matchesCapacity = this.searchCriteria.capacity ? appartement.capacity >= this.searchCriteria.capacity : true;
      return matchesSize && matchesCapacity;
    });
  }
}
