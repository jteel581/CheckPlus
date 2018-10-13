/*
	description:		details various information regarding a bank account entered into our system
	notes:				one individual can have multiple accounts
*/
create table dbo.account
(
	account_id			int			identity(100000, 1)		not null,
	first_name			varchar(50)							not null,
	last_name			varchar(50)							null,		--nullable because this could be a business, and in that case, first_name would be the only used field
	first_name_2nd		varchar(50)							null,
	last_name_2nd		varchar(50)							null,
	bank_id				int									not null,	--fk to the id of the bank
	address_nm			varchar(100)						not null,
	city				varchar(100)						not null,
	state				varchar(50)							null,
	country				varchar(100)						not null,
	zip_code			varchar(25)							not null,
	account_number		varchar(20)							not null,
	phone_number		varchar(15)							null,		--phone number assigned to the account
primary key (account_id asc),
foreign key (bank_id) references dbo.bank (bank_id)
)
;