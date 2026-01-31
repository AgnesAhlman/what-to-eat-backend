param location string = resourceGroup().location
param appName string = 'what-to-eat'

// 1. App Service Plan (The Server/Compute)
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${appName}-plan'
  location: location
  sku: { name: 'B1' } // A stable, low-cost tier for C#
  kind: 'linux'
  properties: { reserved: true }
}

// 2. The App Service (The actual API host)
resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: appName
  location: location
  tags: {
    'azd-service-name': 'api'
  }
  identity: { type: 'SystemAssigned' } // Critical for Managed Identity auth
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      connectionStrings: [
        {
          name: 'AzureSqlDb'
          type: 'SQLAzure'
          connectionString: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};Authentication=Active Directory Default;Encrypt=True;'
        }
      ]
    }
  }
}

// 3. SQL Server (Unified)
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: '${appName}-sql'
  location: location
  properties: {
    // Merge Admin & Auth Policy directly here
    administrators: {
      administratorType: 'ActiveDirectory'
      login: az.deployer().userPrincipalName // Uses the clean parameter
      sid: az.deployer().objectId // The ID is safe to auto-detect
      tenantId: subscription().tenantId
      azureADOnlyAuthentication: true
    }
  }
}

// 3.2 AD Only Auth
resource adOnlyAuth 'Microsoft.Sql/servers/azureADOnlyAuthentications@2022-05-01-preview' = {
  parent: sqlServer
  name: 'Default'
  properties: {
    azureADOnlyAuthentication: true
  }
}

// 4. SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: '${appName}-db'
  location: location
  sku: { name: 'GP_S_Gen5_1', tier: 'GeneralPurpose' }
}

// 5. Firewall Rule: Allow the App Service to talk to SQL
resource allowAzure 'Microsoft.Sql/servers/firewallRules@2022-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}
