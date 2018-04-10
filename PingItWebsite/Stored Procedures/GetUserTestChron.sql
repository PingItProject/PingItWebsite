CREATE DEFINER=`root`@`%` PROCEDURE `GetUserTestChron`(IN user VARCHAR(100))
BEGIN
	#Get the user tests chronologically for a chart
	SELECT	wt.tstamp, 
			wt.city, 
            wt.state, 
            wt.provider, 
            wt.loadtime, 
            gt.webspeed
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
		ON	gt.guid = wt.guid;
END