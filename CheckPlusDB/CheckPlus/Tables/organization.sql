﻿/*
	description:
	notes:
*/
create table dbo.organization
(
	organization_id			int				not null,
	organization_nm			varchar(100)	not null,
primary key (organization_id asc),
foreign key (organization_id) references dbo.entity (entity_id)
)
;