FROM mcr.microsoft.com/dotnet/sdk:3.1.426-focal AS build-env
WORKDIR /src

COPY ./*.sln ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore
COPY ./ ./
RUN dotnet publish ./LeagueActivityBot.Host/ -o /publish -c Release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:3.1.426-focal AS runtime
WORKDIR /app
COPY --from=build-env /publish .


ENTRYPOINT ["dotnet","LeagueActivityBot.Host.dll"]
