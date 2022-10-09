#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# build with: docker build -t ghcr.io/muscle-up-app/apigateway:muscleUpDev -f ApiGateway/ApiGateway.API/Dockerfile .
# push with: docker push ghcr.io/muscle-up-app/apigateway:muscleUpDev

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateway/ApiGateway.API/ApiGateway.API.csproj", "ApiGateway/ApiGateway.API/"]
RUN dotnet restore "ApiGateway/ApiGateway.API/ApiGateway.API.csproj"
COPY . .
WORKDIR "/src/ApiGateway/ApiGateway.API"
RUN dotnet build "ApiGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/etc/ssl/aspnetapp.pfx
ENTRYPOINT ["dotnet", "ApiGateway.API.dll"]

COPY ./ApiGateway/ApiGateway.API/SSL/aspnetapp.pfx /etc/ssl/