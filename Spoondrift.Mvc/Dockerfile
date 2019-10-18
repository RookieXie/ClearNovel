FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Spoondrift.Mvc/Spoondrift.Mvc.csproj", "Spoondrift.Mvc/"]
RUN dotnet restore "Spoondrift.Mvc/Spoondrift.Mvc.csproj"
COPY . .
WORKDIR "/src/Spoondrift.Mvc"
RUN dotnet build "Spoondrift.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spoondrift.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spoondrift.Mvc.dll"]