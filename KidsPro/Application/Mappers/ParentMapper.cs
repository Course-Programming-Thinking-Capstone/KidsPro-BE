using Application.Dtos.Response.Account.Parent;
using Domain.Entities;

namespace Application.Mappers
{
    public static class ParentMapper
    {
        public static ParentOrderResponse ParentShowContact(Parent entity) => new ParentOrderResponse()
        {
            Email = entity.Account.Email,
            PhoneNumber = entity.PhoneNumber
        };
        
    }
}
