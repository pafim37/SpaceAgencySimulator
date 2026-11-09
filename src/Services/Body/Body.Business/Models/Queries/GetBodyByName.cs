using MediatR;
using Sas.Body.Core.DataTransferObject;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetBodyByName(string Name) : IRequest<BodyDto?>;
