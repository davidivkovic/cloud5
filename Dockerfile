FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy-chiseled AS base
WORKDIR /
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cloud5.csproj", "./"]
RUN dotnet restore "Cloud5.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Cloud5.csproj" -c $BUILD_CONFIGURATION -o /build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cloud5.csproj" -c $BUILD_CONFIGURATION -o /publish /p:UseAppHost=false

FROM base AS final
WORKDIR /
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "Cloud5.dll"]
