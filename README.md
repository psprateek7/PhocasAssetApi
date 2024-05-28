# Phocas Asset api

This repository contains a .NET 8 WebAPI project that demonstrates the integration with DynamoDB for managing GPS data of simulated vehicles. The API supports operations such as querying events based on asset and time range, retrieving single events by ID, fetching the latest events for all assets, and events for a specific asset and trip.

## Features

- **Query Support**: Queries based on asset and time.
- **Docker Integration**: Runs in a Docker container.
- **Local DynamoDB Instance**: Utilizes a local DynamoDB instance for development.


## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - if building/running the project locally.
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Getting Started

Clone the repository to your local machine and start the application:

```bash
git clone https://github.com/psprateek7/PhocasAssetApi.git
cd PhocasAssetApi/PhocasAsset
docker-compose up --build
```

Access the application at
`http://localhost:8080`

Run unit tests
```bash
cd ../PhocasAssetApi/PhocasAsset.Tests
dotnet test
```

## API Endpoints

Each endpoint's function and required query parameters are described below:

### `GET /getEventById`
- **id** (string): The unique identifier for the event you wish to retrieve.

### `GET /getEventByAssetAndTimeRange`
Retrieves events within a specified time range for a given asset.

### `GET /getPagedEventByAssetAndTimeRange`
Retrieves events within a specified time range for a given asset, with pagination.

### `GET /getEventByAssetAndTrip`
Retrieves events associated with a specific asset and trip.

### `GET /getLatestEvents`
Retrieves the latest events for all assets. This endpoint does not require any query parameters.
