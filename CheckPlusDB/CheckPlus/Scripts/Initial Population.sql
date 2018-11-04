/*
	purpose:	just to populate the database with dummy data
	notes:		must copy-paste into ssms and run manually
*/

use CheckPlus;

insert into dbo.bank 
(		bank_nm,				routing_number,		contact_nm,			contact_email,		contact_phone,	bank_address,		bank_city,		bank_state, bank_country,		bank_zip	)
--100000
select	'First Together Bank',	'000123678',		'Grant Billings',	'gjbill@fun.com',	'675-555-8976',	'123 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100001
select	'Name Me 1 Bank',		'239898788',		'John Doe1',		'jdoe1@nm1.com',	'675-555-8976',	'124 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100002
select	'Name Me 2 Bank',		'938747658',		'John Doe2',		'jdoe2@nm2.com',	'675-555-8976',	'125 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100003
select	'Name Me 3 Bank',		'342323678',		'John Doe3',		'jdoe3@nm3.com',	'675-555-8976',	'126 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100004
select	'Name Me 4 Bank',		'124453458',		'John Doe4',		'jdoe4@nm4.com',	'675-555-8976',	'127 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100005
select	'Name Me 5 Bank',		'002343678',		'John Doe5',		'jdoe5@nm5.com',	'675-555-8976',	'128 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100006
select	'Name Me 6 Bank',		'012334478',		'John Doe6',		'jdoe6@nm6.com',	'675-555-8976',	'129 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100007
select	'Name Me 7 Bank',		'010122378',		'John Doe7',		'jdoe7@nm7.com',	'675-555-8976',	'120 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100008
select	'Name Me 8 Bank',		'023433678',		'John Doe8',		'jdoe8@nm8.com',	'675-555-8976',	'121 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100009
select	'Name Me 9 Bank',		'123123678',		'John Doe9',		'jdoe9@nm9.com',	'675-555-8976',	'122 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100010
select	'Name Me 10 Bank',		'023666278',		'John Doe10',		'jdoe10@nm10.com',	'675-555-8976',	'112 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'		union
--100011
select	'Union Credit Union',	'000432178',		'Sherida Smith',	'sksmith@ucu.org',	'864-555-1234',	'113 Place Av.',	'Greensboro',	'Iowa',		'United States',	'54321'						
;

insert into dbo.account 
(		first_name,	last_name, 	bank_id,	address,		city,		state,			country,			zip_code,	account_number,		phone_number			)
--100000
select	'Shawna',	'Tweep',	100000,		'100 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'135468743564',		'864-555-8765'		union
--100001
select	'Garret',	'Darlin',	100001,		'101 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'68786765343655',	'864-555-8765'		union
--100002
select	'John',		'Doe2',		100002,		'102 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'655435468768',		'864-555-8765'		union
--100003
select	'John',		'Doe3',		100003,		'103 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6843543524135',	'864-555-8765'		union
--100004
select	'John',		'Doe4',		100001,		'104 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'35213875415435',	'864-555-8765'		union
--100005
select	'John',		'Doe5',		100002,		'105 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'13549865435243',	'864-555-8765'		union
--100006
select	'John',		'Doe6',		100003,		'106 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'688546835746241',	'864-555-8765'		union
--100007
select	'John',		'Doe7',		100001,		'107 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6543543152413',	'864-555-8765'		union
--100008
select	'John',		'Doe8',		100002,		'108 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'54654321321635',	'864-555-8765'		union
--100009
select	'John',		'Doe9',		100003,		'109 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6543216456354',	'864-555-8765'		union
--100010
select	'John',		'Doe10',	100004,		'110 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'585879874653132',	'864-555-8765'		union
--100011
select	'John',		'Doe11',	100005,		'111 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'879843213213',		'864-555-8765'		union
--100012
select	'John',		'Doe12',	100006,		'112 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'879846321321',		'864-555-8765'		union
--100013
select	'John',		'Doe13',	100007,		'113 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6887651354654',	'864-555-8765'		union
--100014
select	'John',		'Doe14',	100004,		'114 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'45467897965311',	'864-555-8765'		union
--100015
select	'John',		'Doe15',	100005,		'115 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'2132168798645351',	'864-555-8765'		union
--100016
select	'John',		'Doe16',	100006,		'116 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'689798465132168',	'864-555-8765'		union
--100017
select	'John',		'Doe17',	100007,		'117 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'3213546874635',	'864-555-8765'		union
--100018
select	'John',		'Doe18',	100004,		'118 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6875435468354',	'864-555-8765'		union
--100019
select	'John',		'Doe19',	100005,		'119 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'2135743545341',	'864-555-8765'		union
--100019
select	'John',		'Doe20',	100006,		'120 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'686798764512231',	'864-555-8765'		union
--100020
select	'John',		'Doe21',	100007,		'121 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6854354356354',	'864-555-8765'		union
--100021
select	'John',		'Doe22',	100004,		'122 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'68541635413635',	'864-555-8765'		union
--100022
select	'John',		'Doe23',	100005,		'123 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6853526345354',	'864-555-8765'		union
--100023
select	'John',		'Doe24',	100006,		'124 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6854685743524132',	'864-555-8765'		union
--100024
select	'John',		'Doe25',	100007,		'125 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6854658463546',	'864-555-8765'		union
--100025
select	'John',		'Doe26',	100004,		'126 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'68588746846534',	'864-555-8765'		union
--100026
select	'John',		'Doe27',	100005,		'127 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'654635432132',		'864-555-8765'		union
--100027
select	'John',		'Doe28',	100006,		'128 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6354165416354',	'864-555-8765'		union
--100028
select	'John',		'Doe29',	100007,		'129 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6554165468',		'864-555-8765'		union
--100029
select	'John',		'Doe30',	100008,		'130 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'685541635415',		'864-555-8765'		union
--100030
select	'John',		'Doe31',	100009,		'131 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'6564554135224',	'864-555-8765'		union
--100031
select	'John',		'Doe32',	100010,		'132 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'65546541635463',	'864-555-8765'		union
--100032
select	'John',		'Doe33',	100011,		'133 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'84654165346',		'864-555-8765'		union
--100033
select	'John',		'Doe34',	100009,		'134 Good Rd.',	'Smelton',	'Wisconsin', 	'United States',	'09876',	'54325435546354',	'864-555-8765'
;

insert into dbo.client
(		client_nm,				default_fee,		days_bw_letters			)
--100000
select	'Chick-fil-A',			100.05,				15					union
--100001
select	'Chick-fil-B',			100.05,				15					union
--100002
select	'Chick-fil-C',			100.05,				15					union
--100003
select	'Chick-fil-D',			100.05,				15					union
--100004
select	'Chick-fil-E',			100.05,				15					union
--100005
select	'Chick-fil-F',			100.05,				15					union
--100006
select	'Chick-fil-G',			100.05,				15					union
--100007
select	'Chick-fil-H',			100.05,				15					union
--100008
select	'Chick-fil-I',			100.05,				15
;

insert into dbo.acct_check 
(		account_id,			check_number,		amount,			date_written,	client_id	)
select	100000,				'000001',			235.67,			'2018-07-24',	100000		union
select	100001,				'000024',			1235.67,		'2018-07-24',	100001		union
select	100002,				'000058',			200.56,			'2018-07-24',	100002		union
select	100003,				'000101',			235.60,			'2018-07-24',	100003		union
select	100004,				'000023',			265.47,			'2018-07-24',	100003		union
select	100005,				'000005',			223.57,			'2018-07-24',	100004		union
select	100006,				'000047',			231.66,			'2018-07-24',	100005		union
select	100007,				'000059',			335.67,			'2018-07-24',	100006		union
select	100008,				'000061',			255.61,			'2018-07-24',	100007		union
select	100009,				'000070',			335.67,			'2018-07-24',	100008		union
select	100010,				'000015',			255.67,			'2018-07-24',	100008		union
select	100011,				'000028',			235.67,			'2018-07-24',	100008		union
select	100012,				'000003',			237.67,			'2018-07-24',	100007		union
select	100013,				'000005',			235.87,			'2018-07-24',	100006		union
select	100014,				'000004',			235.67,			'2018-07-24',	100001		union
select	100015,				'000078',			233.77,			'2018-07-24',	100002		union
select	100016,				'000104',			231.68,			'2018-07-24',	100002		union
select	100008,				'000064',			432.98,			'2018-07-24',	100007		union
select	100009,				'000071',			1565.89,		'2018-07-24',	100008		union
select	100010,				'000011',			100.00,			'2018-07-24',	100008		union
select	100011,				'000025',			549.78,			'2018-07-24',	100008		union
select	100012,				'000012',			154.67,			'2018-07-24',	100007		union
select	100013,				'000003',			143.56,			'2018-07-24',	100006		union
select	100014,				'000004',			987.67,			'2018-07-24',	100001		union
select	100015,				'000056',			342.53,			'2018-07-24',	100002		union
select	100016,				'000090',			242.56,			'2018-07-24',	100002		union
select	100000,				'000021',			235.67,			'2018-07-24',	100000		union
select	100001,				'000025',			1235.67,		'2018-07-24',	100001		union
select	100002,				'000108',			200.56,			'2018-07-24',	100002		union
select	100003,				'000121',			235.60,			'2018-07-24',	100003		union
select	100004,				'000100',			265.47,			'2018-07-24',	100003		union
select	100005,				'000055',			223.57,			'2018-07-24',	100004		union
select	100006,				'000067',			231.66,			'2018-07-24',	100005		union
select	100007,				'000099',			335.67,			'2018-07-24',	100006		union
select	100008,				'000001',			255.61,			'2018-07-24',	100007		union
select	100009,				'000072',			335.67,			'2018-07-24',	100008		union
select	100017,				'000110',			214.76,			'2018-07-24',	100001
;
