START TRANSACTION;
SET @prelanderID = 1;

DELETE FROM tm_prelander_tag_answer WHERE prelanderid=@prelanderID;
DELETE FROM tm_prelander_tag WHERE prelanderid=@prelanderid;
DELETE FROM tm_prelander_tag_answer WHERE prelanderid=@prelanderid;
UPDATE tm_action SET prelander_data=NULL WHERE prelanderid=@prelanderid;

COMMIT;