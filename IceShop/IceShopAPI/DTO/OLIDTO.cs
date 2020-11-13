using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class OLIDTO
    {

        //[Key]
        //public int Id { get; set; }

        
        public int OrderId { get; set; }
        public OrderDTO Order { get; set; }

        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }

        public int ProductQuantity { get; set; }

        internal OLIDTO(int orderId, int productId, int productQuantity)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductQuantity = productQuantity;
        }

    }


}

