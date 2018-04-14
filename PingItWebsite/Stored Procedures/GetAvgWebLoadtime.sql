CREATE DEFINER=`root`@`%` PROCEDURE `GetAvgWebLoadtime`()
BEGIN
	SELECT 		website, AVG(total) as total
	FROM 		PingIt.websites
	GROUP BY	website;
END