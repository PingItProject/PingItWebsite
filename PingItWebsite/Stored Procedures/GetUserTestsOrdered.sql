CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestsOrdered`(IN user VARCHAR(100))
BEGIN
	   SELECT 	ordered.*, 
				rt.rank
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
		) ordered
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
        ON			ordered.state = rt.state
				AND ordered.city  = rt.city
				AND ordered.provider = rt.provider
		ORDER BY	ordered.tstamp ASC;
END