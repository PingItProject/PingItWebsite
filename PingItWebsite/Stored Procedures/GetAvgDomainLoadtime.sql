CREATE DEFINER=`root`@`%` PROCEDURE `GetAvgDomainLoadtime`()
BEGIN
	SELECT 		website, AVG(total) as total
	FROM 		PingIt.websites
	GROUP BY	website;
END