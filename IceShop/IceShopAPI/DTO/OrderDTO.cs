using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceShopAPI.DTO
{
    public class OrderDTO
    {

        public int Id { get; set; }

        public int CustomerId { get; set; }


        public string Address { get; set; }




        public int LocationId { get; set; }


        public double Subtotal { get; set; }


        public double TimeOrderWasPlaced { get; set; }

        public double TimeOrderWasFulfilled { get; set; }//TODO: What the hell do I do about fulfillment?


    }
}
