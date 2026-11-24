using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Domain.Common.Storage;

public interface IImageValidator
{
    ImageValidationResult IsValid(byte[] bytes, string optionsName);
}