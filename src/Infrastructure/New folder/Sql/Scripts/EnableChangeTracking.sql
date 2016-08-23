--Enable Tracking for the database
ALTER DATABASE [db-msonecatalogdev]
SET CHANGE_TRACKING = ON
(CHANGE_RETENTION = 5 DAYS, AUTO_CLEANUP = ON)

--Enable Tracking for Project table
ALTER TABLE dbo.Project
ENABLE CHANGE_TRACKING
WITH (TRACK_COLUMNS_UPDATED = ON)

--Enable Tracking for Technology table
ALTER TABLE dbo.Technology
ENABLE CHANGE_TRACKING
WITH (TRACK_COLUMNS_UPDATED = ON)