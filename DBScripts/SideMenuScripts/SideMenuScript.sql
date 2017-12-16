
insert into modules (
	ModuleName,	Url	,Status,	ModuleCode,	IsFederation,	DisplayOrder,	ControlId,	ParentID,	IsSeed)
	values(
'Receipts & Payments A/C',	'RecieptsAndPayments/ReceiptsAndPayments',	1	,'',	0,	3	,'lnkSideReceiptsAndPaments',	4	,1)

insert into RoleModules (ModuleId,	RoleId,	ModuleActionId,	Status,	RoleModuleCode)
Values(	1100,	3	,NULL	,1,	NULL)

select * from Roles where roleid= 2

select * from Employee