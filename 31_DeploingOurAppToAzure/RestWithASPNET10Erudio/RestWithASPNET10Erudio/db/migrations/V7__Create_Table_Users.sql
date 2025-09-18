CREATE TABLE dbo.users (
    [id] bigint NOT NULL IDENTITY,
    [user_name] varchar(50) NOT NULL,
    [password] varchar(130) NOT NULL,
    [full_name] varchar(120) NOT NULL,
    [refresh_token] varchar(500) NULL,
    [refresh_token_expiry_time] datetime2(6) NULL,
    CONSTRAINT UQ_users_user_name UNIQUE ([user_name]),
    PRIMARY KEY ([id])
);