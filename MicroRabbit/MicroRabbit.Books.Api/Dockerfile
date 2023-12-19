#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroRabbit.Books.Api/MicroRabbit.Books.Api.csproj", "MicroRabbit.Books.Api/"]
COPY ["MicroRabbit.Books.Application/MicroRabbit.Books.Application.csproj", "MicroRabbit.Books.Application/"]
COPY ["MicroRabbit.Application/MicroRabbit.Application.csproj", "MicroRabbit.Application/"]
COPY ["MicroRabbit.Domain.Core/MicroRabbit.Domain.Core.csproj", "MicroRabbit.Domain.Core/"]
COPY ["MicroRabbit.Books.Domain/MicroRabbit.Books.Domain.csproj", "MicroRabbit.Books.Domain/"]
COPY ["MicroRabbit.Books.Data/MicroRabbit.Books.Data.csproj", "MicroRabbit.Books.Data/"]
COPY ["MicroRabbit.Data/MicroRabbit.Data.csproj", "MicroRabbit.Data/"]
COPY ["MicroRabbit.Infrastructure.IoC/MicroRabbit.Infrastructure.IoC.csproj", "MicroRabbit.Infrastructure.IoC/"]
COPY ["MicroRabbit.Infrastructure.Bus/MicroRabbit.Infrastructure.Bus.csproj", "MicroRabbit.Infrastructure.Bus/"]
COPY ["MicroRabbit.Infrastructure.Synchronous/MicroRabbit.Infrastructure.Synchronous.csproj", "MicroRabbit.Infrastructure.Synchronous/"]
RUN dotnet restore "MicroRabbit.Books.Api/MicroRabbit.Books.Api.csproj"
COPY . .
WORKDIR "/src/MicroRabbit.Books.Api"
RUN dotnet build "MicroRabbit.Books.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroRabbit.Books.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroRabbit.Books.Api.dll"]