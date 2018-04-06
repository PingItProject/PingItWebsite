CREATE DEFINER=`root`@`%` PROCEDURE `GetGoogleTestResults`(IN c VARCHAR(100), IN s VARCHAR(10), IN b VARCHAR(25), IN ordering bool)
BEGIN
	SELECT score, category, resources, g.hosts, bytes, html_bytes, css_bytes, image_bytes, webspeed, w.city, w.state, w.browser
    FROM googletests g
    JOIN webtests w 
		ON w.guid = g.guid 
			AND ((c = 'null' AND b = 'null')
				OR (c = 'null' AND w.browser = b)
                OR (w.city = c AND w.state = s AND b = 'null')
                OR (w.city = c AND w.state = s AND w.browser = b))
	ORDER BY
		CASE WHEN ordering = TRUE THEN tstamp END ASC,
		CASE WHEN ordering = FALSE THEN tstamp END DESC;
END