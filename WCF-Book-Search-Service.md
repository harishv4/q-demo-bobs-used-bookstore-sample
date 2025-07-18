# WCF Book Search Service Architecture

## Overview
The WCF Book Search Service is a modernization example that extracts the book search functionality from the monolithic Bob's Used Books application into a separate service. This demonstrates a common modernization pattern of incrementally refactoring a monolith into services.

## Architecture Components

### Service Layer
- **Bookstore.WcfServices**: Contains the service contract, data contracts, and service implementation
  - `IBookSearchService`: Service contract defining the operations
  - `BookSearchService`: Implementation of the service contract
  - Data contracts (DTOs) for transferring data between service and client

### Host Application
- **Bookstore.WcfHost**: ASP.NET Core application that hosts the WCF service
  - Uses CoreWCF for .NET 6.0 compatibility
  - Configures service endpoints and behaviors
  - Manages dependency injection for service dependencies

### Client Integration
- **Web Application**: Modified to conditionally use the WCF service
  - `WcfClientFactory`: Creates WCF client proxies
  - `BookServiceWcfAdapter`: Adapter that implements `IBookService` but calls the WCF service
  - Configuration toggle to enable/disable WCF service usage

## Communication Flow
1. Web application checks if WCF service is enabled in configuration
2. If enabled, requests are routed through the WCF client adapter
3. WCF client adapter calls the WCF service using generated proxies
4. WCF service processes the request and returns data
5. Adapter maps the returned DTOs back to domain entities

## Data Transfer
- Data is transferred using Data Transfer Objects (DTOs)
- DTOs are serialized/deserialized automatically by WCF
- Navigation properties are manually reconstructed on the client side

## Configuration
- WCF service endpoint: http://localhost:8080/BookSearchService
- Toggle in web app: `"WcfService:Enabled": true/false` in appsettings.Development.json

## Benefits
- **Modularity**: Separates book search functionality into an independent service
- **Scalability**: Service can be scaled independently of the web application
- **Technology Evolution**: Demonstrates modernization without rewriting the entire application
- **Deployment Flexibility**: Service can be deployed separately from the web application

## Future Considerations
- Migrate to more modern service technologies (gRPC, REST APIs)
- Add authentication and authorization to the service
- Implement caching for improved performance
- Deploy as containerized service in cloud environment