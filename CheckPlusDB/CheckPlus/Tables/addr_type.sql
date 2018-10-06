/*
	description:
	notes:
*/
create table [dbo].[addr_type]
(
	addr_type_id		int			not null,
	addr_type_cd		char(1)		not null,
	addr_type_nm		varchar(30)	not null,
primary key (addr_type_id asc)
)
;