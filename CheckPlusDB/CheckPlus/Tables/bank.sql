/*
	description:		details various information regarding a bank
	notes:				
*/
create table dbo.bank
(
	bank_id			integer			not null,
	bank_nm			varchar(100)	not null,
	routing_number	varchar(20)		not null,
	contact_nm		varchar(35)		not null,
	contact_email	varchar(35)		not null,
	contact_phone	varchar(15)		null,
primary key (bank_id asc)
)
;