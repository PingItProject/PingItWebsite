CREATE DEFINER=`root`@`%` PROCEDURE `GetTestResults`(IN loc VARCHAR(100), IN browser VARCHAR(25), IN ordering bool)
BEGIN
	
	SELECT	tstamp, url, loadtime, location, platform, guid
	FROM 	webtests w
	WHERE	
		#For data with neither
		(loc = 'null' AND browser = 'null')
        OR
		#For data with just a browser
		(loc = 'null' AND platform = browser)
        OR
        #For data with just a location
        (location = loc AND browser = 'null')
        OR 
        #For data with both
        (location = loc AND platform = browser)
	ORDER BY 
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END