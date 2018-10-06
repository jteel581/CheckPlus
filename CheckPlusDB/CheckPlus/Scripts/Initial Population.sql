/*
	purpose:	just to populate the database with dummy data
	notes:		must copy-paste into ssms and run manually
*/

--not up to date right now; do not use
insert into dbo.acct_holder
(		first_name,		last_name,			middle_name,		title			)
select	'John',			'Smith',			'Xavier',			'Mr.'		union
select	'Madelynn',		'Geraldino',		'Patricia',			'Mrs.'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Jamie',		'Jackson',			'Alex',				'Miss'		union
select	'Maggie',		'Gordon',			'',					''
;

insert into dbo.bank 
(		bank_nm,							routing_number,					contact_nm,						contact_email,					contact_phone				)
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'First Union Together Bank',		'000123678',					'Grant Billings',				'gjbilling@fun.com',			'675-555-8976'			union
select	'Union Credit Union',				'000432178',					'Sherida Smith',				'sksmith@ucu.org',				'864-555-1234'				
;

insert into dbo.address
(		address_nm,							city,					state,			country,				zip_code			)
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'			union
select	'1701 Wade Hemptan Blvd.',			'Charlotte',			'NC',			'United States',		'28803'
;

insert into dbo.account 
(		acct_holder_id,			bank_id,			address_id,			account_number,				phone_number			)
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'		union
select	100000,					100001,				100000,				'9991234321234',			'864-555-8765'
;

insert into dbo.client
(		client_nm,				default_fee,		days_bw_letters			)
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15					union
select	'Chick-fil-A',			100.05,				15
;

insert into dbo.acct_check 
(		account_id,			check_number,		amount,			date_written,			client_id			)
select	100000,				'000001',			235.67,			'2018-07-24',			100000
;