CREATE DEFINER=`root`@`%` PROCEDURE `GetGoogleTestResults`(IN c VARCHAR(100), IN s VARCHAR(10), IN browser VARCHAR(25), IN ordering bool)
BEGIN
	SELECT score, category, resources, g.hosts, bytes, html_bytes, css_bytes, image_bytes, webspeed, w.location, w.platform
    FROM googletests g
    JOIN webtests w 
		ON w.guid = g.guid 
			AND ((c = 'null' AND browser = 'null')
				OR (c = 'null' AND w.platform = browser)
                OR (w.city = c AND w.state = s AND browser = 'null')
                OR (w.city = c AND w.state = s AND w.platform = browser))
	ORDER BY
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END