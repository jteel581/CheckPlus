/*
	description:	
	notes:
*/
create table dbo.user_role
(
	user_role_id		int			not null,
	user_role_nm		varchar(35)	not null,
	role_level			int			not null,
primary key (user_role_id asc)
)
;