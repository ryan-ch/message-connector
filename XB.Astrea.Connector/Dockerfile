#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["XB.Astrea.Connector/XB.Astrea.Connector.csproj", "XB.Astrea.Connector/"]
COPY ["XB.IBM.MQ/XB.IBM.MQ.csproj", "XB.IBM.MQ/"]
COPY ["XB.Astrea/XB.Astrea.csproj", "XB.Astrea/"]
RUN dotnet restore "XB.Astrea.Connector/XB.Astrea.Connector.csproj"
COPY . .
WORKDIR "/src/XB.Astrea.Connector"
RUN dotnet build "XB.Astrea.Connector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XB.Astrea.Connector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XB.Astrea.Connector.dll"]