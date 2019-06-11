FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["trojkaty_api/trojkaty_api.csproj", "trojkaty_api/"]
RUN dotnet restore "trojkaty_api/trojkaty_api.csproj"
COPY . .
WORKDIR "/src/trojkaty_api"
RUN dotnet build "trojkaty_api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "trojkaty_api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "trojkaty_api.dll"]