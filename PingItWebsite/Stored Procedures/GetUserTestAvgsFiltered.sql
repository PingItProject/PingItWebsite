CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestAvgsFiltered`(IN user VARCHAR(50), IN ucity VARCHAR(100), IN ustate VARCHAR(10), IN domain VARCHAR(100))
BEGIN
	#Get user test avgs for a chart
    
    #Get selected columns as well as averages for data
    SELECT			t.*,
					@rownum := @rownum + 1 AS id
    FROM(
		SELECT		url, 
					provider, 
					state, 
					city, 
					AVG(webspeed) AS speed, 
					AVG(loadtime) AS loadtime
		FROM(
				#Combine the google and webtest info that you need 
				SELECT	wt.url, 
						wt.provider, 
						wt.state, 
						wt.city, 
						wt.loadtime, 
						webspeed
				FROM 	PingIt.googletests gt
				JOIN 	PingIt.webtests wt
					ON 	wt.guid = gt.guid 
						AND 
						wt.username = user
				WHERE	wt.url LIKE concat('%', uwebsite, '%')
						AND wt.city = ucity
                        AND wt.state = ustate
			) combined
		GROUP BY 
			combined.url, 
			combined.provider
		ORDER BY url ASC
	) t,
    (SELECT	@rownum := 0) r;
END