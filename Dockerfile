# 1. Build stage: použijeme .NET SDK image k sestavení aplikace
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

# 2. Zkopíruj csproj a obnov závislosti (restore)
COPY *.csproj ./
RUN dotnet restore

# 3. Zkopíruj zbytek kódu
COPY . ./

# 4. Publish aplikaci do složky /app/out
RUN dotnet publish -c Release -o /app/out

# 5. Runtime stage: použijeme menší image pro spuštění aplikace
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

# 6. Zkopíruj publikované soubory z build stage
COPY --from=build /app/out ./

# 7. Otevři port (pokud používáš třeba 5000)
EXPOSE 5000

# 8. Spusť aplikaci
ENTRYPOINT ["dotnet", "MiniChatApp.dll"]
