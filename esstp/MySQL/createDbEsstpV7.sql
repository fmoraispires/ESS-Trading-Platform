-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema dbEsstpV7
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema dbEsstpV7
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `dbEsstpV7` DEFAULT CHARACTER SET latin1 ;
USE `dbEsstpV7` ;

-- -----------------------------------------------------
-- Table `dbEsstpV7`.`asset`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`asset` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`cfd`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`cfd` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`market`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`market` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `buying` DOUBLE NULL DEFAULT NULL,
  `selling` DOUBLE NULL DEFAULT NULL,
  `isin` INT(11) NULL DEFAULT NULL,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  `symbol` VARCHAR(255) NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `asset_id` INT(11) NOT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_market_asset1_idx` (`asset_id` ASC),
  CONSTRAINT `fk_market_asset1`
    FOREIGN KEY (`asset_id`)
    REFERENCES `dbEsstpV7`.`asset` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`operationType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`operationType` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`role` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `roleName` VARCHAR(255) NULL DEFAULT NULL,
  `roleValue` VARCHAR(255) NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`user` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `userName` VARCHAR(255) NULL DEFAULT NULL,
  `email` VARCHAR(255) NULL DEFAULT NULL,
  `nif` INT(11) NULL DEFAULT NULL,
  `passwordHash` VARBINARY(64) NULL DEFAULT NULL,
  `passwordSalt` VARBINARY(128) NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `role_id` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_user_role1_idx` (`role_id` ASC),
  CONSTRAINT `fk_user_role1`
    FOREIGN KEY (`role_id`)
    REFERENCES `dbEsstpV7`.`role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`portfolio`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`portfolio` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `state` BIT(1) NULL DEFAULT NULL,
  `action` VARCHAR(255) NULL DEFAULT NULL,
  `valueOpen` DOUBLE NULL DEFAULT NULL,
  `valueClosed` DOUBLE NULL DEFAULT NULL,
  `invested` DOUBLE NULL DEFAULT NULL,
  `stopLoss` DOUBLE NULL DEFAULT NULL,
  `takeProfit` DOUBLE NULL DEFAULT NULL,
  `units` DOUBLE NULL DEFAULT NULL,
  `brokerMargin` DOUBLE NULL DEFAULT NULL,
  `profit` DOUBLE NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  `isInactive` INT(11) NULL DEFAULT NULL,
  `cfd_id` INT(11) NOT NULL,
  `market_id` INT(11) NOT NULL,
  `user_id` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_portfolio_cfd1_idx` (`cfd_id` ASC),
  INDEX `fk_portfolio_market1_idx` (`market_id` ASC),
  INDEX `fk_portfolio_user1_idx` (`user_id` ASC),
  CONSTRAINT `fk_portfolio_cfd1`
    FOREIGN KEY (`cfd_id`)
    REFERENCES `dbEsstpV7`.`cfd` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_portfolio_market1`
    FOREIGN KEY (`market_id`)
    REFERENCES `dbEsstpV7`.`market` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_portfolio_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `dbEsstpV7`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`operation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`operation` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `action` VARCHAR(255) NULL DEFAULT NULL,
  `amount` DOUBLE NULL DEFAULT NULL,
  `timestamp` DATETIME NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `operationType_id` INT(11) NOT NULL,
  `portfolio_id` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_operation_operationType1_idx` (`operationType_id` ASC),
  INDEX `fk_operation_portfolio1_idx` (`portfolio_id` ASC),
  CONSTRAINT `fk_operation_operationType1`
    FOREIGN KEY (`operationType_id`)
    REFERENCES `dbEsstpV7`.`operationType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_operation_portfolio1`
    FOREIGN KEY (`portfolio_id`)
    REFERENCES `dbEsstpV7`.`portfolio` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `dbEsstpV7`.`permission`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `dbEsstpV7`.`permission` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `permissionName` VARCHAR(255) NULL DEFAULT NULL,
  `permissionValue` VARCHAR(255) NULL DEFAULT NULL,
  `permissionType` VARCHAR(255) NULL DEFAULT NULL,
  `discriminator` VARCHAR(255) NULL DEFAULT NULL,
  `createdBy` INT(11) NULL DEFAULT NULL,
  `createdDate` DATETIME NULL DEFAULT NULL,
  `updatedBy` INT(11) NULL DEFAULT NULL,
  `updatedDate` DATETIME NULL DEFAULT NULL,
  `isInactive` BIT(1) NULL DEFAULT NULL,
  `role_id` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_permission_role1_idx` (`role_id` ASC),
  CONSTRAINT `fk_permission_role1`
    FOREIGN KEY (`role_id`)
    REFERENCES `dbEsstpV7`.`role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
