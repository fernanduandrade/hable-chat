FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HableChat.API/HableChat.API.csproj", "src/HableChat.API/"]
COPY ["src/HableChat.Infra/HableChat.Infra.csproj", "src/HableChat.Infra/"]
COPY ["src/HableChat.Application/HableChat.Application.csproj", "src/HableChat.Application/"]
COPY ["src/HableChat.Domain/HableChat.Domain.csproj", "src/HableChat.Domain/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
RUN dotnet restore "src/HableChat.API/HableChat.API.csproj"
COPY . .
WORKDIR "/src/src/HableChat.API"
RUN dotnet build "HableChat.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HableChat.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HableChat.API.dll"]
