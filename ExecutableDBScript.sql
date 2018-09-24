INSERT INTO RoleMaster (RoleName,IsActive)  VALUES ('SuperAdmin',1)
GO

ALTER TABLE ProductMaster ALTER COLUMN IsOffer BIT NOT NULL
GO

ALTER TABLE UserProfileMaster
ADD CONSTRAINT FK_Role_UserProfileMaster
FOREIGN KEY (RoleID) REFERENCES RoleMaster(RoleID)
GO