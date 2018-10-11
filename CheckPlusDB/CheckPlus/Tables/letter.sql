/*
	description:
	notes:
*/
create table dbo.letter
(
	letter_id			int		identity(100000, 1)		not null,
	acct_check_id		int		not null,				--fk to the bad check which the letter must address
	date_sent_stg_1		date	null,
	date_sent_stg_2		date	null,
	date_sent_stg_3		date	null,
	date_response		date	null,
primary key (letter_id asc),
foreign key (acct_check_id) references dbo.acct_check (acct_check_id)
)
;