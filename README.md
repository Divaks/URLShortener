# 🔗 URL Shortener Full-Stack Project

A modern web service for shortening long URLs featuring a built-in authorization system, role-based access control, and click analytics.

---

## 🛠 Technology Stack

| Layer | Technologies |
| :--- | :--- |
| **Frontend** | **Angular 18**, TS, RxJS, SCSS, Standalone Components |
| **Backend** | **.NET 8 Web API**, Entity Framework Core |
| **Database** | **PostgreSQL** (or SQL Server) |
| **Security** | **ASP.NET Core Identity**, Cookies, Role-based Auth |

---

## 🚀 Key Features

* **✂️ URL Shortening:** Instantly convert long URLs into compact 8-character codes.
* **👤 User Management:** Secure registration and login powered by encrypted Cookies.
* **🛡 Role-Based Access Control (RBAC):**
    * `Anonymous`: View the global URL table.
    * `User`: Create URLs, delete own records, and view personal statistics.
    * `Admin`: Full control over all links in the system.
* **📊 Analytics:** Automatically track the number of visits (`ClickCount`) for every link.
* **📋 User Experience:** Quick copy-to-clipboard button and seamless server-side redirection.

---

## 📂 Project Structure

The project follows **Clean Architecture** principles:

* **`UrlShortener.Core`**: Domain models (`UrlRecord`), repository interfaces, and core business logic.
* **`UrlShortener.Infrastructure`**: `AppDbContext` implementation, migrations, and database operations via Entity Framework.
* **`UrlShortener.Web`**: API Controllers, Identity configuration, Middleware, and entry points.
* **`url-shortener-client`**: Angular Frontend application featuring reactive forms and Auth Guards.

---

## ⚙️ Generation Algorithm

The application uses a stable hashing algorithm to ensure reliable short codes:
1.  Input URL is processed.
2.  **MD5 Hash** is calculated.
3.  The result is converted to a Hex string, and the first **8 characters** are extracted.
4.  **Collision Resolution:** If the code already exists in the database, a unique `Guid` is appended to the URL salt, and the process repeats until a unique code is generated.

---

## 🚦 How to Run the Project

### 1. Database Configuration (Backend)
Edit the `appsettings.json` file in the `UrlShortener.Web` project and provide your connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=UrlShortenerDb;Username=postgres;Password=your_password"
}
