/*
	description:	displays all checks in the db
	notes:			mainly for debugging and sanity check purposes 
						as we integrate the db into the application
*/
select 
	a.account_id,
	ac.acct_check_id,
	b.bank_id,
	a.first_name,
	a.last_name,
	b.routing_number,
	a.account_number,
	ac.check_number,
	ac.amount,
	ac.date_written,
	ac.letter1_send_date,
	ac.letter2_send_date,
	ac.letter2_send_date
from dbo.account a
	join dbo.acct_check ac on ac.account_id = a.account_id
	join dbo.bank b on b.bank_id = a.bank_id
order by last_name, first_name