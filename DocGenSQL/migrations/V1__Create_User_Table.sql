CREATE TABLE [User] (
  [UserID] int IDENTITY(1, 1),
  [UserSub] varchar(150),
  [UserName] varchar(50),
  [UserEmail] varchar(100),
  CONSTRAINT PK_User PRIMARY KEY NONCLUSTERED (UserID),
  CONSTRAINT UserSub UNIQUE(UserSub)
);