FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./src/SqlServer.Migration/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./src/SqlServer.Migration/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SqlServer.Migration.dll"]