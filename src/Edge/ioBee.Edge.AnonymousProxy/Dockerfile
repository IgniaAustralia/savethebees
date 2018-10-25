FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50731
EXPOSE 44326

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ioBee.Edge.AnonymousProxy/ioBee.Edge.AnonymousProxy.csproj", "ioBee.Edge.AnonymousProxy/"]
RUN dotnet restore "ioBee.Edge.AnonymousProxy/ioBee.Edge.AnonymousProxy.csproj"
COPY . .
WORKDIR "/src/ioBee.Edge.AnonymousProxy"
RUN dotnet build "ioBee.Edge.AnonymousProxy.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ioBee.Edge.AnonymousProxy.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ioBee.Edge.AnonymousProxy.dll"]