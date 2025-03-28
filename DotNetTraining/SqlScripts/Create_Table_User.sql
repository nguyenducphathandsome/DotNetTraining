CREATE TABLE "users"
(
    "Id" NVARCHAR(MAX) NOT NULL,
    "Roles" NVARCHAR(MAX),
    "UserName" NVARCHAR(MAX),
    "FirstName" NVARCHAR(MAX),
    "LastName" NVARCHAR(MAX),
    "FullName" NVARCHAR(MAX),
    "Email" NVARCHAR(MAX),
    "Password" NVARCHAR(MAX),
    "CreatedAt" NVARCHAR(MAX),
    "UpdatedAt" NVARCHAR(MAX),
    "Id" NVARCHAR(MAX),
    CONSTRAINT "users_pkey" PRIMARY KEY ("Id")
)

