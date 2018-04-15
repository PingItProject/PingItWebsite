CREATE DEFINER=`root`@`%` PROCEDURE `GetUserPlotKeys`(IN user VARCHAR(100))
BEGIN
	#Get's the keys that are used for the plot graphs
	SELECT		tests.*, 
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
		(SELECT 		@rank := 0) r;
END