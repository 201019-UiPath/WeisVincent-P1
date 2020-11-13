using Newtonsoft.Json;
using System.Collections.Generic;

namespace IceShopWeb.Models
{
    public class Order
    {

        public Order()
        {
        }
        
        /// <summary>
        /// This constructor should be used for when JSON data is received.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerId"></param>
        /// <param name="locationId"></param>
        /// <param name="address"></param>
        /// <param name="Subtotal"></param>
        /// <param name="timeOrderWasPlaced"></param>
        [JsonConstructor]
        internal Order(int id, int customerId, int locationId, string address, double Subtotal, double timeOrderWasPlaced, double timeOrderWasFulfilled)
        {
            Id = id;
            CustomerId = customerId;
            Address = address;
            LocationId = locationId;
            this.Subtotal = Subtotal;
            TimeOrderWasPlaced = timeOrderWasPlaced;
            TimeOrderWasFulfilled = timeOrderWasFulfilled;
        }

        internal Order(int customerId, int locationId, string address, double Subtotal, double timeOrderWasPlaced)
        {
            //TODO: How to deal with Order Id
            CustomerId = customerId;
            Address = address;
            LocationId = locationId;
            this.Subtotal = Subtotal;
            TimeOrderWasPlaced = timeOrderWasPlaced;
        }

        public Order(Customer customer, Location locationPlaced, double subtotal, double timeOrderWasPlaced)
            : this(customer.Id, locationPlaced.Id, locationPlaced.Address, subtotal, timeOrderWasPlaced)
        {
            Customer = customer;

            Location = locationPlaced;

            OrderLineItems = new List<OrderLineItem>();

            TimeOrderWasPlaced = timeOrderWasPlaced;
            TimeOrderWasFulfilled = -1;
        }

        public Order(Customer customer, Location locationPlaced, List<OrderLineItem> orderLineItems, double subtotal, double timeOrderWasPlaced)
            : this(customer, locationPlaced, subtotal, timeOrderWasPlaced)
        {
            OrderLineItems = orderLineItems;
        }



        
        public int Id { get; set; }

        
        public int CustomerId { get; set; }

        /// <summary>
        /// This field represents the customer who placed the order.
        /// </summary>
        public Customer Customer { get; set; }

        
        public string Address { get; set; }




        
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public List<OrderLineItem> OrderLineItems { get; set; }

        
        public double Subtotal { get; set; }


        /// <summary>
        /// This field represents the time the order was placed in Linux Epoch format. Should be in UTC, and converted when displayed in UI.
        /// Methods responsible for this should be in the DateTimeUtility class of the IceShopLib assembly.
        /// </summary>
        
        public double TimeOrderWasPlaced { get; set; }

        public string TimeOrderWasPlacedAsString
        {
            get
            {
                if (TimeOrderWasPlaced > 0)
                {
                    return DateTimeUtility.GetDateTimeFromUnixEpochAsDouble(TimeOrderWasPlaced).ToShortDateString();
                } else return "";
            }
        }
        
        public double TimeOrderWasFulfilled { get; set; }//TODO: What the hell do I do about fulfillment?
        public string TimeOrderWasFulfilledAsString
        {
            get
            {
                if (TimeOrderWasFulfilled > 0)
                {
                    return DateTimeUtility.GetDateTimeFromUnixEpochAsDouble(TimeOrderWasPlaced).ToShortDateString();
                }
                else return "";
            }
        }

        public bool IsComplete
        {
            get
            {
                if (TimeOrderWasFulfilled > TimeOrderWasPlaced) return true; else return false;


            }
        }


    }
}
