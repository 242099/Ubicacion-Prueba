USE AppUbicacionesDB;
GO

CREATE TABLE Ubicaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Latitud FLOAT NOT NULL,
    Longitud FLOAT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

Select * From Ubicaciones;