#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cars.Api/Cars.Api.csproj", "Cars.Api/"]
COPY ["BLL/LearnProject.BLL.Contracts/LearnProject.BLL.Contracts.csproj", "BLL/LearnProject.BLL.Contracts/"]
COPY ["LearnProject.Domain/LearnProject.Domain.csproj", "LearnProject.Domain/"]
COPY ["BLL/LearnProject.BLL.Services/LearnProject.BLL.Services.csproj", "BLL/LearnProject.BLL.Services/"]
COPY ["LearnProject.Shared/LearnProject.Shared.Common/LearnProject.Shared.Common.csproj", "LearnProject.Shared/LearnProject.Shared.Common/"]
COPY ["Infrastructure/Data/LearnProject.Data.DAL/LearnProject.Data.DAL.csproj", "Infrastructure/Data/LearnProject.Data.DAL/"]
COPY ["Infrastructure/Data/LearnProject.Data.MigrationService/LearnProject.Data.MigrationService.csproj", "Infrastructure/Data/LearnProject.Data.MigrationService/"]
RUN dotnet restore "./Cars.Api/./Cars.Api.csproj"
COPY . .
WORKDIR "/src/Cars.Api"
RUN dotnet build "./Cars.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cars.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cars.Api.dll"]