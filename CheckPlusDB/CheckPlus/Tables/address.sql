/*
	description:
	notes:
*/
create table dbo.address
(
	address_id		int				identity(100000, 1)		not null,
	address_nm		varchar(100)	not null,
	city			varchar(50)		not null,
	state			varchar(50)		null,
	country			varchar(100)	null,
	zip_code		varchar(15)		null,
primary key (address_id asc)
)
;
