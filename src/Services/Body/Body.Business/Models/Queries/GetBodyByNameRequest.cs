using MediatR;
using Sas.Body.Core.DataTransferObject;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetBodyByNameRequest(string Name) : IRequest<BodyDto?>;
