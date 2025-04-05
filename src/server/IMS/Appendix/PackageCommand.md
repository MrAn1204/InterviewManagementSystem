# Add package for IMS.Domain
dotnet add IMS.Domain package Microsoft.EntityFrameworkCore
dotnet add IMS.Domain package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Add package for IMS.Business
dotnet add IMS.Business package Microsoft.EntityFrameworkCore
dotnet add IMS.Business package MediatR
dotnet add IMS.Business package AutoMapper
dotnet add IMS.Business package Newtonsoft.Json

# Add package for IMS.Data
dotnet add IMS.Data package Microsoft.EntityFrameworkCore
dotnet add IMS.Data package Microsoft.EntityFrameworkCore.SqlServer
dotnet add IMS.Data package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add IMS.Data package Microsoft.AspNetCore.Identity

# Add package for IMS.API
dotnet add IMS.API package Microsoft.EntityFrameworkCore
dotnet add IMS.API package Microsoft.EntityFrameworkCore.SqlServer
dotnet add IMS.API package Microsoft.EntityFrameworkCore.Design
dotnet add IMS.API package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add IMS.API package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add IMS.API package Microsoft.AspNetCore.Identity.UI
dotnet add IMS.API package Microsoft.AspNetCore.Mvc.NewtonsoftJson
dotnet add IMS.API package Microsoft.AspNetCore.Mvc.Versioning
dotnet add IMS.API package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
dotnet add IMS.API package Swashbuckle.AspNetCore
dotnet add IMS.API package Swashbuckle.AspNetCore.Swagger
dotnet add IMS.API package Swashbuckle.AspNetCore.SwaggerGen
dotnet add IMS.API package Swashbuckle.AspNetCore.SwaggerUI
dotnet add IMS.API package Microsoft.Extensions.Configuration
