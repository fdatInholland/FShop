namespace FShop.Infrastructure.EventBus.Product
{
    public class ProductCreated
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
