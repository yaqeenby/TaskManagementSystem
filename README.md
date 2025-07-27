# 📌 TaskManagementSystem

A backend system for managing tasks, users, and roles. Designed with clean architecture principles, built using ASP.NET Core.

## 🚀 Features

- User authentication with JWT
- Role-based access control (RBAC)
- Task creation, assignment, and status tracking
- Middleware for centralized exception handling and role checks
- Entity Framework Core with code-first approach
- Secure API endpoints with `[Authorize]` attributes

## 🛠️ Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- InMemory database
- JWT Authentication
- C#
- Swagger (OpenAPI)

## 🧰 Getting Started

### Prerequisites

- [.NET SDK 8+]

### Running the Project

1. Clone the repository:

   ```bash
   git clone https://github.com/yaqeenby/TaskManagementSystem.git
   cd TaskManagementSystem

   ```

2. Run the project:
   dotnet run

   # Login

   1. Admin: admin@example.com - password: Admin123!
   1. User: user@example.com - password: User123!

3. Open Swagger UI in browser:
   http://localhost:5001/swagger/index.html

### 🔐 Authentication

- This project uses JWT Bearer tokens.

- After login, you receive a token.

- Include this token in the Authorization header as Bearer YOUR_TOKEN.

### 🧩 Middleware

Custom Middleware Included:

- Global Exception Handling.

- Role-Based Access Enforcemen.

### 📝 Assumptions

1. 🔐 No authentication or role-based access control was implemented for the Users API endpoints, since the assignment did not explicitly mention access restrictions for these routes.

2. 📄 Roles and permissions are enforced only where they were clearly required or implied by the assignment (e.g., restricting actions like "List Task" to specific roles such as "Admin").

3. 🧪 The project is built for local development/testing purposes. It assumes:

   - Localhost environment.

   - No production-grade logging, exception handling, or security hardening unless stated.
