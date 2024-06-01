export interface ExpirationInfo {
  date: string; // La date en format string
  timezone_type: number;
  timezone: string;
}
export interface User {
  id: number;
  username: string;
  lastName: string;
  firstName: string;
  email: string;
  phoneNumber: string;
  birthDate: string; // Je garde le type de birthDate en string pour correspondre à votre modèle
  address: string;
  addressComplement: string;
  postalCode: string;
  city: string;
  country: string;
  role: string;
}