namespace Amsberg_digital_universe.Domain.Common;

public class SeederException : Exception
{
    public SeederException() : base()
    {
    }

    public SeederException(string message) : base(message)
    {
    }
}