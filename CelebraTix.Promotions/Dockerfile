FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CelebraTix/CelebraTix.csproj", "CelebraTix/"]
RUN dotnet restore "CelebraTix/CelebraTix.csproj"
COPY . .
WORKDIR "/src/CelebraTix"
RUN dotnet build "CelebraTix.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CelebraTix.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CelebraTix.dll"]
