#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sas.Client/Sas.Client.csproj", "Sas.Client/"]
RUN dotnet restore "Sas.Client/Sas.Client.csproj"
COPY . .
WORKDIR "/src/Sas.Client"
RUN dotnet build "Sas.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sas.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sas.Client.dll"]