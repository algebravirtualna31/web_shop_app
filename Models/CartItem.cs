namespace web_shop_app.Models
{
    public class CartItem
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal GetTotal()
        {
            return Product.Price * Quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            if(quantity < 1)
            {
                return;
            }

            Quantity = Quantity + quantity;
        }
    }
}
