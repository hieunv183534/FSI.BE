#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Modules/Data/VNPTNET.NOM.Data.WebAPI/VNPTNET.NOM.Data.WebAPI.csproj", "Modules/Data/VNPTNET.NOM.Data.WebAPI/"]
RUN dotnet restore "Modules/Data/VNPTNET.NOM.Data.WebAPI/VNPTNET.NOM.Data.WebAPI.csproj"
COPY . .
WORKDIR "/src/Modules/Data/VNPTNET.NOM.Data.WebAPI"
RUN dotnet build "VNPTNET.NOM.Data.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VNPTNET.NOM.Data.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VNPTNET.NOM.Data.WebAPI.dll"]