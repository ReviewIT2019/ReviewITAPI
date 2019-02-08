FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ReviewITAPI/ReviewITAPI.csproj", "ReviewITAPI/"]
RUN dotnet restore "ReviewITAPI/ReviewITAPI.csproj" -nowarn:msb3202,nu1503
COPY . .
WORKDIR "/src/ReviewITAPI"
RUN dotnet build "ReviewITAPI.csproj" -c Release -o /app

FROM build AS publish

RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep /.aspnet/https/aspnetapp.pfx -p crypticpassword
RUN update-ca-certificates

RUN dotnet publish "ReviewITAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ReviewITAPI.dll"]
