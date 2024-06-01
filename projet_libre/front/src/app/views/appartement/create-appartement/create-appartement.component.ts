import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppartementService } from 'src/backend/appartement/appartement.service';
import { Apartment, ApartmentCreate } from 'src/backend/appartement/appartement.interface';
import { ImageUploadService } from 'src/backend/image/image-upload.service';
import { forkJoin } from 'rxjs';
import { ModalExempleComponent } from 'src/app/extra_composant/modal-exemple/modal-exemple.component';

@Component({
  selector: 'app-appartement',
  templateUrl: './create-appartement.component.html',
  styleUrls: ['./create-appartement.component.scss']
})
export class CreateAppartementComponent {
  appartements: Apartment[] = [];
  apartment: ApartmentCreate = {
    code: '',
    buildingNumber: '',
    apartmentNumber: '',
    address: '',
    addressComplement: '',
    postalCode: '',
    country: '',
    floor: 0,
    additionalInfo: '',
    apartmentType: '',
    area: 0,
    exposure: '',
    capacity: 0,
    distanceToSlope: 0,
    Amount: 0,
    imageUrls: ''
  };
  selectedFiles: File[] = [];

  constructor(
    private serviceAppartement: AppartementService, 
    private imageUploadService: ImageUploadService,
    private myModalDialogue : MatDialog

  ) {}
  openDialog(title: string, message: string) {
    this.myModalDialogue.open(ModalExempleComponent, {
      data: {
        title: title,
        message: message
      }
    });
  }
  onFileSelected(event: any) {
    this.selectedFiles = Array.from(event.target.files); // Ensure selectedFiles is an array
  }

  onSubmit() {
    if (!Array.isArray(this.selectedFiles) || this.selectedFiles.length === 0) {
      console.error('No files selected');
      return;
    }

    const uploadObservables = this.selectedFiles.map(file => this.imageUploadService.uploadImage(file));

    forkJoin(uploadObservables).subscribe(
      (responses: any[]) => {
        const imageUrls = responses.map(response => response.url).filter(url => url !== null);
        if (imageUrls.length === 0) {
          console.error('No valid image URLs received');
          return;
        }
        this.apartment.imageUrls = JSON.stringify(imageUrls);

        this.serviceAppartement.addAppartement(this.apartment).subscribe(
          (response: any) => {
            console.log('Apartment created successfully', response);
            this.openDialog('Succès', 'L\'appartement a été créé avec succès !');
          },
          (error: any) => {
            console.error('Error creating apartment', error);
            this.openDialog('Erreur', 'Une erreur est survenue lors de la création de l\'appartement.');
          }
        );
      },
      (error: any) => {
        console.error('Error uploading images', error);
        this.openDialog('Erreur', 'Une erreur est survenue lors de l\'upload des images.');
      }
    );
  }
}
