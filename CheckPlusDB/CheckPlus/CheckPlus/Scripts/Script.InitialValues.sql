/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

insert into dbo.entity_type (entity_type_nm)
	select 'Account Holder' union
	select 'Client' union
	select 'Clerk' union
	select 'Supervisor' union
	select 'Manager' union 
	select 'Employee' union 
	select 'Bank'
;

insert into dbo.entity (entity_type_id)
	select 1000 union
	select 1002 union
	select 1003 union
	select 1006 union
	select 1001
;