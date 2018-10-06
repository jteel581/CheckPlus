/*
	description:		details various information regarding a bank account entered into our system
	notes:				one individual can have multiple accounts
*/
create table dbo.account
(
	account_id			int			not null,	--pk the meaningless id assigned to the account
	entity_id_1			int			not null,	--fk to the id of the accountholder (typically a person)
	entity_id_2			int			not null,	--fk to the id of the bank
	account_number		char(9)		not null,
	routing_number		char(13)	not null,
	date_start			date		not null,	--date that the account was entered into our system
	date_end			date		null		--date that we marked the account as ended/inactive; null indicates still in use
primary key (account_id asc),
foreign key (entity_id_1) references dbo.entity (entity_id),
foreign key (entity_id_2) references dbo.entity (entity_id)
)
;