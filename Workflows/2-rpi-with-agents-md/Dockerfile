# Multi-stage build: build with SDK, run with ASP.NET runtime

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore
COPY back/InterestApi.csproj back/
RUN dotnet restore back/InterestApi.csproj

# Copy everything (including front) and place front into wwwroot
COPY . .
RUN mkdir -p back/wwwroot && cp -a front/. back/wwwroot/

# Publish the app
RUN dotnet publish back/InterestApi.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "InterestApi.dll"]
