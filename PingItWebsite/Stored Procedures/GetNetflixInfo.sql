CREATE DEFINER=`root`@`%` PROCEDURE `GetNetflixInfo`(IN ordering bool)
BEGIN
	SELECT		speed, n.type, batch
    FROM		netflix n
    ORDER BY 
		CASE WHEN ordering = TRUE THEN isp END ASC,
		CASE WHEN ordering = FALSE THEN isp END DESC;
END