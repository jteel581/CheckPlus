/*
	description:	describes information regarding the bad checks
	notes:			
*/
create table dbo.acct_check
(
	acct_check_id	int				not null,		--pk
	account_id		int				not null,		--fk to the account from which the check needs to take funds
	amount			decimal(5, 2)	not null,		--amount for which the check was made out
	date_written	date			not null,		--the date the check was written
	check_number	varchar(10)		not null,		--the number on the bad check
	date_received	date			not null,		--date that we received the bad check from the bank
	amount_paid		decimal(5, 2)	null,			--amount the individual paid to satisfy the bad check
	date_paid		date			null,			--date the individual paid the check
primary key (acct_check_id asc),
foreign key (account_id) references dbo.account (account_id)
)
;