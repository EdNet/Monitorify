FROM microsoft/dotnet:1.1-runtime
WORKDIR /monitorify
COPY out .
ENTRYPOINT ["dotnet", "Monitorify.Core.Console.dll"]

#WORKDIR /bin/Debug/netcoreapp1.1/publish
#WORKDIR /bin/
#COPY out .
#RUN ls
#CMD ["ls"]
#ENTRYPOINT ["dotnet", "Monitorify.Core.Console.dll"]