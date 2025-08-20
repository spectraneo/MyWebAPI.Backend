# MyWebAPI - ASP.NET Core Web API with Azure Container Apps

This project demonstrates how to build and deploy an ASP.NET Core Web API using Azure Container Apps. It serves as a learning resource for containerizing .NET applications and deploying them to Azure's serverless container platform.

## 🏗️ Project Overview

This is a simple ASP.NET Core 8.0 Web API that includes:
- Weather forecast API endpoints
- Swagger/OpenAPI documentation
- Docker containerization
- Ready for Azure Container Apps deployment

## 🚀 Features

- **ASP.NET Core 8.0** - Latest .NET framework
- **Swagger UI** - Interactive API documentation at root path
- **Docker Support** - Multi-stage Dockerfile for optimized container builds
- **Weather Forecast API** - Sample controller with GET endpoint
- **Azure Container Apps Ready** - Configured for cloud deployment

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🛠️ Local Development

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MyWebAPI.Backend
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   cd MyWebAPI
   dotnet run
   ```

4. **Access the API**
   - Navigate to `https://localhost:7xxx` (HTTPS) or `http://localhost:5xxx` (HTTP)
   - Swagger UI will be available at the root path
   - API endpoint: `/WeatherForecast`

### Running with Docker

1. **Build the Docker image**
   ```bash
   docker build -t mywebapi .
   ```

2. **Run the container**
   ```bash
   docker run -p 8080:8080 mywebapi
   ```

3. **Access the API**
   - Navigate to `http://localhost:8080`

## ☁️ Azure Container Apps Deployment

### Step 1: Prepare Azure Resources

1. **Login to Azure**
   ```bash
   az login
   ```

2. **Create a resource group**
   ```bash
   az group create --name mywebapi-rg --location eastus
   ```

3. **Create a Container Apps environment**
   ```bash
   az containerapp env create \
     --name mywebapi-env \
     --resource-group mywebapi-rg \
     --location eastus
   ```

### Step 2: Build and Push Container Image

#### Option A: Using Azure Container Registry (ACR)

1. **Create an Azure Container Registry**
   ```bash
   az acr create --resource-group mywebapi-rg --name mywebapiregistry --sku Basic --admin-enabled true
   ```

2. **Login to ACR**
   ```bash
   az acr login --name mywebapiregistry
   ```

3. **Build and push image**
   ```bash
   # Build and push directly to ACR
   az acr build --registry mywebapiregistry --image mywebapi:latest .
   ```

#### Option B: Using Docker Hub

1. **Tag the image**
   ```bash
   docker tag mywebapi your-dockerhub-username/mywebapi:latest
   ```

2. **Push to Docker Hub**
   ```bash
   docker push your-dockerhub-username/mywebapi:latest
   ```

### Step 3: Deploy to Container Apps

#### Using Azure CLI

```bash
az containerapp create \
  --name mywebapi-app \
  --resource-group mywebapi-rg \
  --environment mywebapi-env \
  --image mywebapiregistry.azurecr.io/mywebapi:latest \
  --target-port 8080 \
  --ingress 'external' \
  --registry-server mywebapiregistry.azurecr.io \
  --query properties.configuration.ingress.fqdn
```

#### Using Azure Portal

1. Navigate to Azure Portal
2. Search for "Container Apps"
3. Click "Create"
4. Fill in the required information:
   - **Subscription**: Your Azure subscription
   - **Resource Group**: mywebapi-rg
   - **Container App Name**: mywebapi-app
   - **Region**: East US
   - **Container Apps Environment**: mywebapi-env
5. Configure the container:
   - **Image source**: Azure Container Registry or Docker Hub
   - **Image**: Your pushed image
   - **CPU**: 0.25
   - **Memory**: 0.5Gi
6. Configure ingress:
   - **Ingress**: Enabled
   - **Traffic**: 100% to latest revision
   - **Target port**: 8080
7. Review and create

## 🔧 Configuration

### Environment Variables

The application supports the following environment variables:

- `ASPNETCORE_ENVIRONMENT` - Set to `Production` for production deployment
- `ASPNETCORE_URLS` - Configure binding URLs (default: `http://+:8080`)

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## 📊 Monitoring and Scaling

### Application Insights

To add Application Insights monitoring:

1. **Install the NuGet package**
   ```bash
   dotnet add package Microsoft.ApplicationInsights.AspNetCore
   ```

2. **Configure in Program.cs**
   ```csharp
   builder.Services.AddApplicationInsightsTelemetry();
   ```

### Auto-scaling

Container Apps automatically scales based on:
- HTTP traffic
- CPU usage
- Memory usage
- Custom metrics

Configure scaling rules in the Azure Portal or via Azure CLI.

## 🐛 Troubleshooting

### Common Issues

1. **Container fails to start**
   - Check Docker build logs
   - Verify EXPOSE port in Dockerfile matches target port
   - Ensure application listens on `0.0.0.0:8080`

2. **Application not accessible**
   - Verify ingress is enabled
   - Check target port configuration
   - Ensure security groups allow traffic

3. **Registry authentication issues**
   - Verify ACR admin credentials
   - Check registry server URL format

### Debugging

View logs using Azure CLI:
```bash
az containerapp logs show --name mywebapi-app --resource-group mywebapi-rg
```

## 📚 Learning Resources

- [Azure Container Apps Documentation](https://docs.microsoft.com/en-us/azure/container-apps/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Docker Documentation](https://docs.docker.com/)
- [Azure Container Registry](https://docs.microsoft.com/en-us/azure/container-registry/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test locally and with Docker
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🔗 Useful Commands

### Docker Commands
```bash
# Build image
docker build -t mywebapi .

# Run container
docker run -p 8080:8080 mywebapi

# View running containers
docker ps

# View logs
docker logs <container-id>
```

### Azure CLI Commands
```bash
# List Container Apps
az containerapp list --resource-group mywebapi-rg

# Show app details
az containerapp show --name mywebapi-app --resource-group mywebapi-rg

# Update app
az containerapp update --name mywebapi-app --resource-group mywebapi-rg --image newimage:tag

# Delete app
az containerapp delete --name mywebapi-app --resource-group mywebapi-rg
```

---

**Happy coding and deploying! 🚀**
