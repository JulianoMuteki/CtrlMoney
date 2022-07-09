#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CtrlMoney.UI.Web/CtrlMoney.UI.Web.csproj", "src/CtrlMoney.UI.Web/"]
COPY ["src/CtrlMoney.CrossCutting.Ioc/CtrlMoney.CrossCutting.Ioc.csproj", "src/CtrlMoney.CrossCutting.Ioc/"]
COPY ["src/CtrlMoney.Infra.Repository/CtrlMoney.Infra.Repository.csproj", "src/CtrlMoney.Infra.Repository/"]
COPY ["src/CtrlMoney.Infra.Context/CtrlMoney.Infra.Context.csproj", "src/CtrlMoney.Infra.Context/"]
COPY ["src/CtrlMoney.Domain/CtrlMoney.Domain.csproj", "src/CtrlMoney.Domain/"]
COPY ["src/CtrlMoney.CrossCutting/CtrlMoney.CrossCutting.csproj", "src/CtrlMoney.CrossCutting/"]
COPY ["src/CtrlMoney.AppService/CtrlMoney.AppService.csproj", "src/CtrlMoney.AppService/"]
COPY ["src/CtrlMoney.WorkSheet.Service/CtrlMoney.WorkSheet.Service.csproj", "src/CtrlMoney.WorkSheet.Service/"]
RUN dotnet restore "src/CtrlMoney.UI.Web/CtrlMoney.UI.Web.csproj"
COPY . .
WORKDIR "/src/src/CtrlMoney.UI.Web"
RUN dotnet build "CtrlMoney.UI.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CtrlMoney.UI.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CtrlMoney.UI.Web.dll"]