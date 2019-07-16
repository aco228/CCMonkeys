START TRANSACTION;
SET @trackingid = 'wrde34hwKgOTtPvH3KkSOslGGjyuPGuaPMYbtBQfsk0';
SET @postbackid = (SELECT postbackid FROM cc_postback WHERE trackingid=@trackingid ORDER BY postbackid DESC LIMIT 1);
SET @url = (SELECT url FROM cc_postback WHERE trackingid=@trackingid ORDER BY postbackid DESC LIMIT 1);
SET @actionid = (SELECT actionid FROM cc_postback WHERE trackingid=@trackingid ORDER BY postbackid DESC LIMIT 1);

SELECT @url AS 'url', log.text, log.created FROM cc_postback_log AS log WHERE postbackid=@postbackid;

COMMIT;