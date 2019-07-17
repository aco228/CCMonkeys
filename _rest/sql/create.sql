START TRANSACTION;
SET FOREIGN_KEY_CHECKS = 0; 

CREATE TABLE `cc_postback` (
  `postbackid` int(11) NOT NULL AUTO_INCREMENT,
  `providerid` int(11) NOT NULL,
  `trackingid` varchar(45) DEFAULT NULL,
  `actionid` int(11) DEFAULT NULL,
  `url` varchar(2048) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`postbackid`),
  KEY `providerid` (`providerid`),
  KEY `trackingid_indx` (`trackingid`),
  KEY `actionid_index` (`actionid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `cc_postback_log` (
  `postbacklog` int(11) NOT NULL AUTO_INCREMENT,
  `postbackid` int(11) NOT NULL,
  `text` varchar(2048) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`postbacklog`),
  KEY `postbackid_indx` (`postbackid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `cc_undercover` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `affid` varchar(11) NOT NULL,
  `pubid` varchar(11) CHARACTER SET utf8 DEFAULT NULL,
  `value` int(11) NOT NULL DEFAULT '0',
  `transactions` int(11) NOT NULL DEFAULT '0',
  `tcost` decimal(5,2) DEFAULT NULL,
  `undercoverTransactions` int(11) NOT NULL DEFAULT '0',
  `currentDay` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created` varchar(45) NOT NULL DEFAULT 'CURRENT_TIMESTAMP',
  PRIMARY KEY (`id`),
  KEY `affid_indx` (`affid`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_action` (
  `actionid` int(11) NOT NULL AUTO_INCREMENT,
  `guid` varchar(36) NOT NULL DEFAULT '',
  `trackingid` varchar(45) DEFAULT NULL,
  `userid` int(11) NOT NULL,
  `leadid` int(11) DEFAULT NULL,
  `affid` varchar(45) DEFAULT NULL,
  `pubid` varchar(45) DEFAULT NULL,
  `prelandertypeid` int(11) DEFAULT NULL,
  `prelanderid` int(11) DEFAULT NULL,
  `landerid` int(11) DEFAULT NULL,
  `landertypeid` int(11) DEFAULT NULL,
  `providerid` int(11) DEFAULT NULL,
  `countryid` int(11) DEFAULT NULL,
  `input_redirect` tinyint(1) NOT NULL DEFAULT '0',
  `input_email` tinyint(1) NOT NULL DEFAULT '0',
  `input_contact` tinyint(1) NOT NULL DEFAULT '0',
  `has_subscription` tinyint(1) NOT NULL DEFAULT '0',
  `has_chargeback` tinyint(1) NOT NULL DEFAULT '0',
  `has_refund` tinyint(1) NOT NULL DEFAULT '0',
  `times_charged` int(1) NOT NULL DEFAULT '0',
  `times_upsell` int(1) NOT NULL DEFAULT '0',
  `has_redirectedToProvider` tinyint(1) NOT NULL DEFAULT '0',
  `has_stolen` tinyint(1) NOT NULL DEFAULT '0',
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`actionid`),
  UNIQUE KEY `guid_UNIQUE` (`guid`),
  KEY `leadid_idx` (`leadid`),
  KEY `prelandertypeid_idx` (`prelandertypeid`),
  KEY `prelanderid_idx` (`prelanderid`),
  KEY `landerid_idx` (`landerid`),
  KEY `providerid_idx` (`providerid`),
  KEY `countryid_idx` (`countryid`),
  KEY `landertypeid_idx` (`landertypeid`),
  KEY `userid_idx` (`userid`),
  KEY `trackingid_idx` (`trackingid`),
  KEY `trackingid` (`trackingid`),
  KEY `affid` (`affid`),
  CONSTRAINT `countryid` FOREIGN KEY (`countryid`) REFERENCES `tm_country` (`countryid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `landerid` FOREIGN KEY (`landerid`) REFERENCES `tm_lander` (`landerid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `landertypeid` FOREIGN KEY (`landertypeid`) REFERENCES `tm_landertype` (`landertypeid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `leadid` FOREIGN KEY (`leadid`) REFERENCES `tm_lead` (`leadid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `prelanderid` FOREIGN KEY (`prelanderid`) REFERENCES `tm_prelander` (`prelanderid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `prelandertypeid` FOREIGN KEY (`prelandertypeid`) REFERENCES `tm_prelandertype` (`prelandertypeid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `providerid` FOREIGN KEY (`providerid`) REFERENCES `tm_provider` (`providerid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_action_account` (
  `actionaccount` int(11) NOT NULL AUTO_INCREMENT,
  `actionid` int(11) NOT NULL,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`actionaccount`),
  KEY `actionid_index` (`actionid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_country` (
  `countryid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(90) NOT NULL,
  `code` varchar(2) NOT NULL,
  PRIMARY KEY (`countryid`)
) ENGINE=InnoDB AUTO_INCREMENT=238 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_country_used` (
  `countryusedid` int(11) NOT NULL AUTO_INCREMENT,
  `countryid` int(11) NOT NULL,
  PRIMARY KEY (`countryusedid`),
  KEY `countryid_idx` (`countryid`),
  CONSTRAINT `countryidkey` FOREIGN KEY (`countryid`) REFERENCES `tm_country` (`countryid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_email_blacklist` (
  `blacklistid` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(84) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`blacklistid`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=518 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_lander` (
  `landerid` int(11) NOT NULL AUTO_INCREMENT,
  `landertypeid` int(11) NOT NULL,
  `name` varchar(45) NOT NULL,
  `url` varchar(2048) NOT NULL,
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`landerid`),
  UNIQUE KEY `name_idx` (`name`),
  UNIQUE KEY `url_idx` (`url`),
  KEY `landertypekey_idx` (`landertypeid`),
  CONSTRAINT `landertypekey` FOREIGN KEY (`landertypeid`) REFERENCES `tm_landertype` (`landertypeid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_landertype` (
  `landertypeid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `description` varchar(2048) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`landertypeid`),
  UNIQUE KEY `name_idx` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_lead` (
  `leadid` int(11) NOT NULL AUTO_INCREMENT,
  `msisdn` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `first_name` varchar(45) DEFAULT NULL,
  `last_name` varchar(45) DEFAULT NULL,
  `countryid` int(11) DEFAULT NULL,
  `address` varchar(45) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  `zip` varchar(45) DEFAULT NULL,
  `device` varchar(45) DEFAULT NULL,
  `mno` varchar(45) DEFAULT NULL,
  `actions_count` int(11) NOT NULL DEFAULT '0',
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`leadid`),
  UNIQUE KEY `email` (`email`),
  UNIQUE KEY `msisdn` (`msisdn`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_lead_history` (
  `historyID` int(11) NOT NULL AUTO_INCREMENT,
  `leadid` int(11) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `old_value` varchar(2048) DEFAULT NULL,
  `new_value` varchar(2048) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`historyID`),
  KEY `leadidkey_idx` (`leadid`),
  CONSTRAINT `leadidkey1` FOREIGN KEY (`leadid`) REFERENCES `tm_lead` (`leadid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_prelander` (
  `prelanderid` int(11) NOT NULL AUTO_INCREMENT,
  `prelandertypeid` int(11) NOT NULL,
  `name` varchar(45) NOT NULL,
  `url` varchar(2048) NOT NULL,
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`prelanderid`),
  UNIQUE KEY `name_idx` (`name`),
  UNIQUE KEY `url_idx` (`url`),
  KEY `prelandertypeidkey_idx` (`prelandertypeid`),
  CONSTRAINT `prelandertypeidkey` FOREIGN KEY (`prelandertypeid`) REFERENCES `tm_prelandertype` (`prelandertypeid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_prelandertype` (
  `prelandertypeid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `description` varchar(1024) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`prelandertypeid`),
  UNIQUE KEY `name_idx` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_provider` (
  `providerid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `price` decimal(10,0) DEFAULT '0',
  PRIMARY KEY (`providerid`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_session` (
  `sessionid` int(11) NOT NULL AUTO_INCREMENT,
  `guid` varchar(36) NOT NULL,
  `sessiontype` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `actionid` int(11) NOT NULL,
  `sessiondataid` int(11) DEFAULT NULL,
  `sessionrequestid` int(11) DEFAULT NULL,
  `duration` decimal(5,2) NOT NULL DEFAULT '0.00',
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`sessionid`),
  KEY `userid_idx` (`userid`),
  KEY `sessiondataid_idx` (`sessiondataid`),
  KEY `requid_idx` (`sessionrequestid`),
  KEY `type_idx` (`sessiontype`),
  KEY `actionkey_idx` (`actionid`),
  CONSTRAINT `actionkey` FOREIGN KEY (`actionid`) REFERENCES `tm_action` (`actionid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `sessiondataid` FOREIGN KEY (`sessiondataid`) REFERENCES `tm_session_data` (`sessiondataid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `userid` FOREIGN KEY (`userid`) REFERENCES `tm_user` (`userid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_session_data` (
  `sessiondataid` int(11) NOT NULL AUTO_INCREMENT,
  `countryCode` varchar(2) DEFAULT NULL,
  `countryName` varchar(45) DEFAULT NULL,
  `region` varchar(84) DEFAULT NULL,
  `city` varchar(84) DEFAULT NULL,
  `zipCode` varchar(84) DEFAULT NULL,
  `ISP` varchar(256) DEFAULT NULL,
  `latitude` varchar(84) DEFAULT NULL,
  `longitude` varchar(84) DEFAULT NULL,
  `timezone` varchar(45) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`sessiondataid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_session_request` (
  `sessionrequestid` int(11) NOT NULL AUTO_INCREMENT,
  `rawurl` varchar(1024) NOT NULL,
  `ip` varchar(45) NOT NULL,
  `useragent` varchar(45) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`sessionrequestid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `tm_session_type` (
  `sessiontypeid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`sessiontypeid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

CREATE TABLE `tm_user` (
  `userid` int(11) NOT NULL AUTO_INCREMENT,
  `guid` varchar(36) NOT NULL,
  `countryid` int(11) DEFAULT NULL,
  `leadid` int(11) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`userid`),
  UNIQUE KEY `guid_indx` (`guid`),
  KEY `leadid_idx` (`leadid`),
  KEY `leadidkey_idx` (`leadid`),
  KEY `countrykey_idx` (`countryid`),
  CONSTRAINT `countrykey` FOREIGN KEY (`countryid`) REFERENCES `tm_country` (`countryid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `leadidkey` FOREIGN KEY (`leadid`) REFERENCES `tm_lead` (`leadid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


SET FOREIGN_KEY_CHECKS = 1;
COMMIT;