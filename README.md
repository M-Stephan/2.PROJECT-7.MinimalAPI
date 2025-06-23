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

## 🧪 How to Test

You can use **Swagger** or a tool like **REST Client**, **Postman**, or **curl**.

Examples are provided in `requests.http` file.

## ⚙️ Setup and database

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

5. Once running, use Swagger UI at `http://localhost:5000/swagger` (or `/scalar` if Scalar UI is enabled) to explore and test the endpoints.


6. Run the Docker environment:
```bash
docker-compose up -d
```
---

## 📂 Project Structure
```plaintext
📁7.MinimalAPI
├── 📁.github/workflows
├── 📁bin/
├── 📁obj/
├── 📁Properties/
├── 📁Data/
│        ├── 📄ApplicationDbContext.cs
├── 📁Endpoints/
│        ├── 📄TicketEndpoints.cs
│        └── 📄UserEndpoints.cs
├── 📁Entities/
│        ├── 📄Ticket.cs
│        └── 📄User.cs
├── 📄appsettings.Development.json
├── 📄appsettings.json
├── 📄launchSettings.json
├── 📄Program.cs
├── 📄README.md
├── 📄7.MinimalAPI.csproj
├── 📄7.MinimalAPI.http
├── 📄Dockerfile
├── 📄docker-compose.yml
└── 📄wait-for.sh
```

- ✅ This will:
  - Download the docker-compose.yml file from the GitHub repository
  - Pull the required images from Docker Hub (API + SQL Server)
  - Launch the containers in the background

- 🔗 Once started, the API will be available at:
  - `http://localhost:5000/users`
  - `http://localhost:5000/tickets`
  - `http://localhost:5000/user/4`
  - `http://localhost:5000/ticket/2`

- Scalar UI: `http://localhost:5000/scalar`

---

## 💬 Want to test the live API?

The API is running at:  
🌐 `http://217.145.72.16:5000/scalar`

> For security reasons, contact the author for access please.

---

## Contact
- Stephan .M : martin.stephan9218@gmail.com

