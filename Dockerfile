FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/GraphAPI/Graph.API.csproj", "src/GraphAPI/"]
COPY ["src/Graph.Application/Graph.Application.csproj", "src/Graph.Application/"]
COPY ["src/Graph.Infrastructure/Graph.Infrastructure.csproj", "src/Graph.Infrastructure/"]
COPY ["src/Graph.CrossCutting/Graph.CrossCutting.csproj", "src/Graph.CrossCutting/"]
COPY ["src/Graph.CrossCutting.Ioc/Graph.CrossCutting.Ioc.csproj", "src/Graph.CrossCutting.Ioc/"]
COPY ["src/Graph.Domain/Graph.Domain.csproj", "src/Graph.Domain/"]
COPY ["src/Graph.Domain.Service/Graph.Domain.Service.csproj", "src/Graph.Domain.Service/"]
RUN dotnet restore "src/GraphAPI/Graph.API.csproj"
COPY . .
WORKDIR "/src/src/GraphAPI"
RUN dotnet build "Graph.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Graph.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Graph.API.dll"]