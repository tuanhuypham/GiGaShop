namespace Gigashop.Models
{
    public class CheckoutViewModel
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
