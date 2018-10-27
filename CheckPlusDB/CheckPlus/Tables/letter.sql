/*
	description:
	notes:
*/
create table dbo.letter
(
	letter_id			int		identity(100000, 1)		not null,
	client_id			int								not null,
	letter1_text		varchar(max)					null,
	letter2_text		varchar(max)					null,
	letter3_text		varchar(max)					null,
primary key (letter_id asc),
foreign key (client_id) references dbo.client (client_id)
)
;