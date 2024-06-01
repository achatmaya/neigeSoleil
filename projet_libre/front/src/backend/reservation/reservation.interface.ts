export interface reservationCreate{
    status: string;
    reservationDate: Date;
    startDate: Date;
    endDate: Date;
    numberOfPeople: number;
    amount: number;
    ApartmentId: number;
    UserId: number;
    reservationNumber: string;
}
export interface reservation{
    id: number;
    status: string;
    reservationDate: Date;
    startDate: Date;
    endDate: Date;
    numberOfPeople: number;
    ApartmentId: number;
    UserId: number;
    amount: number;
    reservationNumber: string;
}