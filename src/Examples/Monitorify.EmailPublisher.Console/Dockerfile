#dotnet publish -c Release -o out

FROM microsoft/dotnet:1.1-runtime
WORKDIR /monitorify
COPY out .
COPY config.json .
ENTRYPOINT ["dotnet", "Monitorify.EmailPublisher.Console.dll"]