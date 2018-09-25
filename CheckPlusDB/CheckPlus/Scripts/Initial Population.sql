/*
	purpose:	just to populate the database with dummy data
	notes:		must copy-paste into ssms and run manually
*/

insert into dbo.entity_type (
	entity_type_id,
	entity_type_nm
)
select 1000, 'Account Holder' union
select 1001, 'Bank'
;

insert into dbo.entity (
	entity_id,
	entity_type_id
)
select 111000, 1000 union
select 111001, 1000 union
select 111002, 1000 union
select 111500, 1001 union
select 111501, 1001
;

insert into dbo.person (
	person_id,
	first_name,
	last_name,
	middle_name,
	title
)
select 111000, 'John', 'Smith', 'Xavier', 'Mr.' union
select 111001, 'Madelynn', 'Geraldino', 'Patricia', 'Mrs.' union
select 111002, 'Jamie', 'Jackson', 'Alex', 'Miss'
;

insert into dbo.organization (
	organization_id,
	organization_nm
)
select 111500, 'First Union Together Bank'
;

insert into dbo.account (
	account_id,
	entity_id_1,
	entity_id_2,
	account_number,
	routing_number,
	date_start
)
select 111100, 111000, 111500, '999867890', '9991234321234', '2018-09-18'
;

insert into dbo.account_check (
	account_check_id,
	account_id,
	check_number,
	amount,
	date_received
)
select 111110, 111100, '000001', 235.67, '2018-07-24'
;