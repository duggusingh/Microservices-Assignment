# Microservices Platform


##1. How to run using docker-compose

The entire system is containerized for easy orchestration. To spin up the SQL Server, Backend Services, and Frontend, run the following command in the root directory:

```bash
docker-compose up --build -d

Access points:
Frontend: http://localhost:4200
Product API (Swagger): http://localhost:8081/swagger
Order API (Swagger): http://localhost:8082/swagger

## 2. How to run using docker-compose

Ensure a local instance of MSSQL is running.
Create a container named durgesh-product-images in your Azure Storage Account.

Backend: Navigate to backend/product-service/ and backend/order-service/ and run: dotnet run
Frontend: Navigate to the frontend/ directory and execute:
npm install
ng serve


## step 3:  How to apply DB migrations

The project uses Entity Framework Core with two separate databases: DurgeshProductDb and DurgeshOrderDb. To initialize the schemas, run these commands from the root directory:
Product Service Migrations: dotnet ef database update --project backend/product-service
Order Service Migrations:
dotnet ef database update --project backend/order-service

##step 4:  How to run tests

The solution includes a suite of unit tests for the Order Service to validate critical business logic, such as stock availability and error handling.
To execute the tests, run:
dotnet test backend/order-service/OrderService.csproj

Included Test Cases:
PlaceOrder_ShouldReturnTrue_WhenStockIsSufficient
PlaceOrder_ShouldReturnFalse_WhenStockIsInsufficient
PlaceOrder_ShouldReturnFalse_WhenProductDoesNotExist


## step 5:. Any assumptions or tradeoffs

Assumptions

Data Consistency: I assumed that real-time stock validation is mandatory for this business case. Therefore, the Order Service performs a direct synchronous HTTP call to the Product Service to ensure stock is available before committing a transaction.

Tradeoffs

Availability vs. Consistency: By choosing synchronous communication, I prioritized Consistency. A tradeoff is that if the Product Service is unavailable, the Order Service cannot process orders. For a production environment, a fallback or circuit breaker pattern would be the next step.
Storage Offloading: I chose to store product images in Azure Blob Storage instead of the SQL database. This keeps the DurgeshProductDb lightweight and performant, but introduces a dependency on cloud availability.
Service Discovery: For this local environment, service URLs are managed via Docker environment variables. In a larger production setup, a service mesh or discovery tool (like Consul or Azure API Management) would be preferred.
