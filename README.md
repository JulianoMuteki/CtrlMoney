# Ctrl Money
Controle de gastos e investimentos

### AdminLTE Template
AdminLTE has been carefully coded with clear comments in all of its JS, SCSS and HTML files. SCSS has been used to increase code customizability.
Download from GitHub releases.
https://github.com/ColorlibHQ/AdminLTE


# Docker
## SQL Server
sudo service docker start

sudo docker pull mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04

sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd' --network=mssql-network -e 'MSSQL_PID=Developer' -p 14333:1433 -d mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04

ip addr show

inet 172.31.235.196/14333

## PostgresSQL
CtrlMoney\src\docker-compose.yaml


# Migrations Overview

## Open Package Manager Console > ctrlmoney\src\CtrlMoney.Infra.Context

```Add-Migration InitialCreate```

or

```Update-Database```



    API ASP.NET Core 3.1
    ASP.NET MVC Core
    Fluent Validation
    Entity Framework Core
    SQLLite
    Docker
    Tests



# Packages
PM> Install-Package Microsoft.EntityFrameworkCore -Version 3.1.6
PM> Install-Package FluentValidation -Version 9.0.1
PM> Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 3.1.6
PM> Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 3.0.3
PM> Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -Version 3.1.4

# Database
https://www.npgsql.org/efcore/index.html
