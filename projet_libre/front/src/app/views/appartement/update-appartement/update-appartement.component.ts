import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppartementService } from 'src/backend/appartement/appartement.service';
import { Apartment, ApartmentCreate } from 'src/backend/appartement/appartement.interface';
@Component({
  selector: 'app-update-appartement',
  templateUrl: './update-appartement.component.html',
  styleUrls: ['./update-appartement.component.scss']
})
export class UpdateAppartementComponent {
  apartment:Apartment;
  originalApartment: Apartment;

  constructor(
    public dialogRef: MatDialogRef<UpdateAppartementComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private appartementService: AppartementService,
  ) {
    this.apartment = { ...data.appartement };
    this.originalApartment = { ...data.apartment };
  }

  onUpdate(): void {
    const updatedFields = this.getUpdatedFields();
    if (Object.keys(updatedFields).length > 0) {
      this.appartementService.updateAppartement(this.apartment.id.toString(), updatedFields).subscribe({
        next: (updatedApartment) => {
          this.dialogRef.close(updatedApartment);
        },
        error: (error) => console.error(error)
      });
    } else {
      this.dialogRef.close();
    }
  }

  private getUpdatedFields(): Partial<Apartment> {
    const updatedFields: Partial<Apartment> = {};
    (Object.keys(this.apartment) as (keyof Apartment)[]).forEach(key => {
      if (this.apartment[key] !== this.originalApartment[key]) {
        updatedFields[key] = this.apartment[key] as any; // Assert type any to handle string | number
      }
    });
    return updatedFields;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
