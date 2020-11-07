using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{

    /// <summary>
    /// This model represents a join-table entry between a location and products at that location. 
    /// Each instance of this model represents the quantity of a single product at a single location.
    /// </summary>
    public class InventoryLineItem
    {
        /*[Key]
        public int Id { get; set; }*/

        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column("ProductQuantity")]
        public int ProductQuantity { get; set; }

        internal InventoryLineItem(int locationId, int productId, int productQuantity)
        {
            LocationId = locationId;
            ProductId = productId;
            ProductQuantity = productQuantity;

        }

        public InventoryLineItem(Location location, Product product, int productQuantity) : this(location.Id, product.Id, productQuantity)
        {
            Product = product;
            Location = location;
        }


    }
}
