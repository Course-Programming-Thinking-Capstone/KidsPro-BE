using Application.Dtos.Request.Account.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface ICertificateService
    {
        Task AddCertificateAsync(CertificatesRequest dto);
    }
}
