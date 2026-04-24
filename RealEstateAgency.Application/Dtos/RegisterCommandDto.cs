namespace RealEstateAgency.Application.Dtos;

public record RegisterCommandDto(
    string Login,
    string Email,
    string Password,
    Stream AvatarStream,
    string AvatarFileName,
    string Name,
    string Surname,
    int Age,
    string PhoneNumber
);