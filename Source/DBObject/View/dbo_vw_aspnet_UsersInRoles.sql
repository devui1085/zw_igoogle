If Object_ID('dbo.vw_aspnet_UsersInRoles') Is Not Null
	Drop View dbo.vw_aspnet_UsersInRoles
GO

  CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
  
