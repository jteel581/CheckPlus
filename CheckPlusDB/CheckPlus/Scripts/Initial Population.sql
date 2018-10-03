/*
	purpose:	just to populate the database with dummy data
	notes:		must copy-paste into ssms and run manually
*/

--not up to date right now; do not use
insert into dbo.person 
(
	person_id,
	first_name,
	last_name,
	middle_name,
	title
)
select 111000, 'John', 'Smith', 'Xavier', 'Mr.' union
select 111001, 'Madelynn', 'Geraldino', 'Patricia', 'Mrs.' union
select 111002, 'Jamie', 'Jackson', 'Alex', 'Miss' union
select 112000, 'Maggie', 'Gordon', '', ''
;

insert into dbo.bank 
(
	bank_id,
	bank_nm
)
select 111500, 'First Union Together Bank' union
select 111501, 'Union Credit Union'
;

insert into dbo.bank 
(
	bank_id,
	contact_id,
	contact_email,
	routing_number
)
select 111500, 112000, 'mgordon@futb.com', '864-555-2134'
;

insert into dbo.account (
	account_id,
	person_id,
	bank_id,
	account_number,
	date_start
)
select 111100, 111000, 111500, '9991234321234', GETDATE()
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