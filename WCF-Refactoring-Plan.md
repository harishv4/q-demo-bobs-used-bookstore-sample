# Plan for Refactoring Book Search Service to WCF

## Overview
This document outlines the approach to refactor the Book Search functionality in Bob's Used Books application to use Windows Communication Foundation (WCF) services. The goal is to extract the book search capabilities into a separate WCF service while maintaining the existing functionality.

## Current Architecture
- The application uses a monolithic n-tier architecture with ASP.NET Core 6.0
- Book search functionality is implemented in `BookService` class in the Domain layer
- The service is directly injected into controllers using dependency injection

## Target Architecture
- Extract book search functionality into a separate WCF service
- Main web application will become a client of the WCF service
- Maintain all existing functionality and user experience

## Implementation Steps

### 1. Create WCF Service Library Project
- Add new project `Bookstore.WcfServices` to the solution
- Add required WCF NuGet packages:
  - System.ServiceModel.Primitives
  - System.ServiceModel.Http
  - System.ServiceModel.NetTcp
  - System.Private.ServiceModel

### 2. Define WCF Service Contract
- Create `IBookSearchService.cs` with WCF service contract:
```csharp
[ServiceContract]
public interface IBookSearchService
{
    [OperationContract]
    Task<BookDto> GetBookAsync(int id);
    
    [OperationContract]
    Task<PaginatedListDto<BookDto>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize);
    
    [OperationContract]
    Task<PaginatedListDto<BookDto>> GetBooksByFiltersAsync(BookFiltersDto filters, int pageIndex, int pageSize);
    
    [OperationContract]
    Task<IEnumerable<BookDto>> ListBestSellingBooksAsync(int count);
    
    [OperationContract]
    Task<BookStatisticsDto> GetStatisticsAsync();
}
```

### 3. Create Data Transfer Objects
- Create DTOs for all entities that need to be transferred over WCF:
  - BookDto
  - BookFiltersDto
  - PaginatedListDto<T>
  - BookStatisticsDto

### 4. Implement WCF Service
- Create `BookSearchService.cs` implementing the contract
- Inject the existing `IBookService` into the WCF service
- Map between domain entities and DTOs

### 5. Create WCF Host Project
- Add new console application project `Bookstore.WcfHost`
- Configure service hosting and endpoints
- Set up dependency injection for required services
- Configure database connection

### 6. Modify Web Application
- Create a WCF client proxy in the web application
- Create an adapter that implements `IBookService` but calls the WCF service
- Update DI configuration to use the adapter instead of the direct implementation

### 7. Update Configuration
- Add WCF service URL to application settings
- Configure service behavior and bindings
- Set up error handling and logging

### 8. Testing
- Unit tests for the WCF service
- Integration tests for the web application with WCF service
- End-to-end tests for the complete flow

## Deployment Considerations
- WCF service can be hosted in IIS or as a Windows Service
- Configure proper security settings (authentication, transport security)
- Set up monitoring and logging
- Consider high availability and load balancing options

## Risks and Mitigations
- **Risk**: Performance impact from network calls
  - **Mitigation**: Optimize DTO size, consider caching frequently used data
  
- **Risk**: Service availability issues
  - **Mitigation**: Implement circuit breaker pattern, fallback mechanisms
  
- **Risk**: Security concerns with exposing service
  - **Mitigation**: Use transport security, implement authentication

## Future Considerations
- Consider migrating to more modern service technologies (gRPC, REST APIs)
- Evaluate containerization options for the service
- Explore cloud-native deployment options on AWS