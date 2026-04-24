namespace RealEstateAgency.Application.Dtos;

public record ChangeAvatarCommandDto(Guid UserId, Stream FileStream, string FileName);