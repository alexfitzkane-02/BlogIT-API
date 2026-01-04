# BlogIT-API

BlogIT-API is a C# library that contains all the server side logic for performing CRUD operations related to blog posts users make, credentialing around the application, and more!  

This project makes use of Azure Key Vault storage. This allows me to store all sensitive information in the cloud, such as database connection strings which contains a username and password, and keep that out of the application. 

## Nuget Packages

The following packages are needed for the core function of the application
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore.SwaggerUI

The following packages are needed for Jwt Authentication
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
- System.IdentityModel.Tokens.Jwt

The following packages are needed to hook into Azure Key Vault 
- Azure.Extensions.AspNetCore.Configuration.Secrets
- Azure.Identity
- Azure.Security.KeyVault.Secrets

When running the application locally on your machine, you need to make sure you have Azure CLI installed locally. To check if you already have it installed you can perform the following CMD promt: 

```bash
az version
```
If you do not have it, you can install it through PowerShell with the following command:

```powershell
winget install --exact --id Microsoft.AzureCLI
```

Once you confirm  the Azure CLI is installed, restart Visual Studio and login through the Package Manager Console with the following command:

```bash
az login
```

In certain scenarios you might need to append your tenantID to the command: 

```bash
az login --tenant {insert tenantID}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
