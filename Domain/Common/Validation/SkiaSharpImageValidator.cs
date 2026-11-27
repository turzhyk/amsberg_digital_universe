using Microsoft.Extensions.Options;

namespace Amsberg_digital_universe.Domain.Common.Storage;

using SkiaSharp;

public class SkiaSharpImageValidator(IOptionsMonitor<ImageValidationOptions> options) : IImageValidator
{
    public ImageValidationResult IsValid(byte[] bytes, string optionsName)
    {
        var specificOptions = options.Get(optionsName);

        if (bytes.Length > specificOptions.MaxFileSize)
            return ImageValidationResult.SizeTooBig;

        if (bytes.Length == 0)
            return ImageValidationResult.Invalid;

        try
        {
            using var stream = new MemoryStream(bytes);
            using var codec = SKCodec.Create(stream);

            if (codec == null)
                return ImageValidationResult.WrongFormat;

            var format = codec.EncodedFormat.ToString();

            if (!specificOptions.AllowedFormats.Contains(format,
                    StringComparer.OrdinalIgnoreCase))
                return ImageValidationResult.WrongFormat;

            var width = codec.Info.Width;
            var height = codec.Info.Height;

            if (height > specificOptions.MaxHeight ||
                width > specificOptions.MaxWidth)
                return ImageValidationResult.ResolutionTooBig;
        }
        catch (Exception)
        {
            return ImageValidationResult.Invalid;
        }

        return ImageValidationResult.Valid;
    }
}