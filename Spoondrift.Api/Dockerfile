FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Spoondrift.Api/Spoondrift.Api.csproj", "Spoondrift.Api/"]
RUN dotnet restore "Spoondrift.Api/Spoondrift.Api.csproj"
COPY . .
WORKDIR "/src/Spoondrift.Api"
RUN dotnet build "Spoondrift.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spoondrift.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spoondrift.Api.dll"]