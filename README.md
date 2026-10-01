# 🛒 PC Hardware E-Commerce RESTful Web API

![Status](https://img.shields.io/badge/Status-LIVE-brightgreen?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Supabase-4169E1?style=for-the-badge&logo=postgresql)
![Render](https://img.shields.io/badge/Render-Deployment-46E3B7?style=for-the-badge&logo=render)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger)

> 🌐 **Live Production Swagger UI:** [Click Here to Access Live API](BURAYA_RENDER_SWAGGER_LINKINIZ)
>
> ⚠️ *Note: Hosted on Render's free cloud instance. The initial request may take ~30 seconds due to server cold-start.*

---

## 🔑 Live Interactive Demo Accounts
The Swagger UI includes a custom JavaScript injection that **automatically attaches the Bearer JWT Token** upon successful login, making live endpoint testing seamless.

| Role | Username | Password | Scope & Expected Behavior |
| :--- | :--- | :--- | :--- |
| **Admin** | `demoadmin` | `Password123*` | Full management access (`POST`, `PUT`, `DELETE`, `PATCH` -> `201 Created` / `200 OK`) |
| **User** | `demouser` | `Password123*` | Cart & Order simulation (`POST /api/product` -> `403 Forbidden` Access Denied test) |

---

## 🎯 About the Project
This project is a production-ready, feature-rich E-Commerce RESTful Web API built from scratch, focusing on PC Hardware components (CPUs, GPUs, RAMs, Motherboards, Monitors, Peripherals). It applies strict layered architecture principles and real-world business logic.

Moving beyond standard training project templates, scenarios such as role-based access control (RBAC), dynamic cart management, transactional stock deduction, custom header pagination, and database seeding were engineered in accordance with production standards.

## 🚀 Live Deployment & Cloud Architecture (DevOps)
* **Cloud Hosting & CI/CD:** Deployed on **Render** with automated deployment pipelines from GitHub.
* **Database Infrastructure:** Powered by **Supabase (PostgreSQL)** for reliable cloud relational data storage.
* **Automated Migrations & Data Seeding:** The custom `ConfigureAndMigrateDatabase` startup pipeline automatically executes pending EF Core migrations and seeds default Identity roles, demo user accounts, and real-world computer hardware inventory upon application startup.

---

## 🏗️ Architectural Layers (Clean Architecture)
The project consists of four main layers to ensure loose coupling, testability, and maintainability:
* **Entities:** Contains database domain models (Product, Category, CartItem, Order, ApplicationUser), DTO records, and Custom Exception classes.
* **Repositories:** Data access layer configured with Entity Framework Core and `DbContext` mappings for PostgreSQL.
* **Services:** Core Business Logic layer handling cart computations, stock reservation, security claims, and AutoMapper transformations.
* **Presentation:** REST Controllers receiving HTTP requests, applying Action Filters, and routing requests to the Service layer.

---

## 🌟 Core Features & Technical Achievements

### 1. Role-Based Security & Authorization (RBAC)
* **ASP.NET Core Identity & JWT:** Integrated dual-token mechanism (Access & Refresh Tokens) with UTC expiration standards.
* **Role Isolation:** Administrative endpoints are protected via `[Authorize(Roles = "Admin")]`. Attempting administrative actions with a standard `User` role returns a standard `403 Forbidden` response.
* **Automated Swagger Authorization:** Custom JavaScript injected into Swagger UI automatically attaches `Authorization: Bearer <token>` upon executing `/api/authentication/login`.

### 2. E-Commerce Domain Logic & Data Integrity
* **Token-Based Cart Management:** Automatically identifies active users via JWT Claims in the HTTP Header to manage carts and compute grand totals dynamically.
* **Transactional Stock Management:** Checkout process utilizes database transactions. Upon order approval, stock availability is verified, purchased quantities are deducted from `StockQuantity`, the order record is created, and the active cart is cleared atomically.

### 3. RESTful Standards & Query Features
* **Data Shaping:** Uses `ExpandoObject` to return only specific fields requested via URL queries (e.g., `?fields=id,name,price`).
* **Pagination & Custom Headers:** Page metadata (Total Count, Page Size, Current Page) is injected directly into `X-Pagination` HTTP Response Headers.
* **Filtering, Searching & Sorting:** Price range filtering, category filtering, and product searches return `200 OK` with an empty array `[]` when no matching records exist.
* **HTTP Method Diversity:** Implements `PATCH` (`JsonPatchDocument`) for partial updates, alongside `HEAD` and `OPTIONS` for API discovery.

### 4. Cross-Cutting Concerns & System Quality
* **Global Exception Middleware:** Eliminates boilerplate `try-catch` blocks by handling custom exceptions (e.g., `ProductNotFoundException`) at the middleware layer and returning standardized JSON error payloads (400, 401, 403, 404).
* **Rate Limiting:** Enforces IP-based request throttles (HTTP 429) to protect endpoints against brute-force attacks.
* **Static File Management (File I/O):** Server-side image uploading and file delivery via API routes.

---

## 🎥 System Test & Postman Workflow Video
Watch the end-to-end system test video demonstrating security layers, cart workflows, stock deduction, and exception handling:

👉 **[Watch the System Test & Postman Workflow Video](https://lnkd.in/p/d_TfD5wB)**

---

## 🛠️ Tech Stack & Tools
* **Framework & Language:** .NET 8 / C#
* **Architecture:** Layered / Clean Architecture
* **ORM & Database:** Entity Framework Core, PostgreSQL (Supabase)
* **Security & Auth:** ASP.NET Core Identity, JWT Bearer Tokens, Refresh Tokens, RBAC
* **Cloud & Hosting:** Render, Supabase, Automated DB Seeding
* **Libraries & Tools:** AutoMapper, NewtonsoftJson (PATCH), Swagger UI, Action Filters, Rate Limiting

---

## 📈 Developer Journey
*This project represents my ability to design, build, and deploy production-grade backend Web API systems independently—from initial schema design and business rules to live cloud deployment with automated DB seeding and interactive documentation.*
