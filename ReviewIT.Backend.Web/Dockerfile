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
COPY ["ReviewIT.Backend.Web/ReviewIT.Backend.Web.csproj", "ReviewIT.Backend.Web/"]
COPY ["ReviewIT.Backend.Web.Tests/ReviewIT.Backend.Web.Tests.csproj", "ReviewIT.Backend.Web.Tests/"]
RUN dotnet restore "ReviewIT.Backend.Web/ReviewIT.Backend.Web.csproj" -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/ReviewIT.Backend.Web
RUN dotnet build "ReviewIT.Backend.Web.csproj" -c Release -o /app

# PUB ENV
FROM build AS publish
RUN dotnet publish "ReviewIT.Backend.Web.csproj" -c Release -o /app

# RUN TEST
FROM build AS testrunner
WORKDIR /app/tests
COPY . .
RUN dotnet test --logger "trx;LogFileName=testresults.trx"
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM base AS final
WORKDIR /app
COPY --from=build /.aspnet/https/aspnetapp.pfx /.aspnet/https/aspnetapp.pfx
COPY --from=testrunner /app/tests/ReviewIT.Backend.Web.Tests/TestResults ReviewIT.Backend.Web.Tests/TestResults
COPY --from=testrunner /app/tests/ReviewIT.Backend.Models.Tests/TestResults ReviewIT.Backend.Models.Tests/TestResults
COPY --from=publish /app .
RUN ls -la
ENTRYPOINT ["dotnet", "ReviewIT.Backend.Web.dll"]