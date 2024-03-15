using Application.Dtos.Request.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Application.Services;

public class CertificateService: ICertificateService
{
    public IUnitOfWork _unitOfWork;

    public CertificateService(IUnitOfWork unit)
    {
        _unitOfWork = unit;
    }

    public async Task AddCertificateAsync(CertificatesDto dto)
    {
        var _value = new Certificate()
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            ResourceUrl = dto.ResourceUrl,
            Description = dto.Description,
            CompletionDate = DateTime.UtcNow
        };

        await _unitOfWork.CertificateRepository.AddAsync(_value);
        var result = await _unitOfWork.SaveChangeAsync();

        if (result < 0) throw new BadRequestException("Add certificate failed");
    }
  
}
