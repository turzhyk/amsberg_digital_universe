namespace Amsberg_digital_universe.Domain.User;

public enum EmailConfirmResult
{
    Success,
    Expired,
    Wrong,
    AlreadyConfirmed,
}