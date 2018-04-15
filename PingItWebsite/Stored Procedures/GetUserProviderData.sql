CREATE DEFINER=`root`@`%` PROCEDURE `GetUserProviderData`(IN user VARCHAR(50), IN ucity VARCHAR(100), IN ustate VARCHAR(10), IN domain VARCHAR(100))
BEGIN
	SELECT	provider, 
			AVG(speed) as speed
	FROM
			(SELECT		url, 
						provider, 
						state, 
						city, 
						AVG(webspeed) AS speed
			FROM(
					#Combine the google and webtest info that you need 
					SELECT	wt.url, 
							wt.provider, 
							wt.state, 
							wt.city, 
							webspeed
					FROM 	PingIt.googletests gt
					JOIN 	PingIt.webtests wt
						ON 	wt.guid = gt.guid 
							AND 
							wt.username = user
					WHERE	wt.url LIKE concat('%', domain, '%')
							AND wt.city = ucity
							AND wt.state = ustate
				) combined
			GROUP BY 
				combined.url, 
				combined.provider
			ORDER BY url ASC) t
	GROUP BY t.provider;
END