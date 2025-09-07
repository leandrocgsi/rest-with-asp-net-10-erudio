CREATE TABLE dbo.person (
  [id] bigint NOT NULL IDENTITY,
  [first_name] varchar(80) NOT NULL,
  [last_name] varchar(80) NOT NULL,
  [address] varchar(100) NOT NULL,
  [gender] varchar(6) NOT NULL,
  PRIMARY KEY ([id])
)