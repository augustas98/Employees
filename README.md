# Employees

Prerequisites: .Net 5
https://dotnet.microsoft.com/download/dotnet/5.0

Before running the API for the first time, please do:

1. edit the log4net.config.
"<file value="\repos\Employees\Employees\Logs\log-" />"
It should have an absolute path.


2. Run migrations
Open powershell in \repos\Employees\Employees
type: dotnet ef database update

You are good to go.




