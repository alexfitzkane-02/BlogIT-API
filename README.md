![Build](https://github.com/alexfitzkane-02/BlogIT-API/actions/workflows/ci.yml/badge.svg)


# BlogIT-API

BlogIT-API is a C# library that contains all the server side logic for performing CRUD operations related to blog posts users make, credentialing around the application, and more!  

This project makes use of Azure Key Vault storage. This allows me to store all sensitive information in the cloud, such as database connection strings which contains a username and password, and keep that out of the application. 

## Nuget Packages

The following packages are needed for the core function of the application
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore.SwaggerUI

The following packages are needed for Jwt Authentication
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
- System.IdentityModel.Tokens.Jwt

The following packages are needed to hook into Azure Key Vault 
- Azure.Extensions.AspNetCore.Configuration.Secrets
- Azure.Identity
- Azure.Security.KeyVault.Secrets

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Azure Blob Storage account (for image uploads)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/alexfitzkane-02/BlogIT-API.git
   cd BlogIT-API
   ```

2. **Configure `appsettings.json`**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your SQL Server connection string"
     },
     "Jwt": {
       "Key": "your-secret-key",
       "Issuer": "your-issuer",
       "Audience": "your-audience"
     },
     "AzureBlobStorage": {
       "ConnectionString": "your-azure-blob-connection-string",
       "ContainerName": "your-container-name"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   dotnet run
   ```

5. **Explore with Swagger**
   Navigate to `https://localhost:{port}/swagger` for interactive API docs.

---

## 🔐 Authentication

Protected routes require a Bearer token in the `Authorization` header:

```
Authorization: Bearer <your_jwt_token>
```

Routes marked with 🔒 require authentication with the **Writer** role.

---

## 📋 API Endpoints

### Authors — `/api/author`

#### `GET /api/author`
Get all authors. *(Public)*

**Response `200 OK`:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Alex Kane",
    "urlHandle": "alex-kane"
  }
]
```

---

#### `GET /api/author/{id}`
Get a single author by ID. *(Public)*

**Response `200 OK`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Alex Kane",
  "urlHandle": "alex-kane"
}
```

**Response `404 Not Found`:** `"Author not found"`

---

#### `POST /api/author` 🔒
Create a new author.

**Request body:**
```json
{
  "name": "Alex Kane",
  "urlHandle": "alex-kane"
}
```

**Response `200 OK`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Alex Kane",
  "urlHandle": "alex-kane"
}
```

---

#### `PUT /api/author/{id}` 🔒
Update an existing author.

**Request body:**
```json
{
  "name": "Alexander Kane",
  "urlHandle": "alexander-kane"
}
```

**Response `200 OK`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Alexander Kane",
  "urlHandle": "alexander-kane"
}
```

**Response `404 Not Found`:** `"Author not found"`

---

#### `DELETE /api/author/{id}` 🔒
Delete an author by ID.

**Response `200 OK`:** Returns the deleted author object.

**Response `404 Not Found`:** `"Author not found"`

---

### Categories — `/api/category`

#### `GET /api/category`
Get all categories. *(Public)*

**Response `200 OK`:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Technology",
    "urlHandle": "technology"
  }
]
```

---

#### `GET /api/category/{id}`
Get a single category by ID. *(Public)*

**Response `200 OK`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Technology",
  "urlHandle": "technology"
}
```

**Response `404 Not Found`:** `"Category not found"`

---

#### `POST /api/category` 🔒
Create a new category.

**Request body:**
```json
{
  "name": "Technology",
  "urlHandle": "technology"
}
```

**Response `200 OK`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Technology",
  "urlHandle": "technology"
}
```

---

#### `PUT /api/category/{id}` 🔒
Update an existing category.

**Request body:**
```json
{
  "name": "Tech & Science",
  "urlHandle": "tech-and-science"
}
```

**Response `200 OK`:** Returns the updated category object.

**Response `404 Not Found`:** `"Category not found"`

---

#### `DELETE /api/category/{id}` 🔒
Delete a category by ID.

**Response `200 OK`:** Returns the deleted category object.

**Response `404 Not Found`:** `"Category not found"`

---

### Blog Posts — `/api/blog`

#### `GET /api/blog`
Get all blog posts. *(Public)*

**Response `200 OK`:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "My First Post",
    "description": "This is the content of the post.",
    "author": {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Alex Kane",
      "urlHandle": "alex-kane"
    },
    "featuredImageUrl": "https://yourstorage.blob.core.windows.net/images/example.jpg",
    "urlHandle": "my-first-post",
    "isVisible": true,
    "createdTimeStamp": "2025-01-01",
    "lastEditTimeStamp": "2025-01-02",
    "categories": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Technology",
        "urlHandle": "technology"
      }
    ]
  }
]
```

---

#### `GET /api/blog/{id}`
Get a single blog post by GUID. *(Public)*

**Response `200 OK`:** Returns a single blog post object (same shape as above).

**Response `404 Not Found`:** `"Blog post was not found"`

---

#### `GET /api/blog/{urlHandle}`
Get a single blog post by its URL handle. *(Public)*

Example: `GET /api/blog/my-first-post`

**Response `200 OK`:** Returns a single blog post object.

**Response `404 Not Found`:** `"Blog post was not found"`

---

#### `POST /api/blog` 🔒
Create a new blog post.

**Request body:**
```json
{
  "title": "My New Post",
  "description": "This is the content of the new post.",
  "author": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "featuredImageUrl": "https://yourstorage.blob.core.windows.net/images/example.jpg",
  "urlHandle": "my-new-post",
  "isVisible": true,
  "categories": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```

> ⚠️ `author` must be a valid existing Author GUID. `categories` is a list of existing Category GUIDs.

**Response `200 OK`:** Returns the created blog post object.

**Response `500 Internal Server Error`:** `"An error occurred while creating the blog post."`

---

#### `PUT /api/blog/{id}` 🔒
Update an existing blog post.

**Request body:** Same shape as `POST /api/blog`.

**Response `200 OK`:** Returns the updated blog post object.

**Response `404 Not Found`:** Author, category, or post not found.

---

#### `DELETE /api/blog/{id}` 🔒
Delete a blog post by ID.

**Response `200 OK`:** Returns the deleted blog post object.

**Response `404 Not Found`:** `"Blog not found"`

---

### Images — `/api/images`

#### `GET /api/images`
Get all uploaded images. *(Public)*

**Response `200 OK`:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "fileName": "example",
    "fileExtension": ".jpg",
    "title": "My Image",
    "url": "https://yourstorage.blob.core.windows.net/images/example.jpg",
    "dateCreated": "2025-01-01T12:00:00Z"
  }
]
```

---

#### `POST /api/images/upload`
Upload an image to Azure Blob Storage. *(Public)*

**Request:** `multipart/form-data`

| Field | Type | Description |
|-------|------|-------------|
| `file` | File | Image file (`.jpg`, `.jpeg`, `.png` only, max 10MB) |
| `fileName` | string | Desired file name |
| `title` | string | Display title for the image |

**Response `200 OK`:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fileName": "example",
  "fileExtension": ".jpg",
  "title": "My Image",
  "url": "https://yourstorage.blob.core.windows.net/images/example.jpg",
  "dateCreated": "2025-01-01T12:00:00Z"
}
```

**Response `400 Bad Request`:** Unsupported file type or file exceeds 10MB.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core (.NET 8)
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** JWT Bearer tokens
- **Image Storage:** Azure Blob Storage
- **API Docs:** Swagger / Swashbuckle

---

## 📁 Project Structure

```
BlogIT/
├── Controllers/       # API controllers (Blog, Category, Author, Images)
├── Models/
│   ├── Domain/        # Entity models
│   └── Dto/           # Data Transfer Objects
├── Repositories/
│   ├── Interface/     # Repository interfaces
│   └── Implementation/ # Repository implementations
├── Data/              # EF Core DbContext
└── Program.cs
```

## 📁 Azure

When running the application locally on your machine, you need to make sure you have Azure CLI installed locally. To check if you already have it installed you can perform the following CMD promt: 

```bash
az version
```
If you do not have it, you can install it through PowerShell with the following command:

```powershell
winget install --exact --id Microsoft.AzureCLI
```

Once you confirm  the Azure CLI is installed, restart Visual Studio and login through the Package Manager Console with the following command:

```bash
az login
```

In certain scenarios you might need to append your tenantID to the command: 

```bash
az login --tenant {insert tenantID}
```

Please make sure to update tests as appropriate.
