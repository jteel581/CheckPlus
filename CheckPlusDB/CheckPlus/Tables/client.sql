/*
	description:	the businesses we do business with
	notes:			
*/
create table dbo.client
(
	client_id		int				identity(100000, 1)		not null,
	client_nm		varchar(100)	not null,		--the name of the client
	default_fee		decimal(5,2)	not null,		--the fee that the client assigns to bad checks
	days_bw_letters	int				not null,		--the client's decided number of days between letters' being sent
primary key (client_id asc)
)
;