#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Microservices/XB.Astrea.WebAPI/XB.Astrea.WebAPI.csproj", "Microservices/XB.Astrea.WebAPI/"]
COPY ["Dependencies/XB.IBM.MQ/XB.IBM.MQ.csproj", "Dependencies/XB.IBM.MQ/"]
RUN dotnet restore "Microservices/XB.Astrea.WebAPI/XB.Astrea.WebAPI.csproj"
COPY . .
WORKDIR "/src/Microservices/XB.Astrea.WebAPI"
RUN dotnet build "XB.Astrea.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XB.Astrea.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XB.Astrea.WebAPI.dll"]