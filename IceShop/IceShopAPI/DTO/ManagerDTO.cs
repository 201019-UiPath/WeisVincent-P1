using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopAPI.DTO
{
    /// <summary>
    /// This class represents the manager of a given location, complete with a name, login credentials, and a location Id and navigation property.
    /// </summary>
    public class ManagerDTO : UserDTO
    {

        public ManagerDTO() { }

        internal ManagerDTO(string name, string email, string password, int locationId) : base(name, email, password)
        {
            LocationId = locationId;
        }

        /// <summary>
        /// This constructor should only be used for signup before a manager has selected a location to manage. 
        /// Managers should always have a Location to manage.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public ManagerDTO(string name, string email, string password) : base(name, email, password)
        {

        }

        public ManagerDTO(string name, string email, string password, LocationDTO managedLocation) : base(name, email, password)
        {
            //Location = managedLocation;
            LocationId = managedLocation.Id;
        }

        public int LocationId { get; set; }
        //public LocationDTO Location { get; set; }

    }
}
