#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM dtr-acc.sebank.se/operationslinux/dotnet-core-aspnet:3.1 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENV http_proxy=http://gia.sebank.se:8080
ENV https_proxy=http://gia.sebank.se:8080
ENV no_proxy=".sebank.se"

FROM dtr-acc.sebank.se/operationslinux/dotnet-core-sdk:3.1 AS build
WORKDIR /src
COPY ["Microservices/XB.Astrea.WebAPI/NuGet.Config", ""]
COPY ["Microservices/XB.Astrea.WebAPI/XB.Astrea.WebAPI.csproj", "Microservices/XB.Astrea.WebAPI/"]
COPY ["Dependencies/XB.IBM.MQ/XB.IBM.MQ.csproj", "Dependencies/XB.IBM.MQ/"]
RUN dotnet restore "Microservices/XB.Astrea.WebAPI/XB.Astrea.WebAPI.csproj" --configfile NuGet.Config
COPY . .
WORKDIR "/src/Microservices/XB.Astrea.WebAPI"
RUN dotnet build "XB.Astrea.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XB.Astrea.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

# final stage/image - the runtime image
RUN curl -L http://corpintra.sebank.se/Corpcontent/dominotargetinsite.nsf/siteIDLookup/IT~07CBF8FCD9D922A7C1258450003ACD43/%24FILE/SEB%20Root%20CA%20v2%20PEM.cer > /usr/local/share/ca-certificates/SEBRootCAv2.crt
# The cert linked below seems to be not fully ok. Linux complains when updating ca certs if using this. I have mentioned this to the sebkem people, hopefully the fix it. Using the old link to the binary cert meanwhile.
#RUN curl -L http://corpintra.sebank.se/Corpcontent/dominotargetinsite.nsf/siteIDLookup/IT~07CBF8FCD9D922A7C1258450003ACD43/%24FILE/SEB%20Appl%20Sub%20CA%20v2%20PEM.cer > /usr/local/share/ca-certificates/SEBApplSubCAv2.crt
RUN curl -L http://corpintra.sebank.se/Corpcontent/dominotargetinsite.nsf/siteIDLookup/IT~07CBF8FCD9D922A7C1258450003ACD43/%24FILE/SEB%20Appl%20Sub%20CA%20v2.cer | openssl x509 -inform der -outform PEM > /usr/local/share/ca-certificates/SEBApplSubCAv2.crt
RUN curl -L http://corpintra.sebank.se/Corpcontent/dominotargetinsite.nsf/siteIDLookup/IT~07CBF8FCD9D922A7C1258450003ACD43/%24FILE/SEB%20Test%20Root%20CA%20v2%20PEM.cer > /usr/local/share/ca-certificates/SEBTestRootCAv2.crt
RUN curl -L http://corpintra.sebank.se/Corpcontent/dominotargetinsite.nsf/siteIDLookup/IT~07CBF8FCD9D922A7C1258450003ACD43/%24FILE/SEB%20Test%20Appl%20Sub%20CA%20v2%20PEM.cer > /usr/local/share/ca-certificates/SEBTestApplSubCAv2.crt
RUN update-ca-certificates

RUN groupadd gpp

RUN mkdir -p runtime-assets/certs

#Cert is copied as a secret in the compose file, but if testing locally with another cert, you can copy it here. For mq, make sure to create & use the same user further above as in the cert friendly name...probably it is used to match which cert is loaded
COPY LocalCert/key.p12 runtime-assets/certs/key.p12

RUN useradd -ms /bin/bash d827720345
RUN usermod -a -G gpp d827720345

#RUN chown -R :eventbridge runtime-assets/certs/
RUN chown -R d827720345:gpp ./
RUN chmod -R g+w runtime-assets

RUN apt-get update && apt-get install -y telnet iputils-ping nano

USER d827720345

ENV XMS_TRACE_ON 2

ENTRYPOINT ["dotnet", "XB.Astrea.WebAPI.dll"]