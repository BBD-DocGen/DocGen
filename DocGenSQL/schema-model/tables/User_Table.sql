CREATE TABLE [User] (
  [UserID] int IDENTITY(1, 1),
  [UserName] varchar(50),
  [UserEmail] varchar(100),
  CONSTRAINT PK_User PRIMARY KEY NONCLUSTERED (UserID),
);