using IceShopDB.Models;

namespace IceShopBL
{
    /// <summary>
    /// This class represents a locally staged item in a customer's order cart.
    /// It includes information on what the user is ordering and what store inventory will be affected.
    /// </summary>
    public class StagedLineItem
    {
        public InventoryLineItem affectedInventoryLineItem;

        public Product Product;
        public int Quantity;
        public StagedLineItem(Product product, int quantity, InventoryLineItem affectedInventoryLineItem)
        {
            Product = product;
            Quantity = quantity;
            this.affectedInventoryLineItem = affectedInventoryLineItem;
        }

        public int GetNewQuantityOfAffectedInventoryLineItem()
        {
            return affectedInventoryLineItem.ProductQuantity - Quantity;
        }

    }
}
