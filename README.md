# Central_WebApi

A robust backend system providing Content Management, Reservation, and Booking APIs for an E-Commerce platform.

## 🔗 Related Projects
- Frontend Repository: [E-Commerce Frontend](https://github.com/sairam-T2002/E-Commerce-App.git)

## 📋 Overview
This backend system serves as the core infrastructure for my E-Commerce app, serving content, product reservations, and booking functionalities. Built with scalability and performance in mind.

## 🚀 Features
- **Content Management API**
  - Media asset handling
  - Content versioning

- **Reservation System**
  - Real-time inventory tracking
  - Hold management
  - Reservation expiry handling
  - Conflict resolution

- **Booking API**
  - Order processing
  - Booking confirmation

## 🛠️ Tech Stack
- Framework: ASP.NET Core 8 WebApi
- Database: PostgreSQL

## ⚙️ Prerequisites
```bash
# List your prerequisites here
Dotnet SDK >= 8.0.0
PostgreSQL >= 13
```

## 🔧 Installation

1. Clone the repository
```bash
git clone https://github.com/sairam-T2002/Central_WebApi.git
cd Central_WebApi/Central_WebApi
```

2. Start the server
```bash
dotnet run    # Development
dotnet publish -c Release -o publish  # Production
```

## 📚 API Documentation - OpenAPI is Enabled

## 🔐 Security
- JWT Authentication
- Rate Limiting
- Input Validation
- Data Encryption
