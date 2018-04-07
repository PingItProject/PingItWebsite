CREATE DEFINER=`root`@`%` PROCEDURE `GetTestResults`(IN ucity VARCHAR(100), IN ustate VARCHAR(10), IN ubrowser VARCHAR(25), IN uwebsite VARCHAR(100), IN ordering bool)
BEGIN
	
	SELECT	tstamp, url, loadtime, city, state, browser, provider, guid
	FROM 	webtests w
	WHERE
		#For data with neither
		((ucity = 'null' AND ubrowser = 'null')
        OR
		#For data with just a browser
		(ucity = 'null' AND browser = ubrowser)
        OR
        #For data with just a location
        (city = ucity AND state = ustate AND ubrowser = 'null')
        OR 
        #For data with both
        (city = ucity AND state = ustate AND browser = ubrowser))
        AND
        #For data with or without a website
		(uwebsite = 'null' OR url LIKE concat('%', uwebsite, '%'))
	ORDER BY 
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END