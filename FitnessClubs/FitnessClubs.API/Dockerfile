#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# build with: docker build -t ghcr.io/muscle-up-app/fitnessclubs:muscleUpDev -f FitnessClubs/FitnessClubs.API/Dockerfile .
# push with: docker push ghcr.io/muscle-up-app/fitnessclubs:muscleUpDev

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FitnessClubs/FitnessClubs.API/FitnessClubs.API.csproj", "FitnessClubs/FitnessClubs.API/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
COPY ["FitnessClubs/FitnessClubs.Repo/FitnessClubs.Infrastructure.csproj", "FitnessClubs/FitnessClubs.Repo/"]
COPY ["FitnessClubs/FitnessClubs.Domain/FitnessClubs.Domain.csproj", "FitnessClubs/FitnessClubs.Domain/"]
RUN dotnet restore "FitnessClubs/FitnessClubs.API/FitnessClubs.API.csproj"
COPY . .
WORKDIR "/src/FitnessClubs/FitnessClubs.API"
RUN dotnet build "FitnessClubs.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FitnessClubs.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitnessClubs.API.dll"]