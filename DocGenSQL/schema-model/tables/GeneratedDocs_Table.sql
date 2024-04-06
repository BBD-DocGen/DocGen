CREATE TABLE [GeneratedDocument] (
  [GenDocID] int IDENTITY(1,1),
  [DocTypeID] int,
  [UpDocID] int,
  [GenDocName] varchar(100),
  [GenDocURL] varchar(200),
  CONSTRAINT PK_GeneratedDocument PRIMARY KEY NONCLUSTERED (GenDocID),
  CONSTRAINT FK_GeneratedDocument_UpDocID FOREIGN KEY (UpDocID)
  REFERENCES [dbo].[UploadDocument] (UpDocID),
  CONSTRAINT FK_GeneratedDocument_DocTypeID FOREIGN KEY (DocTypeID)
  REFERENCES [dbo].[DocumentType] (DocTypeID)
);