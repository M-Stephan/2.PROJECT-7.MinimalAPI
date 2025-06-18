# Stage 1 : build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy .csproj and restore dependancies
COPY *.csproj ./
RUN dotnet restore


# Copy and compile other files in release
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2 : runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the API on port 5000
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Run app
ENTRYPOINT ["dotnet", "7.MinimalAPI.dll"]

