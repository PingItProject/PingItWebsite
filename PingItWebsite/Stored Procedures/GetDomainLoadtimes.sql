CREATE DEFINER=`root`@`%` PROCEDURE `GetDomainLoadtimes`(IN uwebsite VARCHAR(100), IN ordering bool)
BEGIN
	SELECT		website, 
				country, 
                city, 
                w.connection, 
                w.first_byte, 
                w.total
    FROM		websites w
    WHERE		w.website = uwebsite
    ORDER BY 
		CASE WHEN ordering = TRUE 
			 THEN country END ASC, city,
		CASE WHEN ordering = FALSE 
             THEN country END DESC, city;
END