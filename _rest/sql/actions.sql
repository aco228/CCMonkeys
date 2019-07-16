START TRANSACTION;
SET @trackingid = 'wrde34hwKgOTtPvH3KkSOslGGjyuPGuaPMYbtBQfsk0';

SELECT 
	a.guid AS 'aguid', a.trackingid, u.guid AS 'uguid', country.code AS 'country',
    #a.prelandertypeid, a.prelanderid, a.landertypeid, a.landerid,
    a.input_redirect, a.input_email, a.input_contact, a.has_redirectedToProvider,
    a.has_subscription, a.has_chargeback, a.has_refund, 
    a.times_charged, a.times_upsell,
    lead.msisdn, lead.email, lead.first_name, lead.last_name, lead.address, lead.city, lead.zip,
    a.created
FROM tm_action AS a 
LEFT OUTER JOIN tm_user AS u ON a.userid=u.userid
LEFT OUTER JOIN tm_lead AS lead ON a.leadid=lead.leadid
LEFT OUTER JOIN tm_country AS country ON a.countryid=country.countryid
WHERE a.trackingid=@trackingid;

COMMIT;