# .NET 8 WebAPI with DynamoDB Integration

This repository contains a .NET 8 WebAPI project that demonstrates the integration with DynamoDB for managing GPS data of simulated vehicles. The API supports operations such as querying events based on asset and time range, retrieving single events by ID, fetching the latest events for all assets, and events for a specific asset and trip.

## Features

- **Query Support**: Advanced queries based on asset and time.
- **Docker Integration**: Runs in a Docker container for easy deployment and scalability.
- **Local DynamoDB Instance**: Utilizes a local DynamoDB instance for development without the need for an AWS account.

## API Endpoints

Each endpoint's function and required query parameters are described below:

### `GET /getEventById`
- **id** (string): The unique identifier for the event you wish to retrieve.

### `GET /getEventByAssetAndTimeRange`
Retrieves events within a specified time range for a given asset.
- **Asset** (int): The asset's identifier.
- **StartDateTime** (string): The start date and time of the range in ISO 8601 format.
- **EndDateTime** (string): The end date and time of the range in ISO 8601 format.

### `GET /getPagedEventByAssetAndTimeRange`
Retrieves events within a specified time range for a given asset, with pagination.
- **Asset** (int): The asset's identifier.
- **StartDateTime** (string): The start date and time of the range in ISO 8601 format.
- **EndDateTime** (string): The end date and time of the range in ISO 8601 format.
- **Limit** (int, optional): The maximum number of events to return per page (default is 25, max 1000).
- **nextToken** (string, optional): A token to retrieve the next page of results.

### `GET /getEventByAssetAndTrip`
Retrieves events associated with a specific asset and trip.
- **Asset** (int): The asset's identifier.
- **Trip** (int): The trip's identifier.

### `GET /getLatestEvents`
Retrieves the latest events for all assets. This endpoint does not require any query parameters.

## Parameter Details
- **Asset** and **Trip** parameters should be non-negative integers.
- **StartDateTime** and **EndDateTime** should be valid ISO 8601 date strings.
- **Limit** should be between 1 and 1000, inclusive.


## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - if building/running the project locally.
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Getting Started

Clone the repository to your local machine:

```bash
git clone https://github.com/psprateek7/PhocasAssetApi.git
cd yourprojectname
