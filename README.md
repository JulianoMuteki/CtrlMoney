# Ctrl Money
Personal spending control


sudo service docker start

sudo docker pull mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04

sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd' --network=mssql-network -e 'MSSQL_PID=Developer' -p 14333:1433 -d mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04

ip addr show

inet 172.31.235.196/14333



# Migrations Overview

## Open Package Manager Console > ctrlbox\src\CtrlBox.Infra.Context

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

