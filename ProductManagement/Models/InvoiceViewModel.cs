using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductManagement.Models
{
    public class InvoiceViewModel
    {
        public Guid CustomerID { get; set; }
        public Guid ProductID { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }

        public List<SelectListItem> customerResponse;
        public List<SelectListItem> assignedProductResponse;
    }
}
