/*
	description:
	notes:
*/
create table dbo.address
(
	address_id		int				not null,
	citystate_id	int				null,
	addr_type_id	int				not null,
	address_nm		varchar(100)	not null,
primary key (address_id asc),
foreign key (citystate_id) references dbo.citystate (citystate_id),
foreign key (addr_type_id) references dbo.addr_type (addr_type_id)
)
;
