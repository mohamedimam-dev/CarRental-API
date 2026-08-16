# 1. Base image for running the app
From mcr.microsoft.com/dotnet/aspnet:8.0 As base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. SDK image for building the app
From mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore dependencies
COPY ["CarRental.API.csproj", "./"]
RUN dotnet restore "CarRental.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src"
RUN dotnet build "CarRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarRental.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRental.API.dll"]