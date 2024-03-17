using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IParentRepository:IBaseRepository<Parent>
{
    Task<Parent?> LoginByPhoneNumberAsync(string phoneNumber);

    Task<bool> ExistByPhoneNumberAsync(string phoneNumber);

    Parent? GetEmailZalo(int parentId);
}