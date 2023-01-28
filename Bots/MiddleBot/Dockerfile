FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InternetMarket/InternetMarket.csproj", "InternetMarket/"]
RUN dotnet restore "InternetMarket/InternetMarket.csproj"
COPY . .
WORKDIR "/src/InternetMarket"
RUN dotnet build "InternetMarket.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InternetMarket.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ARG DB_SERVER
ENV ENV_DB_SERVER=$DB_SERVER
ARG DB_USER
ENV ENV_DB_USER=$DB_USER
ARG DB_PASSWORD
ENV ENV_DB_PASSWORD=$DB_PASSWORD
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InternetMarket.dll"]
