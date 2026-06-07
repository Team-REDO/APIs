# Docker Setup for Concert API and SOAP API

This directory contains Docker configurations for building and running both the Concert API and SOAP API services with MySQL databases.

## Services Overview

### ConcertAPI
- **Type**: REST API with JWT Authentication
- **.NET Version**: 8.0
- **Database**: MySQL (concertdb)
- **Default Port**: 8000 (mapped from 8080 in container)
- **Database Credentials**: 
  - User: root
  - Password: rootpassword123
  - Database: concertdb

### SoapAPI
- **Type**: SOAP API (with SoapCore)
- **.NET Version**: 8.0
- **Database**: MySQL (soapdb)
- **Default Port**: 8001 (mapped from 8080 in container)
- **Database Credentials**:
  - User: soapuser
  - Password: soappassword123
  - Database: soapdb

## File Structure

```
.
├── docker-compose.yml          # Main Docker Compose orchestration
├── ConcertApi/
│   ├── Dockerfile              # Multi-stage build for ConcertAPI
│   ├── entrypoint.sh           # Startup script with DB healthcheck
│   ├── DBV2.0.sql              # Database schema and initial data
│   └── ... (other service files)
└── SoapApi/
    ├── Dockerfile              # Multi-stage build for SoapAPI
    ├── entrypoint.sh           # Startup script with DB healthcheck
    ├── init-soapdb.sql         # Database user and database creation
    └── ... (other service files)
```

## Quick Start

### Prerequisites
- Docker and Docker Compose installed
- Linux/macOS or Windows with Docker Desktop

### Build and Run

1. **Build and start all services**:
   ```bash
   docker-compose up --build
   ```

2. **Run in background** (detached mode):
   ```bash
   docker-compose up -d --build
   ```

3. **Check service status**:
   ```bash
   docker-compose ps
   ```

4. **View logs**:
   ```bash
   # All services
   docker-compose logs -f

   # Specific service
   docker-compose logs -f concertapi
   docker-compose logs -f soapapi
   docker-compose logs -f mysql
   ```

## Access Services

Once running, you can access the services at:

- **ConcertAPI Swagger**: http://localhost:8000/swagger/index.html
- **SoapAPI Swagger**: http://localhost:8001/swagger/index.html
- **MySQL**: localhost:3306

## Database Initialization

### ConcertAPI Database
- **Schema**: Created from `ConcertApi/DBV2.0.sql`
- **Initial Data**: 
  - 3 users seeded (admin@concert.local, user1@concert.local, user2@concert.local)
  - All with password: Password123!
  - Database seeding runs on application startup in Development mode
- **Connection String**: Points to `concertdb` as root user

### SoapAPI Database
- **Schema**: Created from `SoapApi/init-soapdb.sql`
- **Migrations**: Entity Framework Core migrations run on application startup
- **Seeded Data**: 
  - 1 supplier "ABC Supplies" (created via migrations)
  - Tables: Suppliers, Products, PurchaseOrders, PurchaseOrderLines, AuditLogs
- **Connection String**: Points to `soapdb` as soapuser

## Stopping Services

```bash
# Stop all services (keep volumes)
docker-compose stop

# Stop and remove containers
docker-compose down

# Stop, remove containers AND delete volumes (WARNING: deletes database data!)
docker-compose down -v
```

## Rebuilding Services

```bash
# Rebuild specific service
docker-compose build concertapi
docker-compose build soapapi
docker-compose build mysql

# Rebuild all services
docker-compose build

# Rebuild and restart
docker-compose up --build
```

## Troubleshooting

### MySQL Connection Issues
- Check MySQL is running: `docker-compose logs mysql`
- MySQL container name should be `apis_mysql_db`
- Both APIs wait for MySQL health check before starting

### Database Not Initialized
- Check MySQL logs for initialization errors
- Verify SQL files exist in expected locations
- Delete volumes and restart: `docker-compose down -v && docker-compose up`

### Application Won't Start
- Check application logs: `docker-compose logs concertapi` or `docker-compose logs soapapi`
- Verify environment variables are correctly set
- Check database connectivity from logs

### Port Already in Use
- Change port mappings in `docker-compose.yml`:
  ```yaml
  ports:
    - "8000:8080"  # Change 8000 to another available port
  ```

## Development

### Modifying Application Code
1. Edit code in your IDE
2. Rebuild containers: `docker-compose build`
3. Restart services: `docker-compose up`

### Adding New Environment Variables
1. Update the `environment` section in `docker-compose.yml` for the relevant service
2. Update the corresponding Dockerfile if needed
3. Rebuild and restart

### MySQL Data Persistence
- MySQL data is stored in a Docker volume named `mysql_data`
- Data persists between container restarts
- Delete volume to reset database: `docker-compose down -v`

## Performance Notes

- Health checks are configured with 30-second intervals
- Start period is set to 30+ seconds for applications to initialize
- Volumes use local driver for better performance on most systems

## Security Notes

⚠️ **These credentials are for development only!**

- Change default MySQL passwords in `docker-compose.yml` for production
- Use environment files (.env) instead of hardcoded values
- Never commit sensitive credentials to version control
- Use Docker secrets in production environments

## Next Steps

1. Access the APIs via Swagger UI at the URLs above
2. Use the provided Postman collections to test the services
3. Modify environment variables in `docker-compose.yml` as needed
4. Set up CI/CD pipelines with these Docker configurations
5. Consider using private Docker registries for production deployments
