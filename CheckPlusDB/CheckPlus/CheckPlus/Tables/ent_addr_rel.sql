/*
	description:
	notes:
*/
create table dbo.ent_addr_rel
(
	ent_addr_rel_id			int			identity(10000, 1)			not null,
	entity_id				int										not null,
	address_id				int										not null,
	date_start				date									not null,
	date_end				date									null,
primary key (ent_addr_rel_id asc),
foreign key (entity_id) references dbo.entity (entity_id),
foreign key (address_id) references dbo.address (address_id)
)
;