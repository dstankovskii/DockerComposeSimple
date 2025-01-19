# Contracts

The Contracts project contains shared gRPC definitions and data transfer objects used by all services.

## Features
- gRPC `.proto` files for service definitions.
- Auto-generated gRPC client and server stubs (conditionally generated).
- Shared DTOs for consistent data exchange between services.

## Structure
- **Protos**: Original `.proto` files defining gRPC services.
- **Generated**: Auto-generated C# code for gRPC communication.

## Conditional Proto Generation
To improve build performance, the generation of gRPC stubs is disabled by default. You can enable it manually when needed.

To generate gRPC stubs:
```bash
dotnet build /p:GenerateProtos=true
```

This ensures that only explicitly requested builds generate the necessary files.

### Adding a New gRPC Service
1. Add the `.proto` file to the `Protos` directory.
2. Update the project file (`Contracts.csproj`) to include the new `.proto` with conditional generation:
   ```xml
   <ItemGroup Condition="'$(GenerateProtos)' == 'true'">
       <Protobuf Include="Protos/your-new-service.proto" GrpcServices="Both" />
   </ItemGroup>
   ```
3. Use the build command with `GenerateProtos` to generate the stubs.

## Usage
Include this project as a dependency in other services to share contracts and gRPC definitions.