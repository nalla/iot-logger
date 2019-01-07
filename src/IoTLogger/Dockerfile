FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1.5-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
RUN sed -i "s|http://10.0.0.5:8086|http://influxdb:8086|g" appsettings.json
RUN echo '{"urls":"http://+:80"}' > hosting.json
ENTRYPOINT [ "dotnet", "IoTLogger.dll" ]
