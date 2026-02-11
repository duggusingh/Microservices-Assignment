## Microservices Architecture


## 📋 Prerequisites
* Docker Desktop (Running)
* .NET 8.0 SDK

---

 ##Step 1: Configuration

Before running the application, you must provide a valid Azure Storage Connection String for image uploads to work.

1. Open `backend/product-service/appsettings.json`.
2. Replace the placeholder in `AzureBlob:ConnectionString` with your actual Azure Storage connection string.

```json
"AzureBlob": {
  "ConnectionString": "YOUR_AZURE_CONNECTION_STRING_HERE",
  "ContainerName": "durgesh-product-images"
}

## Step 2: Docker command

docker-compose up --build -d

## Step 3: initialise database

dotnet ef database update --project backend/product-service
dotnet ef database update --project backend/order-service

## Step 4: Run unit tests

dotnet test backend/order-service/OrderService.csproj

