/*
	description:	describes information regarding the bad checks
	notes:			
*/
create table dbo.account_check
(
	account_check_id	int				identity(10000, 1)		not null,		--pk
	account_id			int										not null,		--fk to the account from which the check needs to take funds
	amount				decimal(5, 2)							not null,		--amount for which the check was made out
	date_received		date									not null,		--date that we received the bad check from the bank
primary key (account_check_id asc),
foreign key (account_id) references dbo.account (account_id)
)
;