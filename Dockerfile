FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /api

# Copier les fichiers csproj d'abord (cache Docker)
COPY incedentAPI-RimaBouazra.csproj ./
COPY AppTests/AppTests.csproj AppTests/

# Récupérer les dépendances
RUN dotnet restore incedentAPI-RimaBouazra.csproj

# Copier tout le reste
COPY . .

# Publier uniquement l'API
RUN dotnet publish incedentAPI-RimaBouazra.csproj -c Release -o /app/publish

# Préparer l'environnement d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Forcer l'API à écouter sur le port 80
ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "incedentAPI-RimaBouazra.dll"]