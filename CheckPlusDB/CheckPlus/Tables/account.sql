/*
	description:		details various information regarding a bank account entered into our system
	notes:				one individual can have multiple accounts
*/
create table dbo.account
(
	account_id			int			identity(100000, 1)		not null,
	acct_holder_id		int									not null,	--fk to the id of the account holder (typically a person)
	acct_holder_id_2	int									null,		--fk to possible secondary account holder
	bank_id				int									not null,	--fk to the id of the bank
	address_id			int									not null,	--fk to the id of the address associated with the account
	account_number		varchar(20)							not null,
	date_start			date		default getdate()		not null,	--date that the account was entered into our system
	date_end			date								null,		--date that we marked the account as ended/inactive; null indicates still in use;
	phone_number		varchar(15)							null,		--phone number assigned to the account
primary key (account_id asc),
foreign key (acct_holder_id) references dbo.acct_holder (acct_holder_id),
foreign key (acct_holder_id_2) references dbo.acct_holder (acct_holder_id),
foreign key (bank_id) references dbo.bank (bank_id)
)
;