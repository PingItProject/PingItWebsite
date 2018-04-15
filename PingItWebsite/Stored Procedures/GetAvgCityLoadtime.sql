CREATE DEFINER=`root`@`%` PROCEDURE `GetAvgCityLoadtime`(IN domain VARCHAR(100))
BEGIN
	SELECT 		city, country, AVG(total) as total
	FROM 		PingIt.websites
    WHERE		domain = 'null' OR website LIKE concat('%', domain, '%')
	GROUP BY	city, country;
END