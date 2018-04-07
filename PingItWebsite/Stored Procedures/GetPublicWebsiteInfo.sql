CREATE DEFINER=`root`@`%` PROCEDURE `GetPublicWebsiteInfo`(IN uwebsite VARCHAR(100), IN ordering bool)
BEGIN
	SELECT		website, country, city, w.connection, w.first_byte, w.total
    FROM		websites w
    WHERE		(uwebsite = 'null' OR w.website = uwebsite)
    ORDER BY 
		CASE WHEN ordering = TRUE THEN website END ASC,
		CASE WHEN ordering = FALSE THEN website END DESC;
END