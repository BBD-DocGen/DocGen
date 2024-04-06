CREATE TABLE [UploadDocument] (
  [UpDocID] int IDENTITY(1, 1),
  [UserID] int,
  [UpDocName] varchar(100),
  [UpDocURL] varchar(200),
  CONSTRAINT PK_UploadDocument PRIMARY KEY NONCLUSTERED (UpDocID),
  CONSTRAINT FK_UploadDocument_UserID FOREIGN KEY (UserID)
  REFERENCES [dbo].[User] (UserID)
);