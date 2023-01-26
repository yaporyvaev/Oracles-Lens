FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src

COPY ./*.sln ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore
COPY ./ ./
RUN dotnet publish ./LeagueActivityBot.Host/ -o /publish -c Release --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build-env /publish .

RUN apt-get update && \
    apt-get install -yq tzdata && \
    ln -fs /usr/share/zoneinfo/Europe/Moscow /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata
ENV TZ="Europe/Moscow"

ENTRYPOINT ["dotnet","LeagueActivityBot.Host.dll"]