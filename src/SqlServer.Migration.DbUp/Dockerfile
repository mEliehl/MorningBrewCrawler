FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./src/SqlServer.Migration.DbUp/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./src/SqlServer.Migration.DbUp/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SqlServer.Migration.DbUp.dll"]