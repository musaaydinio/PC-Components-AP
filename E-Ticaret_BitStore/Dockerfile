FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "Web_E-Ticaret_BitStore/Web_E-Ticaret_BitStore.csproj"
RUN dotnet publish "Web_E-Ticaret_BitStore/Web_E-Ticaret_BitStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Web_E-Ticaret_BitStore.dll"]