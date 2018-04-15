CREATE DEFINER=`root`@`%` PROCEDURE `GetUserProviderData`(IN user VARCHAR(50), IN ucity VARCHAR(100), IN ustate VARCHAR(10), IN domain VARCHAR(100))
BEGIN
	SELECT		provider, 
				state, 
				city, 
				AVG(webspeed) AS speed
	FROM(
				#Combine the google and webtest info that you need 
				SELECT	wt.provider, 
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
	GROUP BY combined.provider;
END