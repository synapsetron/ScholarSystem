# ScholarSystem

**ScholarSystem** is a modern **educational management system** built with **Clean Architecture**, designed to efficiently handle **teacher, course, and student administration**. The project adheres to best software development practices, leveraging **MediatR, CQRS, Repository Pattern, and Unit of Work** for maintainability and scalability.

---

## 🚀 Tech Stack & Architecture

### 📌 Clean Architecture with Layered Structure

The project structure ensures clear separation of concerns:

- **`ScholarSystem.Domain`** – Contains core business logic and domain entities.
- **`ScholarSystem.Application`** – Implements MediatR handlers, DTOs, business rules, and application services.
- **`ScholarSystem.Infrastructure`** – Implements database interactions using Entity Framework Core and the repository pattern.
- **`ScholarSystem.WebAPI`** – Exposes API endpoints through ASP.NET Core.

### 📌 Patterns & Libraries Used

| Category                 | Technologies / Patterns                        |
|--------------------------|------------------------------------------------|
| **Backend**              | ASP.NET Core Web API                           |
| **Database**             | MariaDB (via EF Core)                          |
| **ORM**                  | Entity Framework Core                          |
| **Design Patterns**      | Repository, Unit of Work, Options Pattern      |
| **Architecture**         | Clean Architecture, CQRS                       |
| **MediatR**              | CQRS Implementation                            |
| **Validation**           | FluentValidation                               |
| **Logging**              | Serilog                                        |
| **Mapping**              | AutoMapper                                     |
| **Error Handling**       | FluentResults                                  |
| **Dependency Injection** | ASP.NET Core DI                                |

---

## 🔧 Setup & Installation

### 1️⃣ Prerequisites

Ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- MariaDB (if not using Docker)
- [Postman](https://www.postman.com/) (for API testing)

### 2️⃣ Environment Variables

Create a `.env` file in the root directory and add:

```env
MYSQL_ROOT_PASSWORD=rootpassword
MYSQL_DATABASE=mydatabase
MYSQL_USER=myuser
MYSQL_PASSWORD=mypassword
```

### 3️⃣ Configure User Secrets for WebAPI

Navigate to the `ScholarSystem.WebAPI` directory and run:

```sh
dotnet user-secrets set "Database:ServerVersion" "10.6.14-mariadb"
dotnet user-secrets set "Database:DefaultConnection" "Server=mariadb;Port=3306;Database=mydatabase;User=root;Password=rootpassword;Protocol=TCP;"
```

### 4️⃣ Running the Application

#### Option 1: Using Docker (Recommended)

Run the following command in the root directory:

```sh
docker-compose up --build
```

#### Option 2: Running Manually

Follow these steps to run the application locally:

1. **Restore dependencies:**

```sh
dotnet restore
```

2. **Build the solution:**

```sh
dotnet build
```

3. **Start MariaDB** (locally or via Docker).

4. **Apply database migrations:**

```sh
dotnet ef database update --project ScholarSystem.Infrastructure --startup-project ScholarSystem.WebAPI
```

5. **Run the Web API with .NET 9:**

```sh
dotnet run --project ScholarSystem.WebAPI
```

---

## 📌 API Documentation

Once the API is running, access Swagger UI at:

- [http://localhost:5000/swagger](http://localhost:5000/swagger)

Or use Postman by importing the OpenAPI specification.

### 📌 phpMyAdmin Access

If using Docker, phpMyAdmin is accessible via:

- [http://localhost:8080](http://localhost:8080)

---

## 🎯 Key Features

- ✅ **Teacher Management** – Create, update, delete, and retrieve teachers.
- ✅ **Course Administration** – Assign teachers, manage courses, and track student enrollments.
- ✅ **Student Management** – Create, update, delete, and retrieve students.
- ✅ **MediatR & CQRS** – Separates Queries (read operations) and Commands (write operations) for enhanced scalability.
- ✅ **Repository & Unit of Work** – Ensures reliable data persistence.
- ✅ **FluentValidation & FluentResults** – Provides structured error handling and validation.
- ✅ **Docker Support** – Easy deployment via docker-compose.

