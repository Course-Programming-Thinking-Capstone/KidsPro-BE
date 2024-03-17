using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.Order
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
