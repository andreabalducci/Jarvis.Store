FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Jarvis.Store.Service/Jarvis.Store.Service.csproj", "Jarvis.Store.Service/"]
RUN dotnet restore "Jarvis.Store.Service/Jarvis.Store.Service.csproj"
COPY . .
WORKDIR "/src/Jarvis.Store.Service"
RUN dotnet build "Jarvis.Store.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jarvis.Store.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jarvis.Store.Service.dll"]
