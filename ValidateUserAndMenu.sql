﻿
/**
EXEC [dbo].[ValidateUserAndMenu] 'jk.123test@gmail.com','P@ssw0rd'
**/

CREATE PROC [dbo].[ValidateUserAndMenu] @UserName AS VARCHAR(50),
                                        @Password AS VARCHAR(50)
AS
     BEGIN
         DECLARE @ErrorMessage VARCHAR(200)= '';
         DECLARE @IsLoginValid BIT= 0;
         DECLARE @RoleName VARCHAR(50)= '';
         DECLARE @UserFullName VARCHAR(250)= '';
         IF NOT EXISTS
(
    SELECT 1
    FROM dbo.[UserProfileMaster]
    WHERE Primary_Email = @UserName
)
             BEGIN
                 SET @ErrorMessage = 'Invalid User Name';
                 RAISERROR(@ErrorMessage, 11, 1);
                 RETURN;
             END;
         IF NOT EXISTS
(
    SELECT 1
    FROM dbo.[UserProfileMaster]
    WHERE Primary_Email = @UserName
          AND [Password] = @Password
)
             BEGIN
                 SET @ErrorMessage = 'User Name or Password is invalid.';
                 RAISERROR(@ErrorMessage, 11, 1);
                 RETURN;
             END;
         IF EXISTS
(
    SELECT 1
    FROM dbo.[UserProfileMaster]
    WHERE Primary_Email = @UserName
          AND [Password] = @Password
)
             BEGIN
--SELECT * FROM dbo.MenuMaster
--SELECT * FROM dbo.RoleMaster
--SELECT * FROM dbo.UserAddressDetails
--SELECT * FROM dbo.UserProfileMaster
                 SELECT @RoleName = R.RoleName,
                        @IsLoginValid = 1,
                        @UserFullName = U.User_Fname+ISNULL(U.User_Mname, '')+' '+U.User_Lname
                 FROM dbo.[UserProfileMaster] U
                      INNER JOIN dbo.RoleMaster R ON U.RoleID = R.RoleID
                 WHERE U.Primary_Email = @UserName
                       AND U.[Password] = @Password;
             END;
         SELECT @IsLoginValid AS IsLoginValid,
                @RoleName AS RoleName,
                @UserFullName AS UserFullName;
     END;