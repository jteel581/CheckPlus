/*
	description:
	notes:
*/
create table dbo.account_type
(
	account_type_id			int				identity(1000, 1)		not null,
	account_type_nm			varchar(30)								not null,
primary key (account_type_id asc)
)
;