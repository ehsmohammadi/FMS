delete FROM [StorageSpace].[BasicInfo].[ActivityLocation]

DBCC CHECKIDENT ('[StorageSpace].[BasicInfo].[ActivityLocation]', RESEED, 0)



INSERT INTO BasicInfo.ActivityLocation (
Code,  Name, 
        --CountryCity.dbo.cities.GMT,
		--Latitude ,
		--Longitude,
		CountryName        )

SELECT 
	CountryCity.dbo.countries.CountryAbbrivation + CountryCity.dbo.cities.CityAbbreviation AS Code,  CountryCity.dbo.cities.CityName AS Name, 
        --CountryCity.dbo.cities.GMT,
		--NULL AS Latitude ,
		--NULL AS Longitude,
		CountryCity.dbo.countries.CountryName AS CountryName        

FROM      CountryCity.dbo.cities INNER JOIN
                CountryCity.dbo.countries ON CountryCity.dbo.cities.CountryID = CountryCity.dbo.countries.CountryID
--WHERE CountryCity.dbo.countries.InActive = 0 AND CountryCity.dbo.cities.InActive = 0
where CountryCity.dbo.countries.CountryAbbrivation + CountryCity.dbo.cities.CityAbbreviation is not null
ORDER BY CountryCity.dbo.countries.CountryName

