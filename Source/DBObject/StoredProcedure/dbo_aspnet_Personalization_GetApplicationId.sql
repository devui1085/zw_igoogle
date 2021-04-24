If Object_ID('dbo.aspnet_Personalization_GetApplicationId') Is Not Null
	Drop Procedure dbo.aspnet_Personalization_GetApplicationId
GO
CREATE PROCEDURE dbo.aspnet_Personalization_GetApplicationId (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END
