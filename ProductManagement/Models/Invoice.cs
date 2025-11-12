namespace ProductManagement.Models
{
    public class Invoice
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }
    }
}
