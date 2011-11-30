CREATE TABLE `entry_type` (
  `entry_type_id` int(11) NOT NULL,
  `entry_type` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`entry_type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `main_entry` (
  `main_entry_id` int(11) NOT NULL AUTO_INCREMENT,
  `entry_type_id` int(11) NOT NULL,
  `entry_name` varchar(500) DEFAULT NULL,
  `profile_url` varchar(500) DEFAULT NULL,
  `entry_job_title` varchar(500) DEFAULT NULL,
  `entry_location` varchar(500) DEFAULT NULL,
  `entry_region` varchar(500) DEFAULT NULL,
  `scrape_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `semi_known_main_entry_id` int(11) DEFAULT NULL,
  `shares_groups` int(11) DEFAULT NULL,
  `shares_connections` int(11) DEFAULT NULL,
  PRIMARY KEY (`main_entry_id`),
  KEY `entry_type_id` (`entry_type_id`),
  CONSTRAINT `main_entry_ibfk_1` FOREIGN KEY (`entry_type_id`) REFERENCES `entry_type` (`entry_type_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=14281 DEFAULT CHARSET=latin1;

# ************************************************************
# Sequel Pro SQL dump
# Version 3408
#
# http://www.sequelpro.com/
# http://code.google.com/p/sequel-pro/
#
# Host: 192.168.2.101 (MySQL 5.5.13-log)
# Database: linkedin_miner
# Generation Time: 2011-11-30 06:22:37 +0000
# ************************************************************


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table entry_type
# ------------------------------------------------------------

DROP TABLE IF EXISTS `entry_type`;

CREATE TABLE `entry_type` (
  `entry_type_id` int(11) NOT NULL,
  `entry_type` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`entry_type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

LOCK TABLES `entry_type` WRITE;
/*!40000 ALTER TABLE `entry_type` DISABLE KEYS */;

INSERT INTO `entry_type` (`entry_type_id`, `entry_type`)
VALUES
	(1,'Anonymous'),
	(2,'Semi-known'),
	(3,'Identified');

/*!40000 ALTER TABLE `entry_type` ENABLE KEYS */;
UNLOCK TABLES;



/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
