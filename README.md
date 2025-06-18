🎬 MovieRestApi
A full-featured REST API for movie management built with ASP.NET Core.

🔧 Features
✅ CRUD operations for movies
🗄️ PostgreSQL integration with Dapper
🔐 JWT-based authentication & authorization
⭐ Movie ratings
🔍 Advanced features: filtering, pagination, caching
📦 SDK using Refit
🔻 Minimal API version available


🚀 Getting Started

Prerequisites
.NET 9 SDK
Docker

# Clone the repo
git clone [https://github.com/Rafi340/Movies.Api.git]

# Start PostgreSQL via Docker
docker compose up -d

# Restore and run the API
dotnet restore
dotnet run
Access the API at:
🌐 https://localhost:5001 or http://localhost:5000

🧪 API Testing
Use the included MovieRestApi.postman_collection file:

Open Postman → Import

Select the .postman_collection file from the repo

Explore requests organized by feature

