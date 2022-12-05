CREATE TABLE IF NOT EXISTS Users(
    Id UUID PRIMARY KEY,
    Username VARCHAR(30) NOT NULL,
    PasswordHash BYTEA NOT NULL,
    PasswordSalt BYTEA NOT NULL,
    Email VARCHAR(255) NOT NULL,
    RefreshToken VARCHAR(255),
    TokenCreated TIMESTAMP,
    TokenExpires TIMESTAMP,
    VerificationToken VARCHAR NOT NULL,
    VerifiedAt TIMESTAMP);
