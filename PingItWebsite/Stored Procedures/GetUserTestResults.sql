CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestResults`(IN user VARCHAR(50), IN num int)
BEGIN
	SELECT		tstamp, 
				url, 
                loadtime, 
                city, 
                state, 
                browser, 
                provider, 
                guid
	FROM 		webtests w
	WHERE		username = user 
				AND batch = num
	ORDER BY 	tstamp ASC;
END