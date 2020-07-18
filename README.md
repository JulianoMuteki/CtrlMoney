# Ctrl Money
Controle de gastos e investimentos

### AdminLTE Template
AdminLTE has been carefully coded with clear comments in all of its JS, SCSS and HTML files. SCSS has been used to increase code customizability.
Download from GitHub releases.
https://github.com/ColorlibHQ/AdminLTE


# Docker
sudo service docker start

sudo docker pull mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04

sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd' --network=mssql-network -e 'MSSQL_PID=Developer' -p 14333:1433 -d mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04

ip addr show

inet 172.31.235.196/14333



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

