 FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
 WORKDIR /app
 EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-nanoserver-1809 AS build
WORKDIR /src
COPY ["Novel.Blazor.Web/Novel.Blazor.Web.csproj", "Novel.Blazor.Web/"]
RUN dotnet restore "Novel.Blazor.Web/Novel.Blazor.Web.csproj"
COPY . .
WORKDIR "/src/Novel.Blazor.Web"
RUN dotnet build "Novel.Blazor.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Novel.Blazor.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Novel.Blazor.Web.dll"]