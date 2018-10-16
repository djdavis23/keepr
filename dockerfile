FROM microsoft/dotnet:2.1-sdk
WORKDIR /app
COPY . .
CMD ASPNETCORE__URLS=http://*:$PORT dotnet keepr.dll

# Adding a comment to trigger docker file change