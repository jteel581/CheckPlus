/*
	description:		details various information regarding a bank account entered into our system
	notes:				
*/
create table dbo.cp_user
(
	cp_user_id		integer			identity(100000, 1)		not null,
	client_id		integer			null,			--fk to the client if it is a client user who needs to see reports
	first_name		varchar(30)		not null,
	last_name		varchar(30)		not null,
	username		varchar(15)		not null,
	user_password	varchar(50)		not null,
	user_role_cd	char(1)			not null,
primary key (cp_user_id asc)
)
;