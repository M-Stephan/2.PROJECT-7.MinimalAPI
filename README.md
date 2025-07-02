# 2.PROJECT-7.MinimalAPI

## Author
- Stephan .M

## Description
- This is a minimal Web API built using **ASP.NET 8**, following BeCode's challenge for managing support tickets and users.

## 🚀 Project Features

- Minimal API architecture (no controllers, lightweight)
- Endpoints for:
  - ✅ Users: CRUD (Create, Read, Update, Delete)
  - ✅ Tickets: CRUD with status management
- OpenAPI (Swagger) integration
- Database support with **Entity Framework Core** and SQL Server
- Automatic migrations applied at startup (database schema and initial data)
- Clear code separation by namespace (Users, Tickets, Endpoints)
- Optional: Simple data validation

## 📁 Project Structure
<details><summary>Watch Here</summary>

```plaintext
7.MinimalAPI
📁7.MinimalAPI
├── 📁.github/workflows
├── 📁bin/
├── 📁obj/
├── 📁Migrations/
├── 📁Properties/
├── 📁Controllers/
│        ├── 📄TicketController.cs
│        └── 📄UserController.cs
├── 📁Data/
│        └── 📄ApplicationDbContext.cs
├── 📁Entities/
│        ├── 📁DTOs/
│        │        ├── 📄LoginRequestDTO.cs
│        │        ├── 📄LoginResponseDTO.cs
│        │        ├── 📄RegisterRequestDTO.cs
│        │        ├── 📄TicketDTO.cs
│        │        └── 📄UserDTO.cs
│        └── 📁Models/
│                 ├── 📄Ticket.cs
│                 └── 📄User.cs
├── 📁Services/
│        ├── 📄ITicketService.cs
│        ├── 📄IUserService.cs
│        ├── 📄TicketService.cs
│        └── 📄UserService.cs
├── 📄appsettings.Development.json
├── 📄appsettings.json
├── 📄launchSettings.json
├── 📄Program.cs
├── 📄7.MinimalAPI.csproj
├── 📄7.MinimalAPI.http
├── 📄Dockerfile
├── 📄docker-compose.yml
└── 📄README.md
```
</details>

## 💬 Want to test the live API without downloads?
- The API is running at:
  - 🌐 Scalar UI: [→ Click Here ←](http://217.145.72.16:5000/scalar)
  - 🌐 Swagger UI: [→ Click Here ←](http://217.145.72.16:5000/swagger/index.html)

- You can register a user and login to get a token to test the API (token valid for 2 hours).
- Once logged in, paste your token in the Bearer Token field to authenticate and use API.

## ⚙️ Setup and database
<details><summary>Watch Here</summary>
After cloning this repository, follow these steps to run the API properly:

1. **Configure the database connection** in `appsettings.json` (or environment variables) under `"DefaultConnection"`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=MaDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
   }
   ```
   Replace the connection string with your SQL Server credentials and database name.

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```
   The API will automatically apply any pending Entity Framework Core migrations to update the database schema and seed initial data (users and tickets).

4. Alternatively, you can apply migrations manually before running the app:
   ```bash
   dotnet ef database update
   ```

5. Install Docker Desktop and Setup [→ How to install ←](https://docs.docker.com/desktop/setup/install/windows-install/)

6. Run the Docker environment:
   ```bash
   docker-compose up -d --build
   ```
</details>

## 🧪 How to Test
- 🔗 Once started, the API will be available at:
  - Scalar UI: `http://localhost:5000/scalar`
  - Swagger UI: `http://localhost:5000/swagger/index.html`

## Contact
- Stephan .M : martin.stephan9218@gmail.com

