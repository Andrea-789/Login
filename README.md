# Login
 Login Web Api

Angular 13 ASP NET Core 5 SQL Server WebApi Authentication with email to activate user

Sample Login project demonstrating JWT-based auth in Angular(v13) and ASP.NET Core(v5) with user registration and email to activate user plus pages forgot and change password 

There's a simple Role managment ADMIN and USER, the first user who sign-up is the ADMIN, all the other is USER

Angular runs on localhost:4200, ASP.NET on localhost:6002

You can change them in enviroment.ts and in appsettings.json. Here you have to set your DB connection string, your Secret to ecrypt your password, the ExpirationPwd (180 days in this sample)
You have to set also all email data to allow the api to send email to user

Developed with VSCode, Visual Studio 2019 and SQL Server 2019
