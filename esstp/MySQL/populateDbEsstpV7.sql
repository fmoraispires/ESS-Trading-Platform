-- populate with data sql script for database dbfrendauthv5
-- workbench 8.0.15
-- mysql community server 5.7.25
-- ESS Trading Platform
-- populate database v4
-- Author Francisco Morais, pg10293
-- Date 22-11-2019
-- this script populates initial data on the tables
-- (1) data for the personalization sub-schema
-- (2) data for the access sub-schema
-- (3) data for the core sub-schema
-- (4) data for the connection sub-schema
-- 1 role (3)
INSERT INTO `dbEsstpV7`.`role` (`roleName`,`roleValue`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`,`isInactive`) VALUES ('System Administrator','Admin',1,now(),1,now(),0);
INSERT INTO `dbEsstpV7`.`role` (`roleName`,`roleValue`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`,`isInactive`) VALUES ('Regular User','Regular',1,now(),1,now(),0);
-- 2 user (1)
INSERT INTO `dbEsstpV7`.`user` (`userName`,`email`,`nif`,`passwordHash`,`passwordSalt`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`,`isInactive`,`role_id`) VALUES ('superadmin','esstp.sa@gmail.com','123456789','','',1,now(),1,now(),0,1);
-- 3 permission (1)
-- role admin
-- permissions dbEsstpV7
-- 4 asset
INSERT INTO `dbEsstpV7`.`asset` (`name`,`isInactive`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`) VALUES ('Commodities',0,1,now(),1,now());
INSERT INTO `dbEsstpV7`.`asset` (`name`,`isInactive`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`) VALUES ('Stocks',0,1,now(),1,now());
-- 5 operationType
INSERT INTO `dbEsstpV7`.`operationType`(`name`,`isInactive`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`) VALUES ('Debt',0,1,now(),1,now());
INSERT INTO `dbEsstpV7`.`operationType`(`name`,`isInactive`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`) VALUES ('Credit',0,1,now(),1,now());
-- 6 cfd
INSERT INTO `dbEsstpV7`.`cfd`(`name`,`isInactive`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`) VALUES ('Buy',0,1,now(),1,now());
INSERT INTO `dbEsstpV7`.`cfd`(`name`,`isInactive`,`createdBy`,`createdDate`,`updatedBy`,`updatedDate`) VALUES ('Sell',0,1,now(),1,now());

