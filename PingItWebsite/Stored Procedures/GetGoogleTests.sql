CREATE DEFINER=`root`@`%` PROCEDURE `GetGoogleTests`(IN ucity VARCHAR(100), IN ustate VARCHAR(10), IN ubrowser VARCHAR(25), IN uwebsite VARCHAR(100), IN ordering bool)
BEGIN
	SELECT 	w.tstamp, 
			score, 
            category, 
            resources, 
            g.hosts, 
            bytes, 
            html_bytes, 
            css_bytes, 
            image_bytes, 
            webspeed, 
            w.city, 
            w.state, 
            w.browser
    FROM 	googletests g
    JOIN 	webtests w 
		ON 	w.guid = g.guid 
		AND ((ucity = 'null' AND ubrowser = 'null')
			OR (ucity = 'null' AND w.browser = ubrowser)
			OR (w.city = ucity AND w.state = ustate AND ubrowser = 'null')
			OR (w.city = ucity AND w.state = ustate AND w.browser = ubrowser))
		AND (uwebsite = 'null' OR w.url LIKE concat('%', uwebsite, '%'))
	ORDER BY
		CASE WHEN ordering = TRUE 
			 THEN tstamp END ASC,
		CASE WHEN ordering = FALSE 
			 THEN tstamp END DESC;
END