#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
RUN apt update -y && apt install screenfetch openssl -y
WORKDIR /app
COPY 'certificados-digitais' ./'certificados-digitais'

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["Siscomex.Core/Siscomex.Core.csproj", "Siscomex.Core/"]
COPY ["ConsultaPLI/ConsultaPLI.Core.Lib/ConsultaPLI.Core.Lib.csproj", "ConsultaPLI/ConsultaPLI.Core.Lib/"]
COPY ["Shared/SimpleBrowser.Core/SimpleBrowser.Core.csproj", "Shared/SimpleBrowser.Core/"]
COPY ["Shared/Siscomex.Core.Shared/Siscomex.Core.Shared.csproj", "Shared/Siscomex.Core.Shared/"]
RUN dotnet restore "Siscomex.Core/Siscomex.Core.csproj"
COPY . .
WORKDIR "/src/Siscomex.Core"
RUN dotnet build "Siscomex.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Siscomex.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV DOWNLOAD=/app/siscomex-download
ENV UPLOAD=/app/siscomex-upload
ENV CERT=/app/certificados-digitais/yamaha.pfx
RUN mkdir ${DOWNLOAD} -p
RUN mkdir ${UPLOAD} -p
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Siscomex.Core.dll"]