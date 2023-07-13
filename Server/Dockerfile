FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Base/Module/FSI.WebAPI/FSI.WebAPI.csproj", "Base/Module/FSI.WebAPI/"]
COPY ["Base/Module/FSI.HttpApi/FSI.HttpApi.csproj", "Base/Module/FSI.HttpApi/"]
COPY ["Base/Module/FSI.HttpApi.Client/FSI.HttpApi.Client.csproj", "Base/Module/FSI.HttpApi.Client/"]
COPY ["Base/Module/FSI.GrpcClient/FSI.GrpcClient.csproj", "Base/Module/FSI.GrpcClient/"]
COPY ["Base/Module/FSI.EFCore/FSI.EFCore.csproj", "Base/Module/FSI.EFCore/"]
COPY ["Base/Module/FSI.Domain/FSI.Domain.csproj", "Base/Module/FSI.Domain/"]
COPY ["Base/Module/FSI.Domain.Shared/FSI.Domain.Shared.csproj", "Base/Module/FSI.Domain.Shared/"]
COPY ["Base/Module/FSI.Application/FSI.Application.csproj", "Base/Module/FSI.Application/"]
COPY ["Base/Module/FSI.Application.Contracts/FSI.Application.Contracts.csproj", "Base/Module/FSI.Application.Contracts/"]
COPY ["Base/Core/FSI.Common/FSI.Common.csproj", "Base/Core/FSI.Common/"]
COPY ["Base/Core/FSI.Lib/FSI.Lib.csproj", "Base/Core/FSI.Lib/"]
RUN dotnet restore "Base/Module/FSI.WebAPI/FSI.WebAPI.csproj"
COPY . .
WORKDIR "/src/Base/Module/FSI.WebAPI"
RUN dotnet build "FSI.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FSI.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FSI.WebAPI.dll"]