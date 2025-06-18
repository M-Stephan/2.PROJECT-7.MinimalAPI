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

## 🐳 Run the API using Docker (no need to clone)

If you just want to **run the project without cloning or downloading the full repository**, you can do it directly via Docker using the following commands:

```bash
curl -O https://raw.githubusercontent.com/M-Stephan/2.PROJECT-7.MinimalAPI/main/docker-compose.yml
docker-compose up -d
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
