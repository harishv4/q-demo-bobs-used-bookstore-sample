# Migration Plan: WCF Book Search Service to REST Web API

## Overview
This document outlines the plan to migrate the existing WCF Book Search Service to a modern ASP.NET Core REST Web API. This migration will improve interoperability, simplify the architecture, and enable better cloud-native deployment options.

## Migration Strategy
We'll use a parallel implementation approach where the REST API is built alongside the existing WCF service, allowing for a smooth transition with minimal disruption.

## Implementation Steps

### 1. Create REST API Project
- Add new project `Bookstore.Api` to the solution
- Configure as ASP.NET Core Web API (.NET 6.0)
- Add required package references:
  ```
  Microsoft.AspNetCore.Mvc.NewtonsoftJson
  Swashbuckle.AspNetCore
  ```

### 2. Define API Controllers
- Create `BooksController.cs` with REST endpoints that map to WCF operations:
  ```csharp
  [ApiController]
  [Route("api/[controller]")]
  public class BooksController : ControllerBase
  {
      [HttpGet("{id}")]
      public async Task<ActionResult<BookDto>> GetBook(int id);
      
      [HttpGet("search")]
      public async Task<ActionResult<PaginatedListDto<BookDto>>> SearchBooks([FromQuery] string searchString, [FromQuery] string sortBy, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10);
      
      [HttpPost("search/filters")]
      public async Task<ActionResult<PaginatedListDto<BookDto>>> SearchBooksByFilters([FromBody] BookFiltersDto filters, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10);
      
      [HttpGet("bestselling")]
      public async Task<ActionResult<IEnumerable<BookDto>>> GetBestSellingBooks([FromQuery] int count = 10);
      
      [HttpGet("statistics")]
      public async Task<ActionResult<BookStatisticsDto>> GetStatistics();
  }
  ```

### 3. Reuse Data Transfer Objects
- Copy DTOs from `Bookstore.WcfServices` to `Bookstore.Api.Models`
- Update attributes from `[DataContract]`/`[DataMember]` to `[JsonProperty]`
- Add validation attributes where appropriate

### 4. Implement Service Layer
- Create `BookService.cs` in the API project that reuses the core logic from the WCF service
- Inject the same dependencies (repositories, etc.)
- Ensure proper error handling and HTTP status codes

### 5. Configure API Host
- Set up dependency injection in `Program.cs`
- Configure JSON serialization settings
- Add Swagger documentation
- Configure CORS policies
- Set up API versioning

### 6. Create REST Client in Web Application
- Add `Bookstore.Api.Client` project or folder in the web app
- Implement `HttpClientFactory` pattern for API communication
- Create `RestBookServiceAdapter` implementing `IBookService` interface:
  ```csharp
  public class RestBookServiceAdapter : IBookService
  {
      private readonly HttpClient _httpClient;
      
      public RestBookServiceAdapter(HttpClient httpClient)
      {
          _httpClient = httpClient;
      }
      
      public async Task<Book> GetBookAsync(int id)
      {
          var response = await _httpClient.GetAsync($"api/books/{id}");
          response.EnsureSuccessStatusCode();
          var bookDto = await response.Content.ReadFromJsonAsync<BookDto>();
          return MapToEntity(bookDto);
      }
      
      // Implement other methods...
  }
  ```

### 7. Update Configuration
- Add REST API URL to application settings:
  ```json
  {
    "ApiService": {
      "BaseUrl": "https://localhost:5001/",
      "Enabled": true
    }
  }
  ```
- Modify DI setup to support both WCF and REST:
  ```csharp
  if (builder.Configuration.GetValue<bool>("ApiService:Enabled"))
  {
      // Register REST client
      builder.Services.AddHttpClient<IBookService, RestBookServiceAdapter>(client =>
      {
          client.BaseAddress = new Uri(builder.Configuration["ApiService:BaseUrl"]);
      });
  }
  else if (builder.Configuration.GetValue<bool>("WcfService:Enabled"))
  {
      // Register WCF client (existing code)
  }
  else
  {
      // Register direct implementation
      builder.Services.AddTransient<IBookService, BookService>();
  }
  ```

### 8. Testing
- Create unit tests for API controllers
- Create integration tests for API endpoints
- Test the web application with the REST client
- Verify feature parity with WCF implementation

### 9. Deployment
- Deploy the REST API as an Azure App Service or AWS Elastic Beanstalk
- Update the web application configuration to point to the deployed API
- Monitor performance and errors

### 10. Decommission WCF Service
- Once the REST API is stable and fully tested, remove WCF dependencies
- Update documentation to reflect the new architecture

## API Design Considerations

### RESTful Resource Mapping
- Books as primary resource (`/api/books`)
- Filtering via query parameters
- Use proper HTTP methods (GET, POST, PUT, DELETE)
- Return appropriate HTTP status codes

### Authentication & Authorization
- Implement JWT Bearer token authentication
- Define role-based authorization policies
- Secure sensitive endpoints

### Performance Optimization
- Implement response caching
- Consider pagination for large result sets
- Use compression for responses

## Benefits of REST Migration
- **Interoperability**: Works with any client that supports HTTP
- **Simplicity**: Easier to understand and consume than WCF
- **Cloud-Native**: Better support for containerization and serverless
- **Developer Experience**: Better tooling and client library support
- **Performance**: Potential for better performance with HTTP/2 and caching

## Timeline
- **Week 1**: Create API project and implement controllers
- **Week 2**: Implement client adapter and update web application
- **Week 3**: Testing and bug fixing
- **Week 4**: Deployment and monitoring