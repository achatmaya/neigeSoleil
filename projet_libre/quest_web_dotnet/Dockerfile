# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore "quest_web_dotnet/quest_web.csproj"
RUN dotnet publish "quest_web_dotnet/quest_web.csproj" -c Release -o /app/publish

RUN ls /app/publish  

# Étape de runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN apt-get update && \
    apt-get install -y tzdata

WORKDIR /app
COPY --from=build /app/publish .

RUN ls /app  # Vérifiez le contenu du répertoire après la copie

 ENTRYPOINT ["sh", "-c", "dotnet quest_web.dll"]
