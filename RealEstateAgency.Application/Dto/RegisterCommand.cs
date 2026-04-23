namespace RealEstateAgency.Application.Dto;

public record RegisterCommand(
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