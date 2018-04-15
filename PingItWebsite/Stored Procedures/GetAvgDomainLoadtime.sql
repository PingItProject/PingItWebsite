CREATE DEFINER=`root`@`%` PROCEDURE `GetAvgDomainLoadtime`(IN domain VARCHAR(100))
BEGIN
	SELECT 		website, 
				AVG(total) as total
	FROM 		PingIt.websites
    WHERE		domain = 'null' 
				OR website LIKE concat('%', domain, '%')
	GROUP BY	website;
END