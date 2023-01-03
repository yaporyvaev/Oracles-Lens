FROM mcr.microsoft.com/dotnet/sdk:3.1.426-focal AS build-env
WORKDIR /src

COPY ./*.sln ./NuGet.Config ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore
COPY ./ ./
RUN dotnet publish ./LeagueActivityBot.Host/ -o /publish -c Release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:3.1.426-focal AS runtime
WORKDIR /app
COPY --from=build-env /publish .
COPY --from=build-env ./dotnet-tools /usr/local/bin

# change timezone from ETC to local
RUN apt update && apt install tzdata -y
ENV TZ="Europe/Moscow"


ENTRYPOINT ["dotnet","LeagueActivityBot.Host.dll"]
