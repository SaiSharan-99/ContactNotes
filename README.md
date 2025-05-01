Contact Notes API (.NET 7)
A RESTful API for managing contacts and notes — with JWT authentication, field normalization, and full test coverage.

## How to Run the Service Locally

### Prerequisites:
- .NET 7 SDK
- SQL Server (local or Docker)
- Visual Studio or VS Code

### Setup Instructions:

1. **Clone the Repository**
```bash
git clone https://github.com/your-username/contact-notes-api.git
cd contact-notes-api
```

2. **Update the Connection String**
Update `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ContactNotesDb;Trusted_Connection=True;"
}
```

3. **Run Database Migrations**
```bash
dotnet ef database update
```

4. **Run the Application**
```bash
dotnet run
```
Then open: `https://localhost:{port}/swagger`

---

## Authentication

### POST `/auth/register`
```json
{
  "username": "johnsmith",
  "password": "secure123"
}
```

### POST `/auth/login`
```json
{
  "username": "johnsmith",
  "password": "secure123"
}
```

Use the token:
```
Authorization: Bearer your.jwt.token.here
```

---

## Contacts Endpoints

| Method | Endpoint           | Description              |
|--------|--------------------|--------------------------|
| GET    | `/contacts`        | Get all contacts         |
| POST   | `/contacts`        | Create a new contact     |
| PUT    | `/contacts/{id}`   | Update a contact         |
| DELETE | `/contacts/{id}`   | Delete a contact         |

---

## Notes Endpoints

| Method | Endpoint                      | Description                   |
|--------|-------------------------------|-------------------------------|
| GET    | `/notes`                      | Get all notes                 |
| GET    | `/notes/contact/{contactId}` | Notes for a specific contact |
| POST   | `/notes`                      | Create a new note (with field normalization) |
| PUT    | `/notes/{id}`                 | Update a note                |
| DELETE | `/notes/{id}`                 | Delete a note                |

Accepts fields like `note_body`, `noteBody`, `text`, etc.

---

## Key Design Decisions

- Clean architecture with Controllers, Services, Repositories, DTOs, and Models
- Used Input Formatter for flexible field normalization
- JWT authentication for secure endpoints
- Unit tested using xUnit, Moq, and FluentAssertions

---

## Tradeoffs & Assumptions

- Manual validation; could be improved with FluentValidation
- No refresh token flow implemented
- No real queue/event system yet — but designed for extension

---

## What I'd Add With More Time

- Real message queue for event-driven note enrichment
- Docker + `docker-compose` for fast startup
- Cloud deployment (Azure App Service or AWS)
- Full integration test suite using `WebApplicationFactory`
- Role-based authorization (`Admin`, `User`)
- Search/filter support on notes

---

##  Testing

Unit tests cover:
- ContactService
- NoteService
- AuthService
- NotesController

To run tests:
```bash
dotnet test
```

---

## Swagger UI
```
https://localhost:{port}/swagger
```
Click "Authorize", paste the token, and test protected routes easily.
