CREATE DEFINER=`root`@`%` PROCEDURE `GetGoogleTestResults`(IN loc VARCHAR(100), IN browser VARCHAR(25), IN ordering bool)
BEGIN
	SELECT score, category, resources, g.hosts, bytes, html_bytes, css_bytes, image_bytes, webspeed, w.location, w.platform
    FROM googletests g
    JOIN webtests w 
		ON w.guid = g.guid 
			AND ((loc = 'null' AND browser = 'null')
				OR (loc = 'null' AND w.platform = browser)
                OR (w.location = loc AND browser = 'null')
                OR (w.location = loc AND w.platform = browser))
	ORDER BY
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END