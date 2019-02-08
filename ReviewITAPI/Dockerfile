# RUN ENV
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# BUILD ENV
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
RUN echo ${HOME}
RUN dotnet dev-certs https --clean; dotnet dev-certs https -ep /.aspnet/https/aspnetapp.pfx -p crypticpassword; update-ca-certificates
COPY ["ReviewITAPI/ReviewITAPI.csproj", "ReviewITAPI/"]
COPY ["ReviewITAPI.Test/ReviewITAPI.Test.csproj", "ReviewITAPI/"]
RUN dotnet restore "ReviewITAPI/ReviewITAPI.csproj" -nowarn:msb3202,nu1503
COPY . .
WORKDIR "/src/ReviewITAPI"
RUN dotnet build "ReviewITAPI.csproj" -c Release -o /app
FROM build AS publish
RUN dotnet publish "ReviewITAPI.csproj" -c Release -o /app

# RUN TEST
FROM build AS testrunner
RUN ls 
WORKDIR /app/tests
COPY ReviewITAPI.Test/. .
RUN dotnet test
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM base AS final
WORKDIR /app
COPY --from=build /.aspnet/https/aspnetapp.pfx /.aspnet/https/aspnetapp.pfx
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ReviewITAPI.dll"]
