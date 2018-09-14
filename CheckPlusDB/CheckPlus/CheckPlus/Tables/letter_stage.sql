/*
	description:
	notes:
*/
create table dbo.letter_stage
(
	letter_stage_id			int				identity(1000, 1)			not null,
	letter_stage_nm			varchar(30)									not null,
	default_text			varchar(1000)								null,
primary key (letter_stage_id asc)
)
;