# 🛒 E-Commerce Core Web API

## 🎯 About the Project
This project is an e-commerce Web API that I coded completely from scratch and independently, applying the layered architecture disciplines I acquired during my backend training process to my own business logic.

Moving beyond standard training project templates, scenarios such as dynamic cart management, stock tracking, virtual payment workflows, and database transactional integrity were developed in accordance with RESTful principles.

## 🏗️ Architectural Layers (Clean Architecture)
The project consists of four main layers, ensuring minimized coupling and maintained code sustainability:
* **Entities:** Contains database tables (Product, Category, CartItem, Order), DTO records, and Custom Exception classes.
* **Repositories:** The data access layer where `DbContext` configurations are made and communication with the database (Entity Framework Core) is established.
* **Services:** The core layer where all Business Logic is executed, cart calculations are performed, and AutoMapper transformations take place.
* **Presentation:** Houses the Controllers that receive HTTP requests and route them to the Service layer.

## 🚀 Project Features

### 1. E-Commerce Domain Logic
* **Token-Based Cart Management:** The system was configured to automatically recognize the requesting user via the JWT token in the Header. Users are able to add products to their carts, update quantities, and the system dynamically calculates the *Grand Total* for the active cart.
* **Order & Transactional Integrity:** Virtual card validation was implemented during the user checkout process. Upon payment approval, purchased quantities are immediately deducted from the `StockQuantity` in the database, an order record is created, and the user's cart is cleared.

### 2. RESTful Standards
* **Data Shaping:** Instead of downloading entire objects, data is dynamically shaped and returned using `ExpandoObject` based on fields specified via the URL (e.g., `?fields=id,name`).
* **Pagination:** Pagination was applied to data listings; metadata such as Total Pages and Current Page was directly injected into the HTTP Response Headers (`X-Pagination` Header).
* **Filtering and Searching:** In price range and name filtering, when no matching record is found, a `200 OK` status with an empty array (`[]`) is returned in compliance with standard practices.
* **HTTP Method Diversity:** `HEAD` requests for reading headers without a payload and `OPTIONS` requests indicating the methods supported by the API were integrated into the system.

### 3. System Architecture & Security
* **Global Exception Handling:** Instead of `try-catch` blocks throughout the project, custom exception classes (e.g., `InvalidPriceRangeBadRequestException`) were created; all errors are caught in the Middleware layer and returned in standard JSON formats like 400, 401, and 404.
* **Rate Limiting:** A limit was imposed on the number of requests from specific IPs (HTTP 429) to protect the system against overloading.
* **Eager Loading:** Relationships between categories and their products were fetched in a single query using the Entity Framework `Include` function.
* **Static File Management (File I/O):** Image/file uploading to the server and downloading via the API were implemented.

## 🎥 Postman System Test & Workflow
You can watch the test video demonstrating how the API's security layers, cart management, stock deduction, and exception handling work end-to-end below:

👉 **[Watch the System Test and Postman Workflow Video Here](https://lnkd.in/p/d_TfD5wB)**

## 🛠️ Technologies Used
* .NET Core (C#) & Entity Framework Core & SQL Server
* Clean Architecture (Entities, Repository, Services, Presentation)
* JWT (JSON Web Token), Refresh Token, Rate Limiting
* AutoMapper, Custom Exceptions, Eager Loading

## 📈 My Developer Journey
*Note for technical teams: This project is a reflection of my ability to transform the architectural disciplines I learned during my backend development process into a project built entirely from scratch with my own custom business logic.*
