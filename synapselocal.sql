-- MySQL dump 10.13  Distrib 8.0.17, for Win64 (x86_64)
--
-- Host: localhost    Database: aggs
-- ------------------------------------------------------
-- Server version	8.0.17

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('00000000000000_CreateIdentitySchema','2.2.6-servicing-10079');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('1e6570a1-c310-4793-91fd-8f14db7e23fb','Teacher','TEACHER','8d6485b6-d8bb-40d2-ab1b-11b07daa44fd'),('aeb2ab58-d2c1-4e1e-b7f3-e27d57e8476c','Student','STUDENT','91972a45-b123-4ba2-8b0b-68366db76fa0'),('e5544f6c-32db-47df-8529-903e8c37fde4','Admin','ADMIN','37f4075d-6f1b-4ce9-9a27-4c2461eb8673');
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('c306c84a-6f03-41ca-b4c2-57ceecb0550c','1e6570a1-c310-4793-91fd-8f14db7e23fb'),('01bf3742-be86-4301-b9a1-7ab7fdf05544','aeb2ab58-d2c1-4e1e-b7f3-e27d57e8476c'),('19f0a0cc-272b-40e1-88bd-c29d27f83e63','e5544f6c-32db-47df-8529-903e8c37fde4'),('9b8c5a6d-2efd-4798-99cc-19a2812b8a84','e5544f6c-32db-47df-8529-903e8c37fde4');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('01bf3742-be86-4301-b9a1-7ab7fdf05544','andrew.britt57@k12.leanderisd.org','ANDREW.BRITT57@K12.LEANDERISD.ORG','andrew.britt57@k12.leanderisd.org','ANDREW.BRITT57@K12.LEANDERISD.ORG',_binary '\0','AQAAAAEAACcQAAAAEG//19q2NW6y+8waJ8iyxhu2bK5czTqhwUeX6QY1NW6x9L9VyG72uz4P4nwLFfWSuA==','ZVBE2WCZCU2VF7YMOOMGMERSQNA7VUOB','cf4d998c-fa57-4de0-b82c-a5dc552fb351',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('19f0a0cc-272b-40e1-88bd-c29d27f83e63','drewbritt02@gmail.com','DREWBRITT02@GMAIL.COM','drewbritt02@gmail.com','DREWBRITT02@GMAIL.COM',_binary '\0','AQAAAAEAACcQAAAAELzKlyKICLyczHoCeP22A0BUznUtvQfhAsRLYON3oJmyrPED4Vn4oKytmK4wHsar2A==','TR2M3HSS6REJMDZE4EZJ7VRHGFW7GA4M','ad79366a-51c4-49b5-849a-32fbdb8816c8',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('9b8c5a6d-2efd-4798-99cc-19a2812b8a84','lyonjenkins@gmail.com','LYONJENKINS@GMAIL.COM','lyonjenkins@gmail.com','LYONJENKINS@GMAIL.COM',_binary '\0','AQAAAAEAACcQAAAAEB5AB8W5RPo7l7tsxqhZ5Jqc1P6UmojH0ar/d5Kwb850xfBPTiy+by7oSygPajLiAw==','LE5C2TW3PQ4BNFXX2PS3QRSPKKJC6O47','28a97a95-9eeb-4724-a4a4-90e16bbe9776',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('c306c84a-6f03-41ca-b4c2-57ceecb0550c','teacheraccount@gmail.com','TEACHERACCOUNT@GMAIL.COM','teacheraccount@gmail.com','TEACHERACCOUNT@GMAIL.COM',_binary '\0','AQAAAAEAACcQAAAAEMTl3/AuhSyp6XvHIJomp7keaqiAZCaFP6Zzey1KuYwi565pDl4oLjnnfOc02GTK+g==','KL34MA7KQ5LPVVLXSGIL4K6DY5ZNAZAQ','2b80e6f9-d43c-4e18-a847-f7478db431ab',NULL,_binary '\0',_binary '\0',NULL,_binary '',0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
INSERT INTO `aspnetusertokens` VALUES ('19f0a0cc-272b-40e1-88bd-c29d27f83e63','[AspNetUserStore]','AuthenticatorKey','TDSQDK2USEZDX4W3ZICTLPHH6BZM3H7M');
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `assignmentcategories`
--

DROP TABLE IF EXISTS `assignmentcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `assignmentcategories` (
  `categoryid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `teacherid` mediumint(9) NOT NULL,
  `categoryname` varchar(50) NOT NULL,
  `categoryweight` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`categoryid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assignmentcategories`
--

LOCK TABLES `assignmentcategories` WRITE;
/*!40000 ALTER TABLE `assignmentcategories` DISABLE KEYS */;
INSERT INTO `assignmentcategories` VALUES (1,2,'Tests',50),(2,2,'Daily Work',50);
/*!40000 ALTER TABLE `assignmentcategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `assignments`
--

DROP TABLE IF EXISTS `assignments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `assignments` (
  `assignmentid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `classid` mediumint(9) NOT NULL,
  `assignmentname` varchar(100) NOT NULL,
  `categoryid` mediumint(9) NOT NULL,
  PRIMARY KEY (`assignmentid`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assignments`
--

LOCK TABLES `assignments` WRITE;
/*!40000 ALTER TABLE `assignments` DISABLE KEYS */;
INSERT INTO `assignments` VALUES (1,5,'Java Test 3',1),(2,5,'Java Practice 4',2),(3,5,'Java Test 4',1),(4,5,'Test 1',1),(5,5,'Test 2',1);
/*!40000 ALTER TABLE `assignments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attendances`
--

DROP TABLE IF EXISTS `attendances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendances` (
  `datetaken` date NOT NULL,
  `studentid` mediumint(9) NOT NULL,
  `classid` mediumint(9) NOT NULL,
  `attendancevalue` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`datetaken`,`studentid`,`classid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendances`
--

LOCK TABLES `attendances` WRITE;
/*!40000 ALTER TABLE `attendances` DISABLE KEYS */;
/*!40000 ALTER TABLE `attendances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classes`
--

DROP TABLE IF EXISTS `classes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `classes` (
  `classid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `teacherid` mediumint(9) NOT NULL,
  `classname` varchar(50) NOT NULL,
  `period` varchar(10) NOT NULL,
  `location` varchar(20) NOT NULL,
  PRIMARY KEY (`classid`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classes`
--

LOCK TABLES `classes` WRITE;
/*!40000 ALTER TABLE `classes` DISABLE KEYS */;
INSERT INTO `classes` VALUES (1,1,'Wind Ensemble 1','1st','Band Hall'),(2,1,'Wind Ensemble 2','1st','Band Hall'),(3,1,'Wind Ensemble 3','1st','Band Hall'),(4,1,'Wind Ensemble 4','1st','Band Hall'),(5,2,'AP Computer Science','6th','2417'),(6,2,'Computer Science Pre-AP','5th','2417'),(7,2,'Computer Science 3','6th','2414'),(8,3,'English 2 Pre-AP','4th','1023'),(9,3,'English 2 Pre-AP','8th','1412'),(10,3,'English 3 AP','7th','1412'),(11,4,'AP Microeconomics','2nd','1206'),(12,4,'AP Microeconomics','4th','1206'),(13,1,'World History','4th','1208'),(14,5,'AP World History','7th','1208'),(15,6,'AP World History','7th','1207'),(16,7,'Dance 1','1st','Dance Gym'),(17,7,'Dance 2','1st','Dance Gym');
/*!40000 ALTER TABLE `classes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grades`
--

DROP TABLE IF EXISTS `grades`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grades` (
  `gradeid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `assignmentid` mediumint(9) NOT NULL,
  `classid` mediumint(9) NOT NULL,
  `studentid` mediumint(9) NOT NULL,
  `gradevalue` varchar(3) NOT NULL,
  PRIMARY KEY (`gradeid`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grades`
--

LOCK TABLES `grades` WRITE;
/*!40000 ALTER TABLE `grades` DISABLE KEYS */;
INSERT INTO `grades` VALUES (1,1,5,1,'t'),(2,2,5,1,'t'),(3,3,5,1,'t'),(4,1,5,3,'t'),(5,2,5,3,'t'),(6,3,5,3,'t'),(7,1,5,2,'t'),(8,2,5,2,'t'),(9,3,5,2,'t'),(10,1,5,4,'t'),(11,2,5,4,'t'),(12,3,5,4,'t'),(13,1,5,5,'t'),(14,2,5,5,'t'),(15,3,5,5,'t'),(16,1,5,6,'t'),(17,2,5,6,'t'),(18,3,5,6,'t'),(19,4,5,1,'t'),(20,5,5,2,'t'),(21,4,5,3,'t'),(22,5,5,3,'t'),(23,4,5,5,'t'),(24,5,5,5,'b'),(25,4,5,6,'b'),(26,5,5,6,'t'),(27,4,5,1,'t'),(28,5,5,2,'t'),(29,4,5,4,'t'),(30,5,5,4,'t');
/*!40000 ALTER TABLE `grades` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `periods`
--

DROP TABLE IF EXISTS `periods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `periods` (
  `periodid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `periodname` varchar(20) NOT NULL,
  `periodnumber` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`periodid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `periods`
--

LOCK TABLES `periods` WRITE;
/*!40000 ALTER TABLE `periods` DISABLE KEYS */;
/*!40000 ALTER TABLE `periods` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `referrals`
--

DROP TABLE IF EXISTS `referrals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `referrals` (
  `referralid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `studentid` mediumint(9) NOT NULL,
  `teacherid` mediumint(9) NOT NULL,
  `dateissued` date NOT NULL,
  `description` varchar(100) NOT NULL,
  `handled` bit(1) NOT NULL,
  PRIMARY KEY (`referralid`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `referrals`
--

LOCK TABLES `referrals` WRITE;
/*!40000 ALTER TABLE `referrals` DISABLE KEYS */;
INSERT INTO `referrals` VALUES (1,4,2,'2019-09-07','Existed.',_binary ''),(2,6,5,'2019-09-07','Made too good of music.',_binary ''),(3,6,5,'2019-09-05','Beat another kid with his drumsticks.',_binary ''),(4,9,3,'2019-08-31','Test Description.',_binary ''),(5,10,1,'2019-09-03','Michael sux lol.',_binary '');
/*!40000 ALTER TABLE `referrals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `studentid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `studentfirstname` varchar(50) NOT NULL,
  `studentlastname` varchar(50) NOT NULL,
  `email` varchar(150) NOT NULL,
  `gradelevel` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`studentid`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES (1,'Andrew','Britt','andrew.britt57@k12.leanderisd.org',11),(2,'Lyon','Jenkins','lyon.jenkins16@k12.leanderisd.org',11),(3,'Michelle','Bickett','michelle.bickett17@k12.leanderisd.org',11),(4,'Billy','Joel','billy.joel81@k12.leanderisd.org',9),(5,'Robert','Plant','robert.plant36@k12.leanderisd.org',12),(6,'John','Bonham','john.bonham28@k12.leanderisd.org',12),(7,'Jimmy','Page','jimmy.page55@k12.leanderisd.org',12),(8,'John','Jones','john.jones16@k12.leanderisd.org',12),(9,'Gwen','Stefani','gwen.stefani39@k12.leanderisd.org',10),(10,'Michael','Aguayo','michael.aguayo72@k12.leanderisd.org',12);
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `studentsclasses`
--

DROP TABLE IF EXISTS `studentsclasses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `studentsclasses` (
  `studentid` mediumint(9) NOT NULL,
  `classid` mediumint(9) NOT NULL,
  PRIMARY KEY (`studentid`,`classid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `studentsclasses`
--

LOCK TABLES `studentsclasses` WRITE;
/*!40000 ALTER TABLE `studentsclasses` DISABLE KEYS */;
INSERT INTO `studentsclasses` VALUES (1,1),(1,5),(1,8),(1,14),(2,1),(2,4),(2,5),(3,5),(3,7),(3,15),(3,17),(4,5),(5,5),(6,5),(6,13),(6,15),(6,16);
/*!40000 ALTER TABLE `studentsclasses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teachers`
--

DROP TABLE IF EXISTS `teachers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `teachers` (
  `teacherid` mediumint(9) NOT NULL AUTO_INCREMENT,
  `teacherfirstname` varchar(50) NOT NULL,
  `teacherlastname` varchar(50) NOT NULL,
  `email` varchar(150) NOT NULL,
  PRIMARY KEY (`teacherid`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teachers`
--

LOCK TABLES `teachers` WRITE;
/*!40000 ALTER TABLE `teachers` DISABLE KEYS */;
INSERT INTO `teachers` VALUES (1,'Amy','Suggs','amy.suggs@leanderisd.org'),(2,'Daniel','Nawrocki','teacheraccount@gmail.com'),(3,'Zachary','Long','zachary.long@leanderisd.org'),(4,'Uncle','Sam','uncle.sam@leanderisd.org'),(5,'Grant','Britton','grant.britton@leanderisd.org'),(6,'Matt','Riley','matt.riley@leanderisd.org'),(7,'Katy','Reeves','katy.reeves@leanderisd.org');
/*!40000 ALTER TABLE `teachers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'aggs'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-10-04 11:19:17
