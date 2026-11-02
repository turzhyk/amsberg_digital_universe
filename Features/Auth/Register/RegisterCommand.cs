using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Amsberg_digital_universe.Features.Auth.Register;

public record RegisterCommand(string Email, string Password) : IRequest<RegisterResponse>;