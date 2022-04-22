FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["LeagueActivityBot/LeagueActivityBot.csproj", "LeagueActivityBot/"]
RUN dotnet restore "LeagueActivityBot/LeagueActivityBot.csproj"
COPY . .
WORKDIR "/src/LeagueActivityBot"
RUN dotnet build "LeagueActivityBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeagueActivityBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeagueActivityBot.dll"]
