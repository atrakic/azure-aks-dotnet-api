# https://mcr.microsoft.com/product/dotnet/sdk
# https://mcr.microsoft.com/v2/dotnet/sdk/tags/list
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:8.0-jammy-aot AS build
ARG TARGETARCH
WORKDIR /source

COPY src/*.csproj .
RUN dotnet restore -r linux-$TARGETARCH

COPY src/. .
RUN dotnet publish --no-restore -c Release -o /app

RUN dotnet publish -r linux-$TARGETARCH --no-restore -o /app
RUN rm /app/*.dbg /app/*.Development.json

# https://mcr.microsoft.com/product/dotnet/runtime-deps
# https://mcr.microsoft.com/v2/dotnet/runtime-deps/tags/list
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/runtime-deps:9.0.6-noble-chiseled-aot as final
WORKDIR /app
COPY --from=build /app .

ENV PORT 3000
EXPOSE ${PORT}

USER $APP_UID
ENTRYPOINT ["./dotnetApi"]
