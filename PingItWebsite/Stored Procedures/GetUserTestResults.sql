CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestResults`(IN u VARCHAR(50), IN b int)
BEGIN
	SELECT		tstamp, url, loadtime, location, platform, guid
	FROM 		webtests w
	WHERE		username = u AND batch = b
	ORDER BY 	tstamp ASC;
END