/*
	description:
	notes:
*/
create table dbo.citystate
(
	citystate_id		int				identity(10000, 1)			not null,
	city				varchar(100)								not null,
	state				varchar(100)								not null,
	state_code			varchar(5)									null,
	country				varchar(100)								not null,
	postal_code			varchar(10)									null,
primary key (citystate_id asc)
)
;