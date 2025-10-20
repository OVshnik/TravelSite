FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /TravelSite

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c release -o /TravelSite

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /TravelSite ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "TravelSite.dll"]