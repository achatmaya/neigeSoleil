export interface ApartmentCreate {
    code: string;
    buildingNumber: string;
    apartmentNumber: string;
    address: string;
    addressComplement: string;
    postalCode: string;
    country: string;
    floor: number;
    additionalInfo: string;
    apartmentType: string;
    area: number;
    exposure: string;
    capacity: number;
    distanceToSlope: number;
    Amount: number,
    imageUrls: string;
  };
  export interface Apartment{
    id: number;
    code: string;
    buildingNumber: string;
    apartmentNumber: string;
    address: string;
    addressComplement: string;
    postalCode: string;
    country: string;
    floor: number;
    additionalInfo: string;
    apartmentType: string;
    area: number;
    exposure: string;
    capacity: number;
    distanceToSlope: number;
    Amount: number;
    imageUrls: string;
  }
