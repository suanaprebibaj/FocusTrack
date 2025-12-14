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
