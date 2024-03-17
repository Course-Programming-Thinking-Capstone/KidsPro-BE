using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Order
{
    public class OrderRequestDto
    {
        public List<int> StudentId { get; set; } = new List<int>();
        public int ParentId { get; set; }
        public int CourseId { get; set; }
        public int VoucherId { get; set; }
        public int PaymentType { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
