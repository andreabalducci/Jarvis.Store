FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Jarvis.Store/Jarvis.Store.csproj", "Jarvis.Store/"]
RUN dotnet restore "Jarvis.Store/Jarvis.Store.csproj"
COPY . .
WORKDIR "/src/Jarvis.Store"
RUN dotnet build "Jarvis.Store.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jarvis.Store.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jarvis.Store.dll"]
