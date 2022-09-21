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
  `fechaInicio` date NOT NULL,
  `fechaFinal` date NOT NULL,
  `alquilerMensual` double NOT NULL,
  `inmuebleId` int(11) NOT NULL,
  `inquilinoId` int(11) NOT NULL,
  PRIMARY KEY (`idContrato`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contrato`
--

LOCK TABLES `contrato` WRITE;
/*!40000 ALTER TABLE `contrato` DISABLE KEYS */;
INSERT INTO `contrato` VALUES (1,'2022-09-10','2022-09-30',75000,2,2),(2,'2022-09-10','2022-09-17',133000,1,1);
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
  PRIMARY KEY (`idInmueble`),
  KEY `propietarioId` (`propietarioId`),
  CONSTRAINT `propietarioId` FOREIGN KEY (`propietarioId`) REFERENCES `propietario` (`idPropietario`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (1,'alguno',1,1,3,'-11.55, -22.23',23500,1,7),(2,'Alguna calle 1500',1,2,3,'-33.302134,-66.336876',27500,1,2),(3,'No se al 2500',2,4,4,'-37.375534,86.375576',38500,0,12);
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (1,'Inquilino Editado','Base','005','2544','inquilino1@npc'),(2,'Creado','Inquilino','555789','279','inquilino2nuevo@npc'),(3,'Juancho','Rivera','333','533','juncholomo@gg'),(4,'Alguno','Extra','258','789999','nose@mail'),(5,'Alejandro','Farro','55345','7855','alej@mail.com'),(6,'Jessica','Farro','2344432','234243','alej@mail');
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
  `fechaPago` datetime NOT NULL,
  `importe` double NOT NULL,
  `contratoId` int(11) NOT NULL,
  `detalle` varchar(150) NOT NULL,
  PRIMARY KEY (`numPago`),
  KEY `contratoId` (`contratoId`),
  CONSTRAINT `contratoId` FOREIGN KEY (`contratoId`) REFERENCES `contrato` (`idContrato`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
INSERT INTO `pago` VALUES (1,'2022-09-13 21:46:20',35000,1,'1er pago'),(2,'2022-09-13 21:46:58',175000,2,'1er pago');
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
  PRIMARY KEY (`idPropietario`),
  UNIQUE KEY `DNI` (`DNI`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (1,'Desde','Base','000','1234','asd@dsa'),(2,'Leandro','Heredia','396','2664','lh@gmail'),(7,'Angel','Gomez','4433','122223','lkj@asdh'),(11,'Laila','Medina','5556','2344443','qweqwe@eeee'),(12,'Anasthasia','Nevus','4125','0297','agnes@mail.com');
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Primera','Prueba','prueba6@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=','',1),(2,'Leandro','Heredia','lea96_2@mail.com','ObW08/YU1fXqk2FnT8ij87FiVNlXqfOqWNNR6xp6Kt0=','/Uploads\\avatar_2.jpg',1),(3,'Pruebas 3','Misticas Total','pruebas5@mail.com','ihAn709rtu2hggMiPeareRwooaH0dTW/PpEqOpqDjOU=','/Uploads\\avatar_0.png',2),(4,'Nuevo','Usuario','nuevo@mail.com','hEgoz6JiWh9RH8ILe+x0bnK2KinsWx16rTwBq3LSAY0=','/Uploads\\avatar_0.jpg',2);
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

-- Dump completed on 2022-09-20 21:47:31
