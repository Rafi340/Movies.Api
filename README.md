# 🎬 MovieRestApi

A full-featured, modular REST API for movie management, built with **ASP.NET Core** and following best practices like clean architecture, modular structure, and robust authentication.

---
## 🔧 Features

- ✅ **CRUD operations** for movies  
- 🗄️ **PostgreSQL** integration with Dapper  
- 🔐 **JWT-based** authentication & authorization  
- ⭐ **User ratings** for movies  
- 🔍 **Advanced features**: filtering, pagination, caching  
- 📦 **Refit-based SDK** client  
- 🔻 **Minimal API** version supported  

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)

---

### 🛠️ Installation & Run

```bash
# Clone the repository
git clone https://github.com/Rafi340/Movies.Api.git
cd Movies.Api

# Start PostgreSQL using Docker
docker compose up -d

# Restore dependencies
dotnet restore

# Run the application
dotnet run

🌐 Access the API at:
https://localhost:5001
http://localhost:5000

🧪 API Testing with Postman
### **This project includes a Postman collection to simplify testing:**
 - Open Postman
 - Click Import
 - Select the MovieRestApi.postman_collection.json file from the repo
 - Explore all endpoints organized by feature
