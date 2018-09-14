/*
	description:
	notes:
*/
create table dbo.person
(
	person_id		int				identity(10000, 1)			not null,
	first_name		varchar(50)									not null,
	middle_name		varchar(50)									null,
	last_name		varchar(100)								not null,
	suffix			varchar(10)									null,
	title			varchar(10)									null,
primary key (person_id asc),
foreign key (person_id) references dbo.entity (entity_id)
)
;