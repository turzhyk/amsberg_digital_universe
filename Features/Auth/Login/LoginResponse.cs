namespace Amsberg_digital_universe.Features.Auth.Login;

public record LoginResponse
{
    public string Token{get;set;}
    public string RefreshToken { get; set; }
}