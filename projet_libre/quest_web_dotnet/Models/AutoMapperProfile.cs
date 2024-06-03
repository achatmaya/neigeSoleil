namespace quest_web.Models;
using AutoMapper;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Apartment, ApartmentDto>();
        CreateMap<Apartment, ApartmentDto.ApartmentWithReservationsDto>();
        CreateMap<Apartment, ApartmentDto.ApartmentWithReservationsAndUsersDto>();
        CreateMap<ApartmentDto.ApartmentUpdateModel, Apartment>();

        CreateMap<User, UserDto>();
        CreateMap<User, UserWithReservationsDto>();
        CreateMap<User, UserWithApartmentsDto>();
        CreateMap<User, UserWithReservationsAndApartmentsDto>();
        CreateMap<UserUpdateModel, User>();

        CreateMap<Reservation, ReservationDto>();
        CreateMap<Reservation, ReservationWithUserDto>();
        CreateMap<Reservation, ReservationWithApartmentDto>();
        CreateMap<Reservation, ReservationWithUserAndApartmentDto>();
        CreateMap<ReservationUpdateModel, Reservation>();   
        CreateMap<Equipment, EquipmentDto>();
        CreateMap<Equipment, EquipmentWithApartmentDto>();
        CreateMap<EquipmentUpdateModel, Equipment>();
    }
}

