/*
	description:	those who hold accounts from which a bad check was written
	notes:			if the account holder is a business, then only the first_name field is used
*/
create table dbo.acct_holder
(
	acct_holder_id	int				identity(100000, 1)		not null,
	first_name		varchar(50)		not null,	
	last_name		varchar(100)	null,			--nullable due to note mentioned above
	middle_name		varchar(50)		null,
	suffix			varchar(10)		null,
	title			varchar(10)		null,
primary key (acct_holder_id asc)
)
;