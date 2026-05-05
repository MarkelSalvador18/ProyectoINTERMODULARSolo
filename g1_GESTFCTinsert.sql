------------------------------------------------------------
-- 0. BORRADO DE DATOS (orden correcto)
------------------------------------------------------------
DELETE FROM RA_TAREA;
DELETE FROM TAREA;
DELETE FROM JORNADA;
DELETE FROM RA;
DELETE FROM MODULO;
DELETE FROM ALUMNO;
DELETE FROM CICLO;

------------------------------------------------------------
-- 1. INSERT CICLO (PADRE PRINCIPAL)
------------------------------------------------------------
INSERT INTO CICLO (NOMBRE)
VALUES ('DAM'), ('DAW');

------------------------------------------------------------
-- 2. INSERT ALUMNO (depende de CICLO)
------------------------------------------------------------
INSERT INTO ALUMNO (DNI, TELEFONO, EMAIL, NOMBRE, APELLIDO1, APELLIDO2, CODIGOCICLO)
VALUES 
('12345678A','600600600','alumno1@correo.com','Iker','García','López',1),
('87654321B','611611611','alumno2@correo.com','Ane','Martínez','Soto',2);

------------------------------------------------------------
-- 3. INSERT MODULO (depende de CICLO)
------------------------------------------------------------
INSERT INTO MODULO (CODIGOMODULO, CODIGOCICLO, NOMBRE)
VALUES
(1,1,'Programación'),
(2,1,'Bases de Datos'),
(1,2,'Entorno Cliente');

------------------------------------------------------------
-- 4. INSERT RA (depende de MODULO)
------------------------------------------------------------
INSERT INTO RA (NUMERORA, CODIGOMODULO, CODIGOCICLO, NOMBRE, DESCRIPCION)
VALUES
(1,1,1,'RA1 Prog','Estructuras básicas'),
(2,1,1,'RA2 Prog','POO'),
(1,2,1,'RA1 BD','Modelo relacional'),
(1,1,2,'RA1 Cliente','DOM y eventos');

------------------------------------------------------------
-- 5. INSERT JORNADA (depende de ALUMNO)
------------------------------------------------------------
INSERT INTO JORNADA (FECHA, DNIALUMNO, HORAS, ESTADO)
VALUES
('2024-01-10','12345678A',5,'REALIZADA'),
('2024-01-11','12345678A',4,'PENDIENTE'),
('2024-01-10','87654321B',6,'REALIZADA');

------------------------------------------------------------
-- 6. INSERT TAREA (depende de JORNADA)
------------------------------------------------------------
INSERT INTO TAREA (CODIGOTAREA, DNIALUMNO, FECHAJORNADA, DESCRIPCION, HORAS)
VALUES
(1,'12345678A','2024-01-10','Tarea de programación',3),
(2,'12345678A','2024-01-11','Tarea de BD',2),
(1,'87654321B','2024-01-10','Tarea de cliente',4);

------------------------------------------------------------
-- 7. INSERT RA_TAREA (depende de TAREA y RA)
------------------------------------------------------------
INSERT INTO RA_TAREA (CODIGOTAREA, FECHAJORNADA, NUMERORA, CODIGOMODULO, CODIGOCICLO, DNIALUMNO)
VALUES
(1,'2024-01-10',1,1,1,'12345678A'),
(2,'2024-01-11',1,2,1,'12345678A'),
(1,'2024-01-10',1,1,2,'87654321B');
