# FastTrak API

REST API serving restaurant menu and nutrition data for the FastTrak mobile app.

## Quick Start

```bash
cd FastTrak.Api
dotnet run --urls "http://*:5050"
```

- Swagger UI: http://localhost:5050/swagger
- Health check: http://localhost:5050/health

## Endpoints

### Restaurants

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/restaurants` | List all restaurants |
| GET | `/api/v1/restaurants/{id}` | Get restaurant by ID |
| GET | `/api/v1/restaurants/{id}/menu-items` | Get menu items (paginated) |

**Query Parameters** for menu-items:
- `page` (default: 1)
- `pageSize` (default: 20, max: 100)
- `category` (optional filter)

### Menu Items

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/menu-items/{id}` | Get item with nutrition data |
| GET | `/api/v1/menu-items/{id}/options` | Get customization options |

### Response Format

All endpoints return:
```json
{
  "data": { ... },
  "meta": { "page": 1, "pageSize": 20, "totalCount": 50 }
}
```

## Configuration

- **Database**: SQL Server LocalDB (development)
- **Rate Limiting**: 100 requests/minute per IP
- **Logging**: Serilog (console)

## Tech Stack

- ASP.NET Core 9 (Minimal APIs)
- Entity Framework Core 9
- SQL Server LocalDB
- Swashbuckle (Swagger)
- AspNetCoreRateLimit
- Serilog
