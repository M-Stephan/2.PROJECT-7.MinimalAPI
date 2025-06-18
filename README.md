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
- In-memory data storage (no database)
- Clear code separation by namespace (Users, Tickets, Endpoints)
- Optional: Simple data validation

## 🧪 How to Test

You can use **Swagger** or a tool like **REST Client**, **Postman**, or **curl**.

Examples are provided in `requests.http` file.

## 📂 Project Structure
```plaintext
📁7.MinimalAPI
├── 📁bin/
├── 📁obj/
├── 📁Properties/
├── 📁Endpoints/
│         ├── 📄TicketEndpoints.cs
│         └── 📄UserEndpoints.cs
├── 📁 Models/
│         ├── 📄Ticket.cs
│         └── 📄User.cs
├── 📄appsettings.Development.json
├── 📄appsettings.json
├── 📄launchSettings.json
├── 📄Program.cs
├── 📄README.md
├── 📄7.MinimalAPI.csproj
└── 📄7.MinimalAPI.http
```

