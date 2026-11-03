namespace Amsberg_digital_universe.Domain.Common.Exceptions;

public class ConfirmTokenInvalidException:Exception
{
    public ConfirmTokenInvalidException() : base(){}
    public ConfirmTokenInvalidException(string message) : base(message){}
    public ConfirmTokenInvalidException(string message, Exception inner) : base(message, inner){}
}