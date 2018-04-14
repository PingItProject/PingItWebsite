CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestChron`(IN user VARCHAR(100))
BEGIN
	   SELECT 	chron.*, rt.rank
       FROM
       #Get the user tests chronologically for a chart
       (SELECT  wt.tstamp, 
				wt.city, 
				wt.state, 
				wt.provider, 
				wt.loadtime, 
				gt.webspeed as speed
		FROM 	webtests wt
		JOIN (
    
			SELECT	state,
					city, 
					provider,
					tstamp, 
					guid
			FROM (
				#Organize the data by tstamp asc
				SELECT		tstamp, 
							state, 
							city, 
							provider, 
							guid
				FROM		webtests
				WHERE		username = user
				ORDER BY 	tstamp ASC
			) orderedtable
			GROUP BY	state,
						city,
						provider,
						tstamp, 
						guid
		) b 
			ON 	b.guid = wt.guid
		JOIN	googletests gt
			ON	gt.guid = wt.guid
		) chron
        JOIN (
		#Get the top five state, city, and providers that were tested most
			SELECT 		    tests.*, 
							@rank := @rank + 1 AS rank
            FROM(
				SELECT		state,
							city,
							provider,
							COUNT(*) AS tests
				FROM		PingIt.webtests wt
				JOIN 		PingIt.googletests gt
					ON 		gt.guid = wt.guid
				WHERE		wt.username = user
				GROUP BY 	state, city, provider
				ORDER BY 	tests DESC
			) tests,
			(SELECT 		@rank := 0) r
		) rt
        ON			chron.state = rt.state
				AND chron.city  = rt.city
				AND chron.provider = rt.provider
		ORDER BY	chron.tstamp ASC;
END