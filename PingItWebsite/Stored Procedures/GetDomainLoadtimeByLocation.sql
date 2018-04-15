CREATE DEFINER=`root`@`%` PROCEDURE `GetDomainLoadtimeByLocation`(IN user VARCHAR(50), IN domain VARCHAR(100))
BEGIN
	SELECT		city,
				state, 
				AVG(loadtime) AS loadtime
	FROM(
				#Combine the google and webtest info that you need 
				SELECT	wt.state, 
						wt.city, 
						loadtime
				FROM 	PingIt.googletests gt
				JOIN 	PingIt.webtests wt
					ON 	wt.guid = gt.guid 
						AND 
						wt.username = user
				WHERE	wt.url LIKE concat('%', domain, '%')
	) combined
	GROUP BY combined.city, combined.state;
END