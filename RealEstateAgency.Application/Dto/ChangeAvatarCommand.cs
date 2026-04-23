namespace RealEstateAgency.Application.Dto;

public record ChangeAvatarCommand(Guid UserId, Stream FileStream, string FileName);