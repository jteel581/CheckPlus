/*
	purpose:	just to populate the database with dummy data
	notes:		must copy-paste into ssms and run manually
*/

--NOT UP TO DATE; DO NOT USE


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