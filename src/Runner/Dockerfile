FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY MorningbrewCrawler.sln ./
COPY ./src/Runner/*.csproj Runner/
COPY ./src/Crawler/*.csproj Crawler/
RUN dotnet restore Runner/Runner.csproj

# Copy everything else and build
COPY ./Directory.Build.props ./
COPY ./src/. ./
RUN dotnet publish Runner/Runner.csproj -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/Runner/out .
ENTRYPOINT ["dotnet", "Runner.dll"]