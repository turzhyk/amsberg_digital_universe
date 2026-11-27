// using Microsoft.Extensions.Options;
// using SixLabors.ImageSharp;
//
// namespace Amsberg_digital_universe.Domain.Common.Storage;
//
// public class ImageSharpImageValidator(IOptionsMonitor<ImageValidationOptions> options) : IImageValidator
// {
//     
//     public ImageValidationResult IsValid(byte[] bytes, string optionsName)
//     {
//         var specificOptions = options.Get(optionsName);
//         
//         if (bytes.Length > specificOptions.MaxFileSize)
//             return ImageValidationResult.SizeTooBig;
//         if (bytes.Length == 0)
//             return ImageValidationResult.Invalid;
//         
//         try
//         {
//             var format = Image.DetectFormat(bytes);
//             if (!specificOptions.AllowedFormats.Contains(format.Name))
//                 return ImageValidationResult.WrongFormat;
//
//             var imgInfo = Image.Identify(bytes);
//
//             if (imgInfo.Height > specificOptions.MaxHeight || imgInfo.Width > specificOptions.MaxWidth)
//                 return ImageValidationResult.ResolutionTooBig;
//         }
//         catch (UnknownImageFormatException)
//         {
//             return ImageValidationResult.WrongFormat;
//         }
//         catch (InvalidImageContentException)
//         {
//             return ImageValidationResult.Invalid;
//         }
//
//         return ImageValidationResult.Valid;
//     }
// }