FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY MorningbrewCrawler.sln ./
COPY ./src/Hangfire.Runner/*.csproj Hangfire.Runner/
COPY ./src/Crawler/*.csproj Crawler/
RUN dotnet restore Hangfire.Runner/Hangfire.Runner.csproj

# Copy everything else and build
COPY ./Directory.Build.props ./
COPY ./src/. ./
RUN dotnet publish Hangfire.Runner/Hangfire.Runner.csproj -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/Hangfire.Runner/out .
ENTRYPOINT ["dotnet", "Hangfire.Runner.dll"]