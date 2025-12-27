# Stage 1: Build (multi-stage)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code and build
COPY . ./
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Port 8080 (Azure App Service uses this port)
EXPOSE 8080

# Environment variable for the port
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RestApiProject.dll"]

CMD ["dotnet", "RestApiProject.dll"]
