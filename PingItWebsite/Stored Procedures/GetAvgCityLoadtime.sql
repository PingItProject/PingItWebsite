CREATE DEFINER=`root`@`%` PROCEDURE `GetAvgCityLoadtime`()
BEGIN
	SELECT 		city, country, AVG(total) as total
	FROM 		PingIt.websites
	GROUP BY	city, country;
END