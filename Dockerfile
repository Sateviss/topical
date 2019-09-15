FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY Application/*.csproj ./Application/
WORKDIR /app/Application
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY Application/. ./Application/
WORKDIR /app/Application
RUN dotnet publish -c Release -o out


# test application -- see: dotnet-docker-unit-testing.md
FROM build AS testrunner
WORKDIR /app/tests
COPY Tests/. .
ENTRYPOINT ["dotnet", "test", "--logger:trx"]


FROM mcr.microsoft.com/dotnet/core/runtime:2.1-alpine AS runtime
WORKDIR /app
COPY --from=build /app/Application/out ./
ENTRYPOINT ["dotnet", "Application.dll"]

