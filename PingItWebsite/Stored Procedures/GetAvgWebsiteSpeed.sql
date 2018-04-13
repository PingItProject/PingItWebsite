CREATE DEFINER=`root`@`%` PROCEDURE `GetAvgSpeedWebsite`()
BEGIN
	SELECT 		website, AVG(total) as total
	FROM 		PingIt.websites
	GROUP BY	website;
END