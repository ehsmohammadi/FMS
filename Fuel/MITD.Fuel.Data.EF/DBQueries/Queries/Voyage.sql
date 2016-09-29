SELECT   Project_1.ProjectID AS Id,  Ship1.Code AS VesselCode,  Ship1.Owner AS ShipOwnerId,Project_1.Code AS VoyageNumber, 

CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, '/'),8,0,'/'),11,0,' '), 14,0,':') + ':00' AS DATETIME) AS StartDateTime,
CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, '/'),8,0,'/'),11,0,' '), 14,0,':') + ':00' AS DATETIME) AS EndDateTime,
~ Project_1.Disabled AS IsActive

FROM      [NGSQLBS,2433].BLKRotation.dbo.Project AS Project_1 INNER JOIN
                [NGSQLBS,2433].BLKRotation.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID
				


--========================
SELECT   Project_1.ProjectID AS Id, Ship1.Code AS VesselCode, Ship1.Owner AS ShipOwnerId, Project_1.Code AS VoyageNumber, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS StartDateTime, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS EndDateTime, 
                ~ Project_1.Disabled AS IsActive
FROM      [NGSQLCNT,2433].CNTRotation.dbo.Project AS Project_1 INNER JOIN
                [NGSQLCNT,2433].CNTRotation.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID
