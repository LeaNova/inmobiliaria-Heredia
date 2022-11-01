-- MariaDB dump 10.19  Distrib 10.4.24-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: inmobiliaria_heredia
-- ------------------------------------------------------
-- Server version	10.4.24-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `contrato`
--

DROP TABLE IF EXISTS `contrato`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contrato` (
  `idContrato` int(11) NOT NULL AUTO_INCREMENT,
  `fechaInicio` varchar(20) NOT NULL,
  `fechaFinal` varchar(20) NOT NULL,
  `alquilerMensual` double NOT NULL,
  `inmuebleId` int(11) NOT NULL,
  `inquilinoId` int(11) NOT NULL,
  PRIMARY KEY (`idContrato`),
  KEY `inmuebleId` (`inmuebleId`),
  KEY `inquilinoId` (`inquilinoId`),
  CONSTRAINT `inmuebleId` FOREIGN KEY (`inmuebleId`) REFERENCES `inmueble` (`idInmueble`),
  CONSTRAINT `inquilinoId` FOREIGN KEY (`inquilinoId`) REFERENCES `inquilino` (`idInquilino`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contrato`
--

LOCK TABLES `contrato` WRITE;
/*!40000 ALTER TABLE `contrato` DISABLE KEYS */;
INSERT INTO `contrato` VALUES (1,'2022-09-10','2022-09-21',75000,2,2),(2,'2022-09-10','2022-09-21',133000,3,5),(4,'2022-09-01','2022-09-25',100000,6,7),(5,'2022-12-31','2023-03-27',45000,6,6),(8,'2022-10-05','2022-09-29',50000,7,6),(9,'2022-11-01','2022-11-30',50000,7,2);
/*!40000 ALTER TABLE `contrato` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inmueble` (
  `idInmueble` int(11) NOT NULL AUTO_INCREMENT,
  `direccion` varchar(60) NOT NULL,
  `uso` int(11) NOT NULL,
  `tipo` int(11) NOT NULL,
  `cantAmbientes` int(11) NOT NULL,
  `coordenadas` varchar(30) NOT NULL,
  `precio` double NOT NULL,
  `disponible` tinyint(1) NOT NULL,
  `propietarioId` int(11) NOT NULL,
  `foto` varchar(50) NOT NULL,
  PRIMARY KEY (`idInmueble`),
  KEY `propietarioId` (`propietarioId`),
  CONSTRAINT `propietarioId` FOREIGN KEY (`propietarioId`) REFERENCES `propietario` (`idPropietario`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (1,'San calle 175',1,1,3,'-11.55, -22.23',23500,1,7,''),(2,'Alguna calle 1500',1,2,3,'-33.302134,-66.336876',27500,1,2,''),(3,'No se al 2500',2,4,4,'-37.375534,86.375576',38500,0,12,'/Uploads\\inmueble_12142044.png'),(5,'Calle tripiante 1780',1,2,4,'-33.30001,-66.112276',48500,0,2,''),(6,'Calle comercial 156',2,5,7,'-32.55,-67.44',100000,1,1,''),(7,'Sucre 123',1,1,2,'-33.55, -66.45',50000,1,18,''),(8,'San martin 1920',1,4,3,'-33.59, -66.55',75500,1,12,'/Uploads\\inmueble_12405546.png'),(15,'Vientos del Moye 155',1,1,3,'-33.282314,-66.245125',46500,1,12,'/Uploads\\inmueble_12525913.png'),(17,'Calle del Double Exception',2,4,6,'-33.2797,-66.311134',150000,0,12,'/Uploads\\inmueble_12700419.png'),(18,'Avenida del Carmen 173',1,6,1,'-33.283707,-66.31337',55000,1,12,'/Uploads\\inmueble_12215411.png'),(20,'Calle nueva 155',2,3,3,'-33.289963,-66.30687',75200,1,12,'/Uploads\\inmueble_1264422.png');
/*!40000 ALTER TABLE `inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inquilino` (
  `idInquilino` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `DNI` varchar(9) NOT NULL,
  `telefono` varchar(15) NOT NULL,
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`idInquilino`),
  UNIQUE KEY `DNI` (`DNI`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (2,'Esteban','Herrera','555789','279','este_herre@mail'),(5,'Alejandro','Farro','55345','7855','alej@mail.com'),(6,'Jessica','Farro','2344432','234243','alej@mail'),(7,'Ailen','Hill','123333','223133','nose@gg'),(9,'Valeria Maria','Padula','12342','11222','maria_p@mail');
/*!40000 ALTER TABLE `inquilino` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pago` (
  `numPago` int(11) NOT NULL AUTO_INCREMENT,
  `fechaPago` varchar(20) NOT NULL,
  `importe` double NOT NULL,
  `contratoId` int(11) NOT NULL,
  `detalle` varchar(150) NOT NULL,
  PRIMARY KEY (`numPago`),
  KEY `contratoId` (`contratoId`),
  CONSTRAINT `contratoId` FOREIGN KEY (`contratoId`) REFERENCES `contrato` (`idContrato`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
INSERT INTO `pago` VALUES (1,'2022-09-13',35000,1,'1er pago'),(2,'2022-09-13',105000,2,'1er pago'),(4,'2022-09-25',100000,4,'Multa: importe de 1 (un) meses de alquiler'),(5,'2022-09-25',45000,5,'2do pago'),(6,'2022-09-29',50000,8,'Primer pago'),(7,'2022-09-29',100000,8,'Multa: importe de 2 (dos) meses de alquiler');
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietario`
--

DROP TABLE IF EXISTS `propietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `propietario` (
  `idPropietario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `DNI` varchar(9) NOT NULL,
  `telefono` varchar(15) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `pass` varchar(60) NOT NULL,
  `foto` varchar(50) NOT NULL,
  PRIMARY KEY (`idPropietario`),
  UNIQUE KEY `DNI` (`DNI`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (1,'Mateo Raiden','Nevus','12500','155455','mateo@mail.com','utFsenRjgHDLVTjCOzD8PiHb1Y7VGPMKTlO/Lf80DSQ=',''),(2,'Leandro','Heredia','396','2664','lh@mail.ccom','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=',''),(7,'Angel','Gomez','4433','122223','lkj@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=',''),(12,'Anasthasia Agnes','Nevus','4125','0297','agnes@mail.com','P5wgYSvMaRwHnlFDd5ow8BZiYsrqwkCtrbbf7xOa0lQ=','/Uploads\\propietario_12.png'),(15,'Matias','Bravo','1223','02665','elmati66@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=',''),(17,'Laila','Medina','555','333','laila@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=',''),(18,'Mariano','Luzza','654987','546798','mluzza@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=','');
/*!40000 ALTER TABLE `propietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `user` varchar(50) NOT NULL,
  `pass` varchar(60) NOT NULL,
  `avatar` varchar(100) DEFAULT NULL,
  `access` int(11) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  UNIQUE KEY `user` (`user`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Primera','Prueba','prueba6@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=','',1),(2,'Leandro','Heredia','lea96_2@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=','/Uploads\\avatar_2.jpg',1),(6,'Carmen','Garcia','car_@mail','5/3ooccGuFkruCRT8Ec2EuairkxdDT/noDPYsapC0H4=','/Uploads\\avatar_6.jpg',2),(7,'Amalia','Moyano','amo@mail.com','hEgoz6JiWh9RH8ILe+x0bnK2KinsWx16rTwBq3LSAY0=',NULL,2);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-10-31 21:21:37
