# Multi-stage build: build with SDK, run with ASP.NET runtime

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 5000
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "InterestApi.dll"]
