cd src/Abdt.Corpportal.Notification.Push.Storage.PostgreSql
dotnet restore
dotnet ef -h
dotnet ef migrations add Initial --verbose --project ../../src/Abdt.Corpportal.Notification.Push.Storage.PostgreSql --startup-project ../../src/Abdt.Corpportal.Notification.Push.Web
dotnet ef database update --verbose --project ../../src/Abdt.Corpportal.Notification.Push.Storage.PostgreSql --startup-project ../../src/Abdt.Corpportal.Notification.Push.Web