#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# build with: docker build -t ghcr.io/muscle-up-app/carnets:muscleUpDev -f Carnets/Carnets.API/Dockerfile .
# push with: docker push ghcr.io/muscle-up-app/carnets:muscleUpDev

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Carnets/Carnets.API/Carnets.API.csproj", "Carnets/Carnets.API/"]
COPY ["Carnets/Carnets.Repo/Carnets.Infrastructure.csproj", "Carnets/Carnets.Repo/"]
COPY ["Carnets/Carnets.Domain/Carnets.Domain.csproj", "Carnets/Carnets.Domain/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
RUN dotnet restore "Carnets/Carnets.API/Carnets.API.csproj"
COPY . .
WORKDIR "/src/Carnets/Carnets.API"
RUN dotnet build "Carnets.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Carnets.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carnets.API.dll"]