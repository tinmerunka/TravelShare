FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY TravelShare/*.csproj ./TravelShare/
RUN dotnet restore ./TravelShare/TravelShare.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./TravelShare/TravelShare.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port
ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "TravelShare.dll"]
