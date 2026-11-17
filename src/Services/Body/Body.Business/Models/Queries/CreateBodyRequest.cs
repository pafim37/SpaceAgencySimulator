using MediatR;
using Sas.Body.Core.DataTransferObject;

namespace Sas.Body.Business.Models.Queries;

public sealed record CreateBodyRequest(NewBodyDto Body, string? SenderId) : IRequest<BodyDto>;
