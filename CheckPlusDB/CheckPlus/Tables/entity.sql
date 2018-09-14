/*
	description:	the base table for all our entities
	notes:			doesn't have much in it besides fks to descriptive tables
*/
CREATE TABLE dbo.entity
(
	entity_id		int			identity(1000, 1)		not null	primary key,
	entity_type_id	int									not null
)
