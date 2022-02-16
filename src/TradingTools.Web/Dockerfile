#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TradingTools.Web/TradingTools.Web.csproj", "TradingTools.Web/"]
RUN dotnet restore "TradingTools.Web/TradingTools.Web.csproj"
COPY . .
WORKDIR "/src/TradingTools.Web"
RUN dotnet build "TradingTools.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingTools.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingTools.Web.dll"]