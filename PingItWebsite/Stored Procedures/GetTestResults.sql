CREATE DEFINER=`root`@`%` PROCEDURE `GetTestResults`(IN c VARCHAR(100), IN s VARCHAR(10), IN browser VARCHAR(25), IN ordering bool)
BEGIN
	
	SELECT	tstamp, url, loadtime, city, state, platform, guid
	FROM 	webtests w
	WHERE	
		#For data with neither
		(c = 'null' AND browser = 'null')
        OR
		#For data with just a browser
		(c = 'null' AND platform = browser)
        OR
        #For data with just a location
        (city = c AND state = s AND browser = 'null')
        OR 
        #For data with both
        (city = c AND state = s AND platform = browser)
	ORDER BY 
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END