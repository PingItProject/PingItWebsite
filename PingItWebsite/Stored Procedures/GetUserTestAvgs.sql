CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestAvgs`(IN user VARCHAR(50))
BEGIN
	SELECT		url, 
				provider, 
                state, 
                city, 
                AVG(webspeed) AS speed, 
                AVG(loadtime) AS loadtime, 
                AVG(score) 	  AS score 
	FROM(
			SELECT	wt.url, 
					wt.provider, 
					wt.state, 
					wt.city, 
					wt.loadtime, 
					score, 
					webspeed
			FROM 	PingIt.googletests gt
			JOIN 	PingIt.webtests wt
				ON 	wt.guid = gt.guid 
					AND 
                    wt.username = user
		) combined
	GROUP BY 
		combined.url, 
        combined.provider, 
        combined.state, 
        combined.city
	ORDER BY url ASC;
END