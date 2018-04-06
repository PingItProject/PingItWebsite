CREATE DEFINER=`root`@`%` PROCEDURE `GetTestResults`(IN c VARCHAR(100), IN s VARCHAR(10), IN b VARCHAR(25), IN ordering bool)
BEGIN
	
	SELECT	tstamp, url, loadtime, city, state, browser, provider, guid
	FROM 	webtests w
	WHERE	
		#For data with neither
		(c = 'null' AND b = 'null')
        OR
		#For data with just a browser
		(c = 'null' AND browser = b)
        OR
        #For data with just a location
        (city = c AND state = s AND b = 'null')
        OR 
        #For data with both
        (city = c AND state = s AND browser = b)
	ORDER BY 
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END