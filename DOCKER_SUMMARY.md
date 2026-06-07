# Docker Configuration Summary

## Created Files

### Root Level
- **docker-compose.yml** - Orchestrates all services with MySQL database
- **DOCKER_SETUP.md** - Comprehensive usage guide

### ConcertApi/
- **Dockerfile** - Multi-stage build for .NET 8.0 REST API
- **entrypoint.sh** - Startup script with MySQL health check

### SoapApi/
- **Dockerfile** - Multi-stage build for .NET 8.0 SOAP API
- **entrypoint.sh** - Startup script with MySQL health check
- **init-soapdb.sql** - Database initialization (user and database creation)

### WebsocketApi/
- **Dockerfile** - Multi-stage build for .NET 10.0 WebSocket Chat API
- **entrypoint.sh** - Startup script with MySQL health check
- **init-websocketdb.sql** - Database initialization (user and database creation)

## Architecture

```
docker-compose.yml
├── MySQL 8.0 Service
│   ├── Database: concertdb (initialized from ConcertApi/DBV2.0.sql)
│   ├── Database: soapdb (initialized from SoapApi/init-soapdb.sql)
│   ├── Database: websocketdb (initialized from WebsocketApi/init-websocketdb.sql)
│   ├── User: root/rootpassword123
│   ├── User: soapuser/soappassword123
│   ├── User: websocketuser/websocketpass123
│   └── Port: 3306 (exposed on host)
│
├── ConcertAPI Service
│   ├── Image: .NET 8.0 aspnet runtime
│   ├── Build: Multi-stage from SDK 8.0
│   ├── Port: 8000 (container: 8080)
│   ├── Environment: Development (enables seeding)
│   ├── Database: concertdb (root user)
│   ├── Entrypoint: Waits for MySQL, then starts app
│   └── Seeding: 3 demo users (admin, user1, user2)
│
├── SoapAPI Service
│   ├── Image: .NET 8.0 aspnet runtime
│   ├── Build: Multi-stage from SDK 8.0
│   ├── Port: 8001 (container: 8080)
│   ├── Environment: Development (enables migrations/seeding)
│   ├── Database: soapdb (soapuser)
│   ├── Entrypoint: Waits for MySQL, then starts app
│   └── Migrations: Auto-run on startup (EF Core)
│
└── WebSocket API Service
    ├── Image: .NET 10.0 aspnet runtime
    ├── Build: Multi-stage from SDK 10.0
    ├── Port: 5001 (container: 5001, HTTPS)
    ├── Environment: Development (enables migrations)
    ├── Database: websocketdb (websocketuser)
    ├── Entrypoint: Waits for MySQL, then starts app
    ├── Migrations: Auto-run on startup (EF Core)
    └── Certificate: Self-signed cert generated for HTTPS (dev only)
```

## Database Initialization Flow

### ConcertApi
1. MySQL container starts
2. DBV2.0.sql is executed during MySQL initialization
3. Creates: concertdb, schema, and relationships
4. ConcertApi starts after MySQL health check passes
5. Application seeding runs (DbSeeder.SeedAsync) in Development mode
6. Creates 3 users and populates initial data

### SoapApi
1. MySQL container starts (already initialized)
2. init-soapdb.sql is executed during MySQL initialization
3. Creates: soapdb database and soapuser account
4. SoapApi starts after MySQL health check passes
5. Entity Framework migrations run automatically on startup
6. Creates tables and seeds initial data (1 supplier via migration)

### WebSocket API
1. MySQL container starts (already initialized)
2. init-websocketdb.sql is executed during MySQL initialization
3. Creates: websocketdb database and websocketuser account
4. WebSocket API starts after MySQL health check passes
5. Entity Framework migrations run automatically on startup
6. Creates tables for Users, ChatRooms, ChatMessages, UsersHasChatRooms
7. Optionally seeds sample data from SeedData.sql

## How to Use

### Quick Start
```bash
# Build and start all services
docker-compose up --build

# Or start in background
docker-compose up -d --build
```

### Access Services
- ConcertAPI: http://localhost:8000/swagger/index.html
- SoapAPI: http://localhost:8001/swagger/index.html
- WebSocket API: wss://localhost:5001/ws/{userId}

### Stop Services
```bash
docker-compose down        # Stop containers
docker-compose down -v     # Stop and delete volumes (data)
```

### View Logs
```bash
docker-compose logs -f                 # All services
docker-compose logs -f concertapi      # Specific service
docker-compose logs -f soapapi
docker-compose logs -f websocketapi
docker-compose logs -f mysql
```

## Database Access

Direct MySQL connection:
- Host: localhost:3306
- Root User: root/rootpassword123
- ConcertDB: concertdb (root user)
- SoapDB: soapdb (soapuser/soappassword123)
- WebSocketDB: websocketdb (websocketuser/websocketpass123)

## Key Features

✓ Multi-stage Docker builds (optimized images)
✓ Health checks on all services
✓ Proper service dependency management
✓ Automatic database initialization
✓ Database seeding and migrations
✓ Persistent MySQL volumes
✓ Development environment setup
✓ Network isolation between containers
✓ Entrypoint scripts for startup control

## Important Notes

- **Credentials**: These are for development only. Change in production.
- **Persistence**: MySQL data is stored in a Docker volume (`mysql_data`)
- **Environment**: Set to Development to enable seeding/migrations
- **Ports**: Map to 8000 and 8001 on host; adjust in docker-compose.yml if needed
- **WebSocket**: Uses HTTPS on port 5001; certificate is self-signed (development only)
- **Health Checks**: 30-second intervals with 30-40 second start period

## File Dependencies

```
ConcertApi/
├── Dockerfile          → Uses ConcertApi.csproj, entrypoint.sh
├── entrypoint.sh       → Waits for MySQL before starting
└── DBV2.0.sql          → Pre-existing schema file (used for init)

SoapApi/
├── Dockerfile          → Uses SoapApi.csproj, entrypoint.sh
├── entrypoint.sh       → Waits for MySQL before starting
└── init-soapdb.sql     → Creates database and user

WebsocketApi/
├── Dockerfile          → Uses WebsocketApi.csproj, entrypoint.sh
├── entrypoint.sh       → Waits for MySQL before starting
└── init-websocketdb.sql → Creates database and user

docker-compose.yml     → Mounts all three .sql files into MySQL init
```

## Troubleshooting

If services fail to start:
1. Check Docker is running
2. Check Docker Compose version: `docker-compose --version`
3. Validate config: `docker-compose config`
4. View logs: `docker-compose logs mysql` or `docker-compose logs <service>`
5. Reset everything: `docker-compose down -v && docker-compose up --build`
