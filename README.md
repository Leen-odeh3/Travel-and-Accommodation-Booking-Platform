<div align="center">
<h2> Travel and Accommodation Booking Platform - Your Ultimate Gateway to Unforgettable Adventures! ⚡🌐✈️</h2>
</div>

## Overview 💥
This project is a comprehensive hotel booking platform designed to provide users with a seamless experience in searching for hotels, viewing detailed information, and making secure bookings. The platform features an advanced search capability, personalized recommendations, and a secure checkout process. For administrators, there is a dedicated interface for managing cities, hotels, and rooms, with functionalities for creating, updating, and deleting entries. User authentication is handled securely through JWT, ensuring robust role-based access control and a maintainable system architecture.

<a href="https://whimsical.com/function-PgTQUTGPFg5SWjqSueYh7x@GjdVyJ6vu7AX2orrZkLYPgQZBb99MoadQxTwLgPFdA24">Functional Requirements And Non-Functional Requirements</a>
## Key Features 🌟
### 1.🔒 User Authentication
- Secure login page with JWT-based authentication.
- Role-based access control for different user permissions.
### 2.🔍 Advanced Search Functionality
- Central search bar for finding hotels and cities.
- Interactive calendar for selecting check-in and check-out dates.
- Adjustable controls for specifying the number of adults, children, and rooms.
### 3.📈 Personalized Recommendations
- Featured deals highlighting special offers on selected hotels.
- Display of recently visited hotels based on user history.
- Trending destinations showcasing the most popular cities.
### 4.🤝 Comprehensive Search Filters
- Filters for price range, star ratings, amenities, and room types.
- Infinite scroll for browsing hotel listings.
### 5.🏠 Detailed Hotel Pages
- High-quality image gallery with fullscreen viewing.
- Detailed information about the hotel, including reviews and an interactive map.
- Room availability and selection with the option to add rooms to the cart for booking.
### 6.✅ Secure Checkout Process
- Collection of user information and payment details.
- Booking confirmation with details, printable options, and email notifications.
### 7.⚙️ Admin Management Interface
- Easy navigation and management of cities, hotels, and rooms.
- Search filters and detailed grids for administrative tasks.
- Forms for creating and updating cities, hotels, and rooms.
### 8.🔧 API Design and Management
- RESTful APIs for all functionalities with clear documentation.
- Robust error handling and logging for debugging and monitoring.
### 9.🛠️ Performance and Scalability
- Efficient data handling for optimized performance.
- Scalable architecture to support growing user and data demands.
### 10.🎯 Comprehensive Testing
- Unit testing, integration testing, and API testing to ensure reliability and performance.
- CI/CD pipeline for automated testing and deployment.
  
## 🌐 API Endpoints

### User Endpoints

| HTTP Method | Endpoint             | Description                                                                                 | Authorization |
|-------------|----------------------|---------------------------------------------------------------------------------------------|---------------|
| POST        | /api/auth/register   | Create a new user account with username, password, and email.                             | All         |
| POST        | /api/auth/login      | Authenticate a user and generate a token if credentials are valid.                         | All          |

### Role Endpoints

| HTTP Method | Endpoint          | Description                                             | Authorization |
|-------------|-------------------|---------------------------------------------------------|---------------|
| POST        | /api/auth/assign-role | Assign a role to a user identified by their email address. | Admin         |

### Token Endpoints

| HTTP Method | Endpoint         | Description                                               | Authorization |
|-------------|------------------|-----------------------------------------------------------|---------------|
| POST        | /api/token/revokeToken | Revokes the specified token and removes it from the user's refresh tokens. | User or Admin  |

#### Detailed Endpoint Description:

- **POST /api/token/revokeToken**
  - **Description**: Revokes the specified token and removes it from the user's refresh tokens.
  - **Request Body**: 
    ```json
    {
      "token": "string"  // The token to be revoked
    }
    ```
  - **Responses**:
    - **200 OK**: If the token was successfully revoked.
    - **400 Bad Request**: If the token is invalid or could not be revoked.
  - **Authorization**: Requires user or admin authentication.
    
### Owner Endpoints

| HTTP Method | Endpoint              | Description                                       | Authorization |
|-------------|-----------------------|---------------------------------------------------|---------------|
| GET         | /api/owner/{id}        | Retrieve an owner by its unique identifier.      | All           |
| POST        | /api/owner             | Create a new owner.                             | Admin         |
| PUT         | /api/owner/{id}        | Update an existing owner.                       | Admin         |
| DELETE      | /api/owner/{id}        | Delete an existing owner.                       | Admin         |
| GET         | /api/owner             | Retrieve all owners.                            | All           |

### City Endpoints

| HTTP Method | Endpoint                                      | Description                                                       | Authorization |
|-------------|-----------------------------------------------|-------------------------------------------------------------------|---------------|
| POST        | /api/city                                     | Add a new city.                                                   | Admin         |
| GET         | /api/city                                     | Retrieve cities with optional filtering by name and description.  | All           |
| GET         | /api/city/{id}                                | Retrieve a city by its unique identifier, with optional hotels.   | All          |
| PUT         | /api/city/{id}                                | Update the information of an existing city.                       | Admin         |
| DELETE      | /api/city/{id}                                | Delete an existing city.                                          | Admin         |
| GET         | /api/city/{cityId}/hotels                     | Retrieve hotels for a specific city.                              | All          |
| POST        | /api/city/{cityId}/hotels                     | Add a hotel to a specific city.                                   | Admin         |
| DELETE      | /api/city/{cityId}/hotel/{hotelId}            | Remove a hotel from a specific city.                              | Admin         |
| POST        | /api/city/{cityId}/upload-image               | Upload an image for a specific city.                              | Admin         |
| DELETE      | /api/city/{cityId}/delete-image/{publicId}    | Delete an image from a specific city.                             | Admin         |
| GET         | /api/city/{cityId}/images                     | Get all images for a specific city.                               | All          |

### Hotel Endpoints

| HTTP Method | Endpoint                                  | Description                                                     | Authorization |
|-------------|-------------------------------------------|-----------------------------------------------------------------|---------------|
| POST        | /api/hotel                                | Add a new hotel.                                                | Admin         |
| GET         | /api/hotel                                | Retrieve hotels with optional filtering by name and description.| All           |
| GET         | /api/hotel/{id}                           | Retrieve a hotel by its unique identifier.                      | All           |
| PUT         | /api/hotel/{id}                           | Update the information of an existing hotel.                    | Admin         |
| DELETE      | /api/hotel/{id}                           | Delete an existing hotel.                                       | Admin         |
| GET         | /api/hotel/{hotelId}/rooms                | Retrieve rooms for a specific hotel.                            | All           |
| POST        | /api/hotel/{hotelId}/rooms                | Add a room to a specific hotel.                                 | Admin         |
| DELETE      | /api/hotel/{hotelId}/room/{roomId}        | Remove a room from a specific hotel.                            | Admin         |
| POST        | /api/hotel/{hotelId}/upload-image         | Upload an image for a specific hotel.                           | Admin         |
| DELETE      | /api/hotel/{hotelId}/delete-image/{publicId}| Delete an image from a specific hotel.                         | Admin         |
| GET         | /api/hotel/{hotelId}/images               | Get all images for a specific hotel.                            | All           |
| POST        | /api/hotel/{hotelId}/amenities            | Add an amenity to a specific hotel.                             | Admin         |
| DELETE      | /api/hotel/{hotelId}/amenities/{amenityId}| Remove an amenity from a specific hotel.                        | Admin         |
| GET         | /api/hotel/{hotelId}/amenities            | Get all amenities for a specific hotel.                         | All           |
| GET         | /api/hotel/{id}/rating                    | Get the review rating of a specific hotel.                      | All           |

### Amenity Endpoints

| HTTP Method | Endpoint                         | Description                                                                                                               | Authorization |
|-------------|----------------------------------|---------------------------------------------------------------------------------------------------------------------------|---------------|
| GET         | /search-results/amenities         | Retrieve all available amenities. Includes details like name, description, and associated room classes.                  | All           |

### Booking Endpoints

| HTTP Method | Endpoint                         | Description                                                                                                               | Authorization |
|-------------|----------------------------------|---------------------------------------------------------------------------------------------------------------------------|---------------|
| POST        | /api/booking/confirm              | Confirm a booking and send a confirmation email. Requires booking confirmation data.                                     | All           |
| GET         | /api/booking/{id}                 | Retrieve a booking by its unique identifier.                                                                             | Admin         |
| POST        | /api/booking/create              | Create a new booking. Requires user email in the token for identification.                                                | All           |
| PUT         | /api/booking/{id}/Update_status  | Update the status of a booking. Requires user email in the token.                                                         | User          |

### Home Page Endpoints

| HTTP Method | Endpoint                 | Description                                                                                                         | Authorization |
|-------------|--------------------------|---------------------------------------------------------------------------------------------------------------------|---------------|
| GET         | /api/homepage/trending-destinations | Get the top 5 most visited cities with details.                                                                  | All           |
| GET         | /api/homepage/search      | Search for hotels based on various criteria such as city name, star rating, number of rooms, and dates.           | All           |

### Invoice Endpoints

| HTTP Method | Endpoint                    | Description                                                             | Authorization |
|-------------|-----------------------------|-------------------------------------------------------------------------|---------------|
| POST        | /api/invoice                | Creates a new invoice record.                                           | Admin         |
| GET         | /api/invoice/{id}           | Retrieves a specific invoice record by ID.                              | All           |
| GET         | /api/invoice/by-booking/{bookingId} | Retrieves all invoice records for a specific booking.                  | All           |
| PUT         | /api/invoice/{id}           | Updates an existing invoice record.                                     | Admin         |
| DELETE      | /api/invoice/{id}           | Deletes an invoice record.                                              | Admin         |

### Review Endpoints

| HTTP Method | Endpoint               | Description                                                          | Authorization |
|-------------|------------------------|----------------------------------------------------------------------|---------------|
| POST        | /api/review            | Creates a new review. Requires 'User' role.                         | User          |
| GET         | /api/review/{id}       | Retrieves a review by its ID.                                        | All           |
| PUT         | /api/review/{id}       | Updates an existing review. Requires 'User' role.                    | User          |
| DELETE      | /api/review/{id}       | Deletes a review by its ID. Requires 'Admin' role.                   | Admin         |

### Room Class Endpoints

| HTTP Method | Endpoint                                    | Description                                                          | Authorization |
|-------------|---------------------------------------------|----------------------------------------------------------------------|---------------|
| POST        | /api/roomclass                              | Creates a new room class. Requires 'Admin' role.                     | Admin         |
| GET         | /api/roomclass/{id}                         | Retrieves a specific room class by ID.                               | All           |
| PUT         | /api/roomclass/{id}                         | Updates an existing room class. Requires 'Admin' role.                | Admin         |
| POST        | /api/roomclass/{roomClassId}/addamenity     | Adds an amenity to a specific room class. Requires 'Admin' role.      | Admin         |
| DELETE      | /api/roomclass/{roomClassId}/amenities/{amenityId} | Deletes an amenity from a specific room class. Requires 'Admin' role. | Admin         |
| GET         | /api/roomclass/{roomClassId}/amenities      | Retrieves all amenities for a specific room class.                    | All           |
| POST        | /api/roomclass/{roomClassId}/rooms          | Adds a new room to a specific room class. Requires 'Admin' role.      | Admin         |
| GET         | /api/roomclass/{roomClassId}/rooms          | Retrieves all rooms for a specific room class.                        | All           |
| DELETE      | /api/roomclass/{roomClassId}/rooms/{roomId} | Deletes a specific room from a room class. Requires 'Admin' role.     | Admin         |
| POST        | /api/roomclass/{roomClassId}/upload-image   | Uploads an image for a specific room class. Requires 'Admin' role.    | Admin         |
| DELETE      | /api/roomclass/{roomClassId}/delete-image/{publicId} | Deletes an image from a specific room class. Requires 'Admin' role.   | Admin         |
| GET         | /api/roomclass/{roomClassId}/image/{publicId} | Retrieves details of an image associated with a specific room class. | All           |

### Room Endpoints

| HTTP Method | Endpoint                                        | Description                                                      | Authorization |
|-------------|-------------------------------------------------|------------------------------------------------------------------|---------------|
| GET         | /api/room/by-price                              | Retrieves rooms within a specific price range.                   | All           |
| GET         | /api/room/{id}                                 | Retrieves a specific room by ID.                                | All          |
| POST        | /api/room/{roomId}/upload-image                 | Uploads an image for a specific room. Requires 'Admin' role.     | Admin         |
| DELETE      | /api/room/{roomId}/delete-image/{publicId}      | Deletes an image from a specific room. Requires 'Admin' role.    | Admin         |
| GET         | /api/room/{roomId}/image/{publicId}             | Retrieves details of an image associated with a specific room.  | All         |
| GET         | /api/room/available-without-bookings            | Retrieves available rooms with no bookings.                      | All          |

### Discount Endpoints

| HTTP Method | Endpoint                     | Description                                                                                                 | Authorization |
|-------------|------------------------------|-------------------------------------------------------------------------------------------------------------|---------------|
| POST        | /api/discount                | Adds a new discount to a room.                                                                             |Admin          |
| PATCH       | /api/discount/{id}           | Updates a discount by ID.                                                                                   | Admin         |
| GET         | /api/discount                | Retrieves a list of all available discounts.                                                               | All          |
| GET         | /api/discount/{id}           | Retrieves the details of a specific discount by ID.                                                         | All         |
| DELETE      | /api/discount/{id}           | Deletes a discount by ID.                                                                                   | Admin          |
| GET         | /api/discount/active         | Retrieves a list of currently active discounts.                                                            | All          |
| GET         | /api/discount/top-discounts   | Retrieves a list of rooms with the highest active discounts.                                                | All         |

### Image Endpoints

| HTTP Method | Endpoint                                 | Description                                                                                         | Authorization |
|-------------|------------------------------------------|-----------------------------------------------------------------------------------------------------|---------------|
| POST        | /api/image/{entityType}/{entityId}/upload-image | Uploads an image for a specific entity (e.g., room, hotel) using the entity type and ID.           | Admin         |
| GET         | /api/image/images/{type}                  | Retrieves all images associated with a specific entity type.                                        | User or Admin  |
| DELETE      | /api/image/delete-image/{publicId}        | Deletes an image by its PublicId.                                                                    | Admin         |
| GET         | /api/image/details/{publicId}             | Retrieves details of an image by its PublicId.                                                       | User or Admin  |

### Hotel Amenities Endpoints

| HTTP Method | Endpoint                      | Description                                        | Authorization     |
|-------------|-------------------------------|----------------------------------------------------|-------------------|
| GET         | /api/hotelAmenities            | Retrieve amenities by hotel name with optional pagination. | Admin, User       |
| GET         | /api/hotelAmenities/all        | Retrieve all amenities by hotel name.             | Admin, User       |
| PUT         | /api/hotelAmenities/{amenityId} | Update a specific amenity by its ID.              | Admin             |

## Layered architecture
`Clean Architecture`
It achieves this by separating the application into different layers that have distinct responsibilities:

<img width="253" alt="clean-architecture-layers" src="https://github.com/user-attachments/assets/d22c667b-6f18-4728-a6f0-b6c514e16e06">

### 1. Domain Layer
Contains the core business logic and domain entities. This layer is independent of other layers and focuses on the business rules and domain models.
#### Components:
- Entities: Represent the core data models. For example, User, Role, Hotel, Room, Booking.
- Value Objects: Define immutable objects used within the domain. For example, Address, Money.
- DTOs (Data Transfer Objects): Define the data structures used for communication between the application layer and the API layer. For example, UserDto, BookingDto.
- Domain Repository: Implement business logic that does not naturally fit within an entity. For example, DiscountRepository.
### 2. Infrastructure Layer
Provides implementations for data access, external services, and other system-level concerns. This layer interacts with databases, file systems, and external APIs.
#### Components:
- Repositories: Implement data access logic and interact with the database. For example, UserRepository, HotelRepository.
- Data Context: Manages database connections and operations. For example, AppDbContext.
- Configurations: Manage configurations for external services and data access.
### 3. Application Layer
Orchestrates the application's workflow, coordinates interactions between domain models and infrastructure, and handles application-specific logic.
#### Components:
- Services: Implement application-specific logic and use domain services and repositories to perform operations. For example, UserService, BookingService.
- Application Interfaces: Define contracts for services and repositories. For example, IUserService, IBookingService.
 - External Services: Handle interactions with third-party services. For example, image storage services, payment gateways.
### 4. API Layer
Exposes endpoints for client interaction and handles HTTP requests and responses. This layer is responsible for routing, validation, and transforming data between the application layer and clients.
#### Components:
- Controllers: Handle incoming HTTP requests, invoke application services, and return responses. For example, UserController, BookingController.
- Middlewares: Implement cross-cutting concerns such as authentication, authorization, and logging.
- Response Handlers: Standardize API responses and error handling. For example, ResponseHandler.

## Image Storage
### Cloudinary Integration
Cloudinary is used for managing and storing images in the Hotel Booking Platform. It provides a scalable, secure, and feature-rich solution for image storage and transformation.

<img width="503" alt="cloud" src="https://github.com/user-attachments/assets/0c91f3e2-1023-442a-b250-a458db4b0d32">

#### We add account informations on appsettings.json and create an entity whose name is CloudinarySettings.
<img width="246" alt="setting" src="https://github.com/user-attachments/assets/bb570e84-265c-49ea-8115-721a9784087d">

### 1. Image Upload
- Endpoint: `POST /api/image/upload`

### 2. Image Retrieval
- Endpoint:` GET /api/image/details/{publicId}`

### 3. Image Deletion
- Endpoint: `DELETE /api/image/delete-image/{publicId}`
## 📩  Booking Confirmation 
Once a booking is confirmed, users receive an email containing the details of their reservation. Below is an example of the booking confirmation email that users receive:

![0](https://github.com/user-attachments/assets/14aff3a6-15e8-4572-a782-840c251c0e90)

## 🔋🪫 Testing Frameworks 
Ensuring code quality and functionality with comprehensive testing suites using:

- ##### xUnit: A popular .NET testing framework that facilitates writing and executing unit tests with a focus on simplicity and extensibility. ✅
- ##### Moq: A powerful mocking library for .NET that allows creating mock objects for unit testing, enabling isolated and controlled test scenarios. 🧪
- ##### FluentAssertions: A library that provides a more expressive and readable syntax for assertions, making tests easier to write and understand. 📏
- ##### Fixture: A testing concept used to set up shared contexts for multiple test cases, allowing for efficient and consistent test execution. 🧩

## Design Patterns
In the Hotel Booking Platform, several design patterns are employed to ensure a clean, maintainable, and scalable architecture. Here’s a summary of the key patterns used:
### 1. Unit of Work Pattern
The Unit of Work pattern is used to manage changes to multiple entities in a single transaction. This pattern ensures that changes to the database are handled in a single unit, maintaining data consistency and integrity.

<img width="457" alt="Unitofwork" src="https://github.com/user-attachments/assets/a4007b80-a962-4fca-952e-d2bee8655f3c">

### 2. Generic Repository Pattern
The Generic Repository pattern provides a way to manage CRUD operations in a consistent and reusable manner. This pattern abstracts the data access layer and provides a generic implementation for common operations.

![layers](https://github.com/user-attachments/assets/2eded6b6-715e-415b-b7a3-63a4d17db03c)

## 🕰️ Project Management 
#### Use of Trello for Task Management
<img width="956" alt="Board" src="https://github.com/user-attachments/assets/38944aca-1b8e-4b6e-bf9b-4d0bf774e459">

## Setup Guide

#### 1. **System Requirements**

- **Operating System**: Windows 10 or higher, macOS, or Linux
- **Software**: .NET Core SDK 6.0 or later, Node.js (if applicable), Docker (if using containers)
- **Hardware**: Minimum 4 GB RAM, 2 CPU cores

#### 2. **Clone the Repository**

```bash
git clone https://github.com/Leen-odeh3/Travel-and-Accommodation-Booking-Platform.git
cd Travel-and-Accommodation-Booking-Platform
```
#### 3. **Configure appsettings.json**
 Open the `appsettings.json` file located in your project directory and configure the connection string for SQL Server. 
 Replace the `<connection_string>` placeholder with your SQL Server connection string:
```{
  "ConnectionStrings": {
    "SqlServer": "<connection_string>"
  }
}
```
## 🛟 Contact and Support
If you have any questions or comments about Project, please contact me via <a href="leenodeh287@gmail.com">Email</a> .

## 🏅 Acknowledgements
I extend my sincere gratitude to <a href="https://www.foothillsolutions.com/">Foothill Technology Solutions </a> for granting me the opportunity to participate in this internship cycle. Their unwavering support has been instrumental throughout the development of this project.

<div align="center">

![download](https://github.com/user-attachments/assets/e0ff60a8-63e0-44c6-8890-1391c1a39e75)

#### Thank you for your interest. I look forward to hearing from you! 🥳

</div>

