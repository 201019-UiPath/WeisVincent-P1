using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class ILIDTO
    {

        
        public int LocationId { get; set; }
        //public LocationDTO Location { get; set; }

        
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }

        public int ProductQuantity { get; set; }

        internal ILIDTO(int locationId, int productId, int productQuantity)
        {
            LocationId = locationId;
            ProductId = productId;
            ProductQuantity = productQuantity;

        }

        public ILIDTO(LocationDTO location, ProductDTO product, int productQuantity) : this(location.Id, product.Id, productQuantity)
        {
            //Product = product;
            //Location = location;
        }


    }
}
