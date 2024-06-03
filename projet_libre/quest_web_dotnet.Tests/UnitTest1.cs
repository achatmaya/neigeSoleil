using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using quest_web.Data;
using quest_web.Controllers;
using quest_web.Models;
using quest_web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;

namespace quest_web_dotnet.Tests
{
    public class UnitTest1
    {
        private ApiDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApiDbContext(options);
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            return config.CreateMapper();
        }

        private ClaimsPrincipal GetClaimsPrincipal(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public void Register_ReturnsCreatedResult_WhenUserIsValid()
        {
            // Arrange
            var mockJwtTokenUtil = new Mock<JwtTokenUtil>(null, null, null);
            var context = GetDbContext();
            var controller = new AuthenticationController(context, mockJwtTokenUtil.Object);

            var user = new User
            {
                Username = "achat",
                Password = "123",
                LastName = "Doe",
                FirstName = "John",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                BirthDate = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                AddressComplement = "Apt 4",
                PostalCode = "12345",
                City = "Anytown",
                Country = "Anycountry",
                Role = "ROLE_USER"
            };

            // Act
            var result = controller.Register(user);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(AuthenticationController.Register), createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public void AddApartment_ReturnsCreatedResult_WhenUserIsOwner()
        {
            // Arrange
            var context = GetDbContext();
            var mapper = GetMapper();

            var user = new User
            {
                Username = "owneruser",
                Password = "hashedpassword",
                Role = "ROLE_OWNER",
                Email = "owner@example.com",
                LastName = "Owner",
                FirstName = "User",
                BirthDate = new DateTime(1980, 1, 1),
                Address = "Owner Address",
                PostalCode = "00000",
                City = "Owner City",
                Country = "Owner Country",
                PhoneNumber = "0000000000"
            };
            context.Users.Add(user);
            context.SaveChanges();

            var controller = new ApartmentController(context, mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = GetClaimsPrincipal(user.Username, user.Role) }
                }
            };

            var apartment = new Apartment
            {
                Code = "APT001",
                BuildingNumber = "12",
                ApartmentNumber = "34B",
                Address = "123 Main St",
                AddressComplement = "Apt 34B",
                PostalCode = "12345",
                Country = "Anycountry",
                Floor = 3,
                 ApartmentType = "2BHK",
                Area = 75,
                Exposure = "North",
                Capacity = 4,
                DistanceToSlope = 0,
                ImageUrls = "url/to/photo",
                Amount = 100
            };

            // Act
            var result = controller.AddApartment(apartment);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ApartmentController.GetOneApartments), createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public void CreateReservation_ReturnsCreatedResult_WhenReservationIsValid()
        {
            // Arrange
            var context = GetDbContext();
            var mapper = GetMapper();

            var user = new User
            {
                Username = "reservationuser",
                Password = "hashedpassword",
                Role = "ROLE_USER",
                Email = "reservation@example.com",
                LastName = "Reservation",
                FirstName = "User",
                BirthDate = new DateTime(1980, 1, 1),
                Address = "Reservation Address",
                PostalCode = "00000",
                City = "Reservation City",
                Country = "Reservation Country",
                PhoneNumber = "0000000000"
            };
            context.Users.Add(user);

            var apartment = new Apartment
            {
                Code = "APT002",
                BuildingNumber = "34",
                ApartmentNumber = "12A",
                Address = "456 Another St",
                AddressComplement = "Apt 12A",
                PostalCode = "67890",
                Country = "Anothercountry",
                Floor = 2,
                 ApartmentType = "3BHK",
                Area = 100,
                Exposure = "South",
                Capacity = 5,
                DistanceToSlope = 1,
                ImageUrls = "url/to/photo",
                Amount = 150
            };
            context.Apartments.Add(apartment);
            context.SaveChanges();

            var controller = new ReservationController(context, mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = GetClaimsPrincipal(user.Username, user.Role) }
                }
            };

            var reservationDto = new ReservationUpdateModel
            {
                ApartmentId = apartment.Id,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                Status = "Pending"  
            };

            // Act
            var result = controller.CreateReservation(reservationDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ReservationController.GetReservation), createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues["id"]);
        }

         [Fact]
        public void AddEquipment_ReturnsCreatedResult_WhenUserIsOwner()
        {
            // Arrange
            var context = GetDbContext();
            var mapper = GetMapper();

            var user = new User
            {
                Username = "equipmentowner",
                Password = "hashedpassword",
                Role = "ROLE_OWNER",
                Email = "owner@example.com",
                LastName = "Owner",
                FirstName = "User",
                BirthDate = new DateTime(1980, 1, 1),
                Address = "Owner Address",
                PostalCode = "00000",
                City = "Owner City",
                Country = "Owner Country",
                PhoneNumber = "0000000000"
            };
            context.Users.Add(user);

            var apartment = new Apartment
            {
                Code = "APT003",
                BuildingNumber = "56",
                ApartmentNumber = "78C",
                Address = "789 Another St",
                AddressComplement = "Apt 78C",
                PostalCode = "98765",
                Country = "Anothercountry",
                Floor = 4,
                 ApartmentType = "4BHK",
                Area = 120,
                Exposure = "West",
                Capacity = 6,
                DistanceToSlope = 2,
                ImageUrls = "url/to/photo",
                Amount = 200
            };
            context.Apartments.Add(apartment);
            context.SaveChanges();

            var managementContract = new ManagementContract
            {
                ApartmentId = apartment.Id,
                UserId = user.Id,
                Status = "Active",
                SignatureDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Code = Guid.NewGuid().ToString()
            };
            context.ManagementContracts.Add(managementContract);
            context.SaveChanges();

            var controller = new EquipmentController(context, mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = GetClaimsPrincipal(user.Username, user.Role) }
                }
            };

            var equipmentDto = new EquipmentUpdateModel
            {
                Name = "Air Conditioner",
                Quantity = 2,
                ApartmentId = apartment.Id
            };

            // Act
            var result = controller.AddEquipment(equipmentDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(EquipmentController.GetEquipment), createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues["id"]);
        }
        [Fact]
        public void UpdateApartment_ReturnsOkResult_WhenUpdateIsValid()
        {
            // Arrange
            var context = GetDbContext();
            var mapper = GetMapper();

            var user = new User
            {
                Username = "owneruser",
                Password = "hashedpassword",
                Role = "ROLE_OWNER",
                Email = "owner@example.com",
                LastName = "Owner",
                FirstName = "User",
                BirthDate = new DateTime(1980, 1, 1),
                Address = "Owner Address",
                PostalCode = "00000",
                City = "Owner City",
                Country = "Owner Country",
                PhoneNumber = "0000000000"
            };
            context.Users.Add(user);
            context.SaveChanges();

            var apartment = new Apartment
            {
                Code = "APT001",
                BuildingNumber = "12",
                ApartmentNumber = "34B",
                Address = "123 Main St",
                AddressComplement = "Apt 34B",
                PostalCode = "12345",
                Country = "Anycountry",
                Floor = 3,
                 ApartmentType = "2BHK",
                Area = 75,
                Exposure = "North",
                Capacity = 4,
                DistanceToSlope = 0,
                ImageUrls = "url/to/photo",
                Amount = 100
            };
            context.Apartments.Add(apartment);
            context.SaveChanges();

            var managementContract = new ManagementContract
            {
                ApartmentId = apartment.Id,
                UserId = user.Id,
                Status = "Active",
                SignatureDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Code = Guid.NewGuid().ToString()
            };
            context.ManagementContracts.Add(managementContract);
            context.SaveChanges();

            var controller = new ApartmentController(context, mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = GetClaimsPrincipal(user.Username, user.Role) }
                }
            };

            var updatedApartment = new ApartmentDto.ApartmentUpdateModel
            {
                BuildingNumber = "New Building Number",
                ApartmentNumber = "New Apartment Number"
            };

            // Act
            var result = controller.UpdateApartment(apartment.Id, updatedApartment);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedApartment = Assert.IsType<ApartmentDto>(okResult.Value);
            Assert.Equal("New Building Number", returnedApartment.BuildingNumber);
            Assert.Equal("New Apartment Number", returnedApartment.ApartmentNumber);
        }
    }
}
