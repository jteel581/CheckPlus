/*
	description:
	notes:
*/
CREATE TABLE dbo.entity_type
(
	entity_type_id		int				not null,
	entity_type_nm		varchar(50)		not null,					--e.g. 'Account Holder', 'Bank', 'Employee'
primary key (entity_type_id asc)
)
;