namespace web_shop_app.ViewModels
{
    public class UserOrderViewModel
    {
        public OrderAddress OrderAddress { get; set; }

        public string Message { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
