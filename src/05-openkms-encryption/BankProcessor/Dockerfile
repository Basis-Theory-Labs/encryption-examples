#syntax=docker/dockerfile:1.2

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS builder

WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish BankProcessor/BankProcessor.csproj -c Release -o publish --no-restore

########################################

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

RUN apk update && apk upgrade && apk upgrade busybox --repository=http://dl-cdn.alpinelinux.org/alpine/edge/main

RUN apk add --no-cache icu-libs tzdata

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=builder /app/publish .
EXPOSE 80

ENTRYPOINT ["dotnet", "BankProcessor.dll"]
