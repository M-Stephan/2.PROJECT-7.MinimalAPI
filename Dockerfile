FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY wait-for.sh /wait-for.sh
RUN chmod +x /wait-for.sh

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY wait-for.sh /wait-for.sh
RUN chmod +x /wait-for.sh

# Installer netcat pour que wait-for.sh puisse fonctionner
RUN apt-get update && apt-get install -y netcat-openbsd

ENTRYPOINT ["/wait-for.sh", "db", "dotnet", "7.MinimalAPI.dll"]

