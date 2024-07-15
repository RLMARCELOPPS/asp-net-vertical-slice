using ecommerse_api.Common.Responses;
using MediatR;

namespace ecommerse_api.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<IApiResponse>
    {
        public Guid Id { get; set; }
    }
}
