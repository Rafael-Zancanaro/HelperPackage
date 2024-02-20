# HelperPackage
package for help on APIs.

# PackageRZ - README

## Installation

To use the PackageRZ in your C# project, follow the steps below:

1. **Installation via NuGet Package Manager Console:**
   Execute the following command in the NuGet Package Manager Console:

   ```bash
   Install-Package PackageRZ
   ```

2. **Installation via Visual Studio Package Manager:**
   Open the Visual Studio Package Manager, go to the "Browse" tab, and search for "PackageRZ". Select the package from the list and click "Install".

## ControllerMain and Custom Response

PackageRZ provides a custom controller called `ControllerMain` that offers a personalized response through the `Response` class. 
This class allows you to automatically return either an OK status or a BadRequest status with a custom message.

PackageRZ provides a custom controller called ControllerMain that expects a method named "Response" as the default response. 
This method provides an OK or BadRequest response with a standardized response model containing properties for Success, Errors, and Data.
Example usage:

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

    [HttpGet("indetifier")]
    [ProducesResponseType(typeof(ResultViewModel<YourOutputModel>), (short)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
        => Response(await _service.DoAnythingAsync());
}
```

In your service, you should return a type of `ResultViewModel<YourOutputModel>` to ensure a fully standardized response.

```csharp
public async Task<ResultViewModel<YourOutputModel>> DoAnythingAsync()
{
    var result = new ResultViewModel<YourOutputModel>();

    if (!AnyValidation)
        return result.AddError("Please provide the error message here.");

    var output = new YourOutputModel();

    return result.AddResult(output);
}
```

```markdown
[ControllerMain](https://github.com/Rafael-Zancanaro/HelperPackage/blob/main/PackageRZ/Controllers/ControllerMain.cs)
[ResulViewModel](https://github.com/Rafael-Zancanaro/HelperPackage/blob/main/PackageRZ/Domain/ViewModels/ResultViewModel.cs)
```

## Use of Repository Pattern

PackageRZ includes a repository pattern that can be used in your C# project. To use it, inherit from the ` BaseRepository<T, TPK>` class in your specific repository.
You should pass your entity in place of 'T' and the type of your primary key in place of 'TPK'.
To inject the context, simply create a construction and pass the context to the base.

Example:
```csharp
public class MyRepository : BaseRepository<T, TPK>, IMyRepository
{
    public class MyRepository(YourContext context) : Base(context)
    {
    }

    // Implement specific methods if necessary
}
```

And you will also have access to the following properties.
![Example Code](D:\Dowloads\image.png)

In your repository interface, you also need to inherit from the IBaseRepository interface, and now you will have access to ready-to-use methods in your application.
Here is the link to the code:
```markdown
[BaseRepository](https://github.com/Rafael-Zancanaro/HelperPackage/blob/main/PackageRZ/Repositories/BaseRepository.cs)
```

## Use of Model Entity

This package also features a generic base entity model, allowing you to choose its type.
To use in your entity, simply inherit from `BaseEntity<TypePrimaryKey` and provide your type.

```csharp
public class Entity : BaseEntity<TypePrimeryKey>
    {
        public string Name { get; private set; }
        public string DateOfBirth { get; private set; }

        public Entity()
        {
        }
    }
```
```markdown
[BaseEntity](https://github.com/Rafael-Zancanaro/HelperPackage/blob/main/PackageRZ/Domain/Entities/BaseEntity.cs)
```

## Extension for Swagger with JWT Token

PackageRZ includes an extension for Swagger that adds a custom button for JWT token authentication. 
To use this extension, call the `SetSwaggerGenAuthButton` method in the `SwaggerGenOptions` method in the `AddSwaggerGen` method in the Program.cs ou Startup.cs 
file:

```csharp
services.AddSwaggerGen(config =>
{
    config.SetSwaggerGenAuthButton();
});
```
```markdown
[SwaggerExtention](https://github.com/Rafael-Zancanaro/HelperPackage/blob/main/PackageRZ/Utils/SwaggerConfig.cs)
```
