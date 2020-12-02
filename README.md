# Store Application

This application is designed with functionality that would make virtual shopping much simpler! Customers can sign up for an account, place orders, view their order history, and specific location inventory. It also comes with an additional interface for managing your business. Managers can view and replenish location inventory, add new products, and view the order history of specific locations. This application used Entity Framework Core to connect to a PostgreSQL database, ASP.NET Core API to create a RESTful API, and HTML, CSS, BootstrapJS, and Javascript to create the front end. 

## Functionality

### API Functionality
- Many API actions utilize asynchronous code and the Task Parallel Library to allow (initially!) concurrent requests.
- Swagger documentation and testing functionality implemented.
- API is partially organized based on like functionality, rather than the objects that the CRUD operations apply to.
- API has capacity to send a user object back regardless of whether they are a customer or manager.
- Uses AutoMapper to map data to DTOs and back to simplify code.
    - Bulk mapping is done in parallel to speed up results.

### Front-end Functionality
- Will deeply acquaint you with the color blue.
- Front-end uses Bootstrap cards to display selectable products to draw attention to options in an appealing way.
- Uses tables to display historical data like order history and associated products to organize data effectively.
- Extensive use of partial views for easy modifiability.
- Loosely coupled with API, uses simple models that mirror the DTOs of the API, while still being able to access more complex data with computed properties.
- Reuses single HttpClient for increased performance.


### Other Program Notes
- Comprehensive input validation unit tests
- Program Business Logic and Library are separated so the validation and custom exception logic can be slotted into other applications.
- There is currently no way to add a new Location, by design. Such a massive event didn’t seem like something that one would want to include in an end-user application, even with manager-functions. But the model and constructors are configured so that functionality could be added swiftly if needed.

### User-Agnostic functionality
- The shop sells various forms of ice as products.
    - Each Product has a name, a price, an enumerated type/category, and a description in the database.
- Each user can sign up with their name, email, and password, and then login with their email and password.
    - All user input of any form is validated with a custom validation class library that can operate with any Regex input condition or list of conditions passed to the validator, as long as the condition class implements the IInputCondition interface.
- All order histories of any kind can be sorted by date and price, in ascending or descending order.
- The menu system is modularized with a pseudo-factory class that handles menu progression, allowing the disposal of previous menus.

### Customer Functionality
- The Customer is greeted by name and offered the choice to view their own order history, select a location to order from, or exit the program.
    - Subsequent menus allow the customer to return to this start menu unless they’re in a contextual submenu or finalizing an order.
- Customer order histories include:
    - The location of the order
    - Subtotal of the order
    - A list of products bought in each order, complete with quantity of each product.
- Customers can view the stock of each product at each location, type the quantity of the product they want, and build up their cart before submitting their order.

### Manager Functionality
- The Manager is greeted with a reminder of their assigned location, and offered the choice to view their location’s order history, manage order history, or exit the program.
    - Subsequent menus allow the manager to return to this start menu unless they’re in a contextual submenu.
- Location order histories include any order placed at that location, with the same sorting capability and formatting as Customer histories.
- Managers have a lot of flexibility in managing stock. They are able to:
    - Increase or decrease the stock of existing items in stock
    - Add an existing product that isn’t in stock to their location
    - Add a brand new product that hasn’t existed before to the SufferShop Catalogue.


## Prerequisites

- .NET Framework Core 3.1 and above

## License

This project is licensed under the MIT License.
