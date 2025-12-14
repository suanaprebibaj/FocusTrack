The project is built with .NET and follows a clean architecture approach.
The main goal is to track focus sessions, process rewards asynchronously, and support public sharing of sessions.
Solution Structure
FocusTrack.Api
FocusTrack.Application
FocusTrack.Domain
FocusTrack.Infrastructure
FocusTrack.RewardWorker
FocusTrack.NotificationWorker

1. Start infrastructure

PostgreSQL and RabbitMQ are started using Docker:docker compose -f docker-compose.infra.yml up -d

2. Apply database migrations: dotnet ef database update -p FocusTrack.Infrastructure -s FocusTrack.Api

API Usage

The following endpoints were tested successfully via Swagger:

POST /api/Sessions
GET /api/Sessions
POST /api/PublicLinks/{sessionId}

Admin endpoints are protected by authorization and are not accessible in the local development setup.
They require authentication and admin claims provided by an external
identity provider.

Background Processing

Rewards and notifications are processed asynchronously using RabbitMQ.
Messages can be inspected via the RabbitMQ management UI at: http://localhost:15672

Testing
Unit tests are included for the reward calculation logic in the RewardWorker.
They cover the required edge cases around the 120-minute daily threshold.
