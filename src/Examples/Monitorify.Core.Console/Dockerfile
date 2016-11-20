FROM microsoft/dotnet:1.1-runtime
WORKDIR /monitorify
COPY out .
COPY config.json .
ENTRYPOINT ["dotnet", "Monitorify.Core.Console.dll"]