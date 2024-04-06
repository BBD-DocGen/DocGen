CREATE TABLE [DocumentType] (
  [DocTypeID] int IDENTITY(1, 1),
  [DocTypeName] varchar(100),
  [DocTypeDescription] varchar(200),
  CONSTRAINT PK_DocumentType PRIMARY KEY NONCLUSTERED (DocTypeID),
);