#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MicroRabbit.Orders.Api/MicroRabbit.Orders.Api.csproj", "MicroRabbit.Orders.Api/"]
COPY ["MicroRabbit.Infrastructure.IoC/MicroRabbit.Infrastructure.IoC.csproj", "MicroRabbit.Infrastructure.IoC/"]
COPY ["MicroRabbit.Domain.Core/MicroRabbit.Domain.Core.csproj", "MicroRabbit.Domain.Core/"]
COPY ["MicroRabbit.Infrastructure.Bus/MicroRabbit.Infrastructure.Bus.csproj", "MicroRabbit.Infrastructure.Bus/"]
COPY ["MicroRabbit.Orders.Application/MicroRabbit.Orders.Application.csproj", "MicroRabbit.Orders.Application/"]
COPY ["MicroRabbit.Application/MicroRabbit.Application.csproj", "MicroRabbit.Application/"]
COPY ["MicroRabbit.Orders.Domain/MicroRabbit.Orders.Domain.csproj", "MicroRabbit.Orders.Domain/"]
COPY ["MicroRabbit.Orders.Data/MicroRabbit.Orders.Data.csproj", "MicroRabbit.Orders.Data/"]
COPY ["MicroRabbit.Data/MicroRabbit.Data.csproj", "MicroRabbit.Data/"]
COPY ["MicroRabbit.Orders.Infrastructure.Synchronous/MicroRabbit.Orders.Infrastructure.Synchronous.csproj", "MicroRabbit.Orders.Infrastructure.Synchronous/"]
RUN dotnet restore "MicroRabbit.Orders.Api/MicroRabbit.Orders.Api.csproj"
COPY . .
WORKDIR "/src/MicroRabbit.Orders.Api"
RUN dotnet build "MicroRabbit.Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroRabbit.Orders.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroRabbit.Orders.Api.dll"]