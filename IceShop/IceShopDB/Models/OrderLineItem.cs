using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    /// <summary>
    /// This model represents a join-table entry between an order and products in that order. 
    /// Each instance of this model represents the quantity of a single product in a single order.
    /// </summary>
    public class OrderLineItem
    {
        //[Key]
        //public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column("product_quantity")]
        public int ProductQuantity { get; set; }

        internal OrderLineItem(int orderId, int productId, int productQuantity)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductQuantity = productQuantity;
        }

        public OrderLineItem(Order order, Product product, int quantityOfProduct) : this(order.Id, product.Id, quantityOfProduct)
        {
            Product = product;

            Order = order;
        }
    }
}
