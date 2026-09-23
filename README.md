# PackageRZ
A comprehensive .NET 10 helper package providing standardized error handling, resilient HTTP calls with Polly, structured logging, base repositories, and more.

## Installation

To use PackageRZ in your C# project, follow the steps below:

1. **Installation via NuGet Package Manager Console:**
   Execute the following command in the NuGet Package Manager Console:
   ```bash
   Install-Package PackageRZ
   ```

2. **Installation via Visual Studio Package Manager:**
   Open the Visual Studio Package Manager, go to the "Browse" tab, and search for "PackageRZ". Select the package from the list and click "Install".

---

## Global Error Handling & Standardization

PackageRZ standardizes how APIs respond to errors using a consistent `ResultViewModel<T>` and `FailureResult`.

### ControllerMain
PackageRZ provides an abstract `ControllerMain` that exposes a `Response<T>(ResultViewModel<T> result)` method. It automatically returns a `200 OK` or `400 BadRequest` (or other statuses) based on the `Success` flag.

```csharp
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerMain
{
    private readonly IService _service;

    public MyController(IService service)
    {
        _service = service;
    }

    [HttpGet("identifier")]
    [ProducesResponseType(typeof(ResultViewModel<YourOutputModel>), (short)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
        => Response(await _service.DoAnythingAsync());
}
```

### BaseService
In your services, inherit from `BaseService` to easily return success or failure states without throwing exceptions:

```csharp
public class MyService : BaseService, IService
{
    public async Task<ResultViewModel<YourOutputModel>> DoAnythingAsync()
    {
        if (validationFails)
            return AddErrors("Please provide the error message here."); // Returns a FailureResult implicitly converted to ResultViewModel

        var output = new YourOutputModel();
        return AddResult(output);
    }
}
```

### Global Exceptions & Handlers
The package relies on the `.NET 8+` `IExceptionHandler` middleware pattern to gracefully trap exceptions and return standardized models. It includes:
- `BaseExceptionHandler<TException>`: A base class to create your own typed handlers.
- `HttpIntegrationExceptionHandler`: Traps `HttpIntegrationException` and returns a standard internal error (without leaking response bodies).
- `ErrorController`: A global fallback controller that writes standard localized errors for unhandled exceptions.

---

## Resilient HTTP Dependencies (Polly & Circuit Breakers)

PackageRZ incorporates **Polly** to provide resilience for your external HTTP dependencies.

To register an `HttpClient` with our standardized Circuit Breaker and Retry policies, use the `PolicyExtensions`:

```csharp
using PackageRZ.Utils;

// In Program.cs
builder.Services.AddHttpClient<IMyExternalClient, MyExternalClient>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
})
.AddPolicyHandler(PolicyExtensions.GetRetryPolicy(retryCount: 3))
.AddHelperPolicyHandler(PolicyExtensions.GetCircuitBreakerPolicy(
    handledEventsAllowedBeforeBreaking: 3, 
    durationOfBreakSeconds: TimeSpan.FromSeconds(30)));
```

`AddHelperPolicyHandler` automatically injects the `HttpCircuitBreakerDelegatingHandler`, which intercepts Polly's `BrokenCircuitException` and rethrows it as a strongly-typed `HttpIntegrationException` containing the blocked request context.

---

## FluentValidation Integration

You can easily register FluentValidation to automatically validate your request models and return the standard `ResultViewModel` on failures. 

In your `Program.cs`, add:

```csharp
using PackageRZ.Utils;

builder.Services.AddFluentValidation<AssemblyMarkerType>(stopOnFirstFailure: false);
```
This registers the `ValidationFilter` action filter, which intercepts invalid requests and short-circuits the pipeline with a standardized `400 BadRequest` containing your FluentValidation error messages.

---

## Structured Logging (Zero Allocation)

PackageRZ includes high-performance structured logging extensions using `LoggerMessage` source generators. They avoid memory allocation when log levels are disabled.

```csharp
using PackageRZ.Logger;

// In your class
_logger.Error(exception, eventId: 100, message: "Custom message", details: "More info", origin: "MyClass");

_logger.Warning(eventId: 101, message: "Just a warning", origin: "MyClass");

// Specialized for HTTP Tracking (automatically formats status, method, url, parameters, etc.)
await _logger.HttpErrorAsync(eventId: 200, httpResponseMessage, origin: "DependencyClient");
```

---

## Repository Pattern

PackageRZ includes a base repository pattern that you can use in your project.

```csharp
public class MyRepository : BaseRepository<MyEntity, Guid>, IMyRepository
{
    public MyRepository(YourContext context) : base(context)
    {
    }
}
```
Inherit `IBaseRepository<T, TPK>` in your interface to get standard CRUD async methods (`InsertAsync`, `UpdateAsync`, `DeleteAsync`, `FindByIdAsync`, `FindByFilterAsync`, `GetAllAsync`).

---

## Extension for Swagger with JWT Token

PackageRZ includes an extension for Swagger that adds a custom global Authorization button for JWT Bearer tokens. 

```csharp
using PackageRZ.Utils;

builder.Services.AddSwaggerGen(config =>
{
    config.SetSwaggerGenAuthButton();
});
```
