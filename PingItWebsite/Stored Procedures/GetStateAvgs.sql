CREATE DEFINER=`root`@`%` PROCEDURE `GetStateAvgs`(IN user VARCHAR(100))
BEGIN
	#Get state test averages for a chart and graph
	SELECT		state, 
				city, 
                provider, 
                AVG(webspeed) AS speed, 
                AVG(loadtime) AS loadtime, 
                AVG(score) 	  AS score,
                #count how many tests fit into the categories
                COUNT(CASE WHEN category = 'LOW' THEN 1 END) AS low,
                COUNT(CASE WHEN category = 'AVERAGE' THEN 1 END) AS average,
                COUNT(CASE WHEN category = 'FAST' THEN 1 END) AS fast
	FROM(
			SELECT	wt.provider,
					wt.state, 
					wt.city, 
					wt.loadtime,
                    category,
					score, 
					webspeed
			FROM 	PingIt.googletests gt
			JOIN 	PingIt.webtests wt
				ON 	wt.guid = gt.guid 
					AND 
                    wt.username = user
		) combined
	GROUP BY 
        combined.state, 
        combined.city, 
        combined.provider,
        combined.category
	ORDER BY state ASC;
END