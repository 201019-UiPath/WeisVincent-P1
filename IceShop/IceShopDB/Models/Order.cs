using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    [Table("Orders")]
    public class Order : IStorableInRepo
    {

        // TODO: Review constructor for Order class, you left good stuff in it

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



        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        /// <summary>
        /// This field represents the customer who placed the order.
        /// </summary>
        public Customer Customer { get; set; }

        [Column("Address")]
        public string Address { get; set; }




        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public List<OrderLineItem> OrderLineItems { get; set; }

        public void AddLineItemToOrder(OrderLineItem addedLineItem)
        {
            OrderLineItems.Add(addedLineItem);
        }

        [Column("Subtotal")]
        public double Subtotal { get; set; }


        /// <summary>
        /// This field represents the time the order was placed in Linux Epoch format. Should be in UTC, and converted when displayed in UI.
        /// Methods responsible for this should be in the DateTimeUtility class of the IceShopLib assembly.
        /// </summary>
        [Column("placedtime_posix")]
        public double TimeOrderWasPlaced { get; set; }

        [Column("finishedtime_posix")]
        public double TimeOrderWasFulfilled { get; set; }//TODO: What the hell do I do about fulfillment?

        [NotMapped]
        public bool IsComplete
        {
            get
            {
                if (TimeOrderWasFulfilled > TimeOrderWasPlaced) return true; else return false;


            }
        }


    }
}
