# 🛠 Job Application API

A clean and modular ASP.NET Core Web API project for managing jobs and applications. It allows creating, updating, retrieving, and deleting jobs and job applications with proper validations, logging, and global error handling.

---

## 🚀 Technologies Used

- **ASP.NET Core Web API** — Backend web framework
- **Entity Framework Core** — ORM for database access
- **PostgreSQL** — Used as the primary database
- **Serilog / ILogger** – for structured logging
- **Global Exception Handling Middleware** – for consistent error responses

---

## 🧩 Design Patterns & Best Practices

| Concept / Pattern               | Description |
|--------------------------------|-------------|
| **Repository Pattern**          | Abstracts data access logic via interfaces like `IJobRepository`. |
| **Service Layer**               | Encapsulates business logic in services like `JobService` and `ApplicationService`. |
| **DTO (Data Transfer Objects)** | Clean separation between domain and transport models. |
| **Dependency Injection (DI)**   | Built-in DI is used throughout the app. |
| **Async/Await Pattern**         | Enhances performance with non-blocking I/O operations. |
| **Validation with Data Annotations** | Ensures robust input using `[Required]`, `[StringLength]`, `[Range]`, etc. |
| **Global Error Handling**       | Centralized exception middleware to capture and return consistent error messages. |
| **Logging (Serilog / ILogger)** | Structured and contextual logs help in monitoring and debugging. |
| **Separation of Concerns**      | Clean layers: Controllers → Services → Repositories. |

---

## ✅ Features

- Create, Update, Delete, and Get Jobs
- Create, Update, Delete, and Get Applications (including resume upload)
- Global error handling and model validation
- Logging using Serilog
- Database access using Entity Framework Core
- Clean and testable architecture

---

## 📝 Setup Instructions

1. Clone the repository
2. Create the database schema: Run the PostgreSQL script located at Data/DBScript.
3. Update connection string in `appsettings.json`
4. Start the API and test endpoints via Swagger/Postman

---

## 📌 Notes

- Logs and uploaded files (`wwwroot`) are excluded from version control.
- Validations are enforced on all endpoints via DTO annotations.
- Designed for extensibility, maintainability, and clean separation of concerns.