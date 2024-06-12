FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY src/*.csproj .
RUN dotnet restore

COPY src/. .
RUN dotnet publish --no-restore -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app
COPY --from=build /app .

ENV PORT 3000
EXPOSE ${PORT}

USER 1000
ENTRYPOINT ["dotnet", "demo-dotnet.dll"]
