FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src
# RUN mkdir -p /usr/local/share/dotnet/sdk/NuGetFallbackFolder

COPY ./*.sln ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore
COPY ./ ./
RUN dotnet publish ./LeagueActivityBot.Host/ -o /publish -c Release --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build-env /publish .


ENTRYPOINT ["dotnet","LeagueActivityBot.Host.dll"]
