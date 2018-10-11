/*
	description:		details various information regarding a bank
	notes:				
*/
create table dbo.bank
(
	bank_id			integer			identity(100000, 1)		not null,
	bank_nm			varchar(100)	not null,
	routing_number	varchar(20)		not null,
	contact_nm		varchar(35)		not null,
	contact_email	varchar(35)		not null,
	contact_phone	varchar(15)		not null,
	bank_address	varchar(100)	not null,
	bank_city		varchar(100)	not null,
	bank_state		varchar(50)		null,
	bank_country	varchar(100)	not null,
	bank_zip		varchar(15)		not null,
primary key (bank_id asc)
)
;