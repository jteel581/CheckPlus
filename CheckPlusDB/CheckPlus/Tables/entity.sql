/*
	description:	the base table for all our entities
	notes:
*/
CREATE TABLE dbo.entity
(
	entity_id		int		not null,
	entity_type_id	int		not null,
primary key (entity_id asc),
foreign key (entity_type_id) references dbo.entity_type (entity_type_id)
)
