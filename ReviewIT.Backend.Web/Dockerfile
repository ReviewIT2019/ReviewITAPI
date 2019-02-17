# INIT RUN ENV
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# BUILD ENV. Setup dev cert and check that it builds
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
RUN echo "Home directory:" + ${HOME}
RUN dotnet dev-certs https --clean; dotnet dev-certs https -ep /.aspnet/https/aspnetapp.pfx -p crypticpassword; update-ca-certificates
COPY ["ReviewIT.Backend.Web/ReviewIT.Backend.Web.csproj", "ReviewIT.Backend.Web/"]
COPY ["ReviewIT.Backend.Web.Tests/ReviewIT.Backend.Web.Tests.csproj", "ReviewIT.Backend.Web.Tests/"]
RUN dotnet restore "ReviewIT.Backend.Web/ReviewIT.Backend.Web.csproj"
COPY . .
WORKDIR /src/ReviewIT.Backend.Web
RUN dotnet build "ReviewIT.Backend.Web.csproj" -c Release -o /app

# COMPILE
FROM build AS publish
RUN dotnet publish "ReviewIT.Backend.Web.csproj" -c Release -o /app				

# RUN TEST
FROM build AS testrunner
WORKDIR /app/tests
COPY . .	
# list all test projects
RUN ls -la | grep "**/**/**.Tests"
RUN dotnet test --logger "trx;LogFileName=testresults.trx" --collect:"Code Coverage"
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

# "ASSEMBLE" FINAL IMAGE
FROM base AS final
WORKDIR /app
# COPY DEV CERT
COPY --from=build /.aspnet/https/aspnetapp.pfx /.aspnet/https/aspnetapp.pfx		
# COPY TESTRESULTS from each test project listed above				
COPY --from=testrunner /app/tests/ReviewIT.Backend.Web.Tests/TestResults TestResults/ReviewIT.Backend.Web.Tests
COPY --from=testrunner /app/tests/ReviewIT.Backend.Models.Tests/TestResults TestResults/ReviewIT.Backend.Models.Tests
COPY --from=testrunner /app/tests/ReviewIT.Backend.BibTexParser.Tests/TestResults TestResults/ReviewIT.Backend.BibTexParser.Tests
# COPY DLL
COPY --from=publish /app .														
# VERIFY CONTENS OF FINAL IMAGE
RUN ls -la					
# VERIFY TEST RESULTS WERE COPIED OVER
RUN ls -la TestResults/															  
ENTRYPOINT ["dotnet", "ReviewIT.Backend.Web.dll"]								