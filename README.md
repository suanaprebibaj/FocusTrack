# FocusTrack Backend

The project is built with .NET and follows a clean architecture approach.  
The main goal is to track focus sessions, process rewards asynchronously, and support public sharing of sessions.

This project is implemented as a Clean Architecture / Modular Monolith.  
It uses asynchronous background processing with RabbitMQ and PostgreSQL.

---

## Solution Structure

FocusTrack.Api  
FocusTrack.Application  
FocusTrack.Domain  
FocusTrack.Infrastructure  
FocusTrack.RewardWorker  
FocusTrack.NotificationWorker  

---

## Running the Project

### 1. Start infrastructure

PostgreSQL and RabbitMQ are started using Docker.

```bash
docker compose -f docker-compose.infra.yml up -d
```

### 2. Apply database migrations 
```bash
dotnet ef database update -p FocusTrack.Infrastructure -s FocusTrack.Api
```

### 3. Api Controllers
The API exposes endpoints for managing focus sessions and public sharing links.
The following endpoints were tested successfully via Swagger:
POST /api/Sessions
GET /api/Sessions
PUT /api/Sessions/{id}
POST /api/PublicLinks/{sessionId}
Admin endpoints are available under the /admin route.
These endpoints are protected by authorization policies and are not accessible in the local development setup.
They require authentication and admin claims provided by an external identity provider.

### 4. Backgroung Processing
Rewards and notifications are processed asynchronously using background workers.
RewardWorker handles daily focus-time reward logic
NotificationWorker handles user notifications
Communication between the API and workers is done using RabbitMQ.

### 5. Testing
Unit tests are included for the reward calculation logic in the RewardWorker.
The tests cover the required edge cases around the 120-minute daily threshold, including boundary conditions.



