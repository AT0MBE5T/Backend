namespace RealEstateAgency.Application.Interfaces.Services;

public interface ICookieService
{
    void SetRefreshTokenCookie(string token);
}