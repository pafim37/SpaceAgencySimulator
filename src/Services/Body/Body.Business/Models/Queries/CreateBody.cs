using MediatR;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Business.Models.Queries;

public sealed record CreateBody(NewBodyDto Body, string? SenderId) : IRequest<BodyEntity>;
