FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY src/Services/Notification/Sas.Notification.Process/ src/Services/Notification/Sas.Notification.Process/
RUN dotnet publish src/Services/Notification/Sas.Notification.Process/Sas.Notification.Process.csproj -c Release -o /publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=3737
EXPOSE 3737
ENTRYPOINT ["dotnet", "Sas.Notification.Process.dll"]
