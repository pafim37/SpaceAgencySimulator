FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY src/Services/Body/ src/Services/Body/
COPY src/Services/BodySystem/Sas.BodySystem.Process/ src/Services/BodySystem/Sas.BodySystem.Process/
COPY src/Services/Mathematica/ src/Services/Mathematica/
COPY src/Services/Orbit/Sas.Orbit.Service/ src/Services/Orbit/Sas.Orbit.Service/
RUN dotnet publish src/Services/BodySystem/Sas.BodySystem.Process/Sas.BodySystem.Process.csproj -c Release -o /publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=3742
EXPOSE 3742
ENTRYPOINT ["dotnet", "Sas.BodySystem.Process.dll"]
