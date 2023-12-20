-- -----------------------------------------------------
-- Schema
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `B1`;
USE `B1` ;


-- -----------------------------------------------------
-- Table `B1`.`Task_1`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`Task_1` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `date_timestamp` DATETIME(6) NOT NULL,
  `charset_eng` VARCHAR(45) NOT NULL,
  `charset_rus` VARCHAR(45) NOT NULL,
  `decimal_value` INT NOT NULL,
  `double_value` REAL NOT NULL,
  PRIMARY KEY (`id`));


-- -----------------------------------------------------
-- Table `B1`.`turnover_subheader`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`turnover_subheader` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `header` VARCHAR(127) NOT NULL,
  `column1` VARCHAR(63) NOT NULL,
  `column2` VARCHAR(63) NOT NULL,
  PRIMARY KEY (`id`));


-- -----------------------------------------------------
-- Table `B1`.`turnover_header`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`turnover_header` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `column1` VARCHAR(63) NOT NULL,
  `subheader1_id` INT NOT NULL,
  `subheader2_id` INT NOT NULL,
  `subheader3_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_turnover_header_subheader1_id`
    FOREIGN KEY (`subheader1_id`)
    REFERENCES `B1`.`turnover_subheader` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_turnover_header_subheader2_id`
    FOREIGN KEY (`subheader2_id`)
    REFERENCES `B1`.`turnover_subheader` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_turnover_header_subheader3_id`
    FOREIGN KEY (`subheader3_id`)
    REFERENCES `B1`.`turnover_subheader` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table `B1`.`turnover_row`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`turnover_row` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `idx` VARCHAR(10) NOT NULL,
  `InputBalanceActive` REAL NOT NULL,
  `InputBalancePassive` REAL NOT NULL,
  `DebitTurnover` REAL NOT NULL,
  `LoanTurnover` REAL NOT NULL,
  `OutputBalanceActive` REAL NOT NULL,
  `OutputBalancePassive` REAL NOT NULL,
  PRIMARY KEY (`id`));


-- -----------------------------------------------------
-- Table `B1`.`turnover_sheet`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`turnover_sheet` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `bank_name` VARCHAR(255) NOT NULL,
  `description` VARCHAR(255) NOT NULL,
  `currency` VARCHAR(45) NOT NULL,
  `sheet_date` DATETIME(6) NOT NULL,
  `turnover_header_id` INT NOT NULL,
  `total_by_sheet_row_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_turnover_sheet_header_id`
    FOREIGN KEY (`turnover_header_id`)
    REFERENCES `B1`.`turnover_header` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_turnover_sheet_row_id`
    FOREIGN KEY (`total_by_sheet_row_id`)
    REFERENCES `B1`.`turnover_row` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table `B1`.`turnover_table`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`turnover_table` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `sheet_id` INT NOT NULL,
  `class_name` VARCHAR(127) NOT NULL,
  `total_by_class_row_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_turnover_table_sheet_id`
    FOREIGN KEY (`sheet_id`)
    REFERENCES `B1`.`turnover_sheet` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_turnover_table_total_row_id`
    FOREIGN KEY (`total_by_class_row_id`)
    REFERENCES `B1`.`turnover_row` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table `B1`.`turnover_group`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `B1`.`turnover_group` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `table_id` INT NOT NULL,
  `group_idx` VARCHAR(10) NOT NULL,
  `row_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_turnover_group_table_id`
    FOREIGN KEY (`table_id`)
    REFERENCES `B1`.`turnover_table` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_turnover_group_row_id`
    FOREIGN KEY (`row_id`)
    REFERENCES `B1`.`turnover_row` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
