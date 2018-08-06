FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./src/SqlServer.Migration.Fluent/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./src/SqlServer.Migration.Fluent/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SqlServer.Migration.Fluent.dll"]