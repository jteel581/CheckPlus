/*
	description:
	notes:
*/
create table dbo.letter
(
	letter_id			int		identity(100000, 1)		not null,
	acct_check_id		int		not null,		--fk to the bad check which the letter must address
	letter_stage_id		int		not null,		--fk
	date_sent			date	null,
	date_response		date	null,
	previous_letter_id	int		null,
primary key (letter_id asc),
foreign key (acct_check_id) references dbo.acct_check (acct_check_id),
foreign key (letter_stage_id) references dbo.letter_stage (letter_stage_id),
foreign key (previous_letter_id) references dbo.letter (letter_id)
)
;