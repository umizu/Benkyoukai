CREATE TABLE IF NOT EXISTS Session(
    Id UUID PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    Name VARCHAR NOT NULL,
    Description VARCHAR NOT NULL,
    StartDateTime DATE NOT NULL,
    EndDateTime DATE NOT NULL,
    LastModifiedDateTime DATE NOT NULL);
