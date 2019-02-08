FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
RUN echo ${HOME}
RUN dotnet dev-certs https --clean; dotnet dev-certs https -ep /.aspnet/https/aspnetapp.pfx -p crypticpassword; update-ca-certificates
COPY ["ReviewITAPI/ReviewITAPI.csproj", "ReviewITAPI/"]
RUN dotnet restore "ReviewITAPI/ReviewITAPI.csproj" -nowarn:msb3202,nu1503
COPY . .
WORKDIR "/src/ReviewITAPI"
RUN dotnet build "ReviewITAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ReviewITAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /.aspnet/https/aspnetapp.pfx /.aspnet/https/aspnetapp.pfx
COPY --from=publish /app .
RUN ls -la
run ls .. -la
run ls ../app -la
run ls ../.aspnet/https/ -la
ENTRYPOINT ["dotnet", "ReviewITAPI.dll"]
