#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ServicePenyewaan.API/ServicePenyewaan.API.csproj", "ServicePenyewaan.API/"]
RUN dotnet restore "ServicePenyewaan.API/ServicePenyewaan.API.csproj"
COPY . .
WORKDIR "/src/ServicePenyewaan.API"
RUN dotnet build "ServicePenyewaan.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServicePenyewaan.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServicePenyewaan.API.dll"]