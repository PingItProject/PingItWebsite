CREATE DEFINER=`root`@`%` PROCEDURE `GetUserAvgDomainLoadtime`(IN user VARCHAR(50), IN domain VARCHAR(100))
BEGIN
	SELECT	AVG(loadtime) as loadtime
	FROM 	PingIt.googletests gt
	JOIN 	PingIt.webtests wt
		ON 	wt.guid = gt.guid 
			AND 
			wt.username = user
	WHERE	wt.url LIKE concat('%', domain, '%');
END