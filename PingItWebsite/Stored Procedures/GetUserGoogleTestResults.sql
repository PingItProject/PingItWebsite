CREATE DEFINER=`root`@`%` PROCEDURE `GetUserGoogleTestResults`(IN guid CHAR(36))
BEGIN
	SELECT		score, 
				category, 
                resources, 
                g.hosts, 
                bytes, 
                html_bytes, 
                css_bytes, 
                image_bytes, 
                webspeed
    FROM 		googletests g
    WHERE 		g.guid = guid;
END