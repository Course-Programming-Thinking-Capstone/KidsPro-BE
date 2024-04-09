namespace Application.Dtos.Request.Order
{
    public class OrderRequest
    {
        public List<int> StudentId { get; set; } = new List<int>();
        public int CourseId { get; set; }
        public int ClassId { get; set; }
        public int VoucherId { get; set; }
        public int PaymentType { get; set; }
        public int Quantity { get; set; }
    }
}
