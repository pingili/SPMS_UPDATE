
update TypeQueries
set TypeQuery='SELECT DISTINCT GLAccountId ID, GLNAME + ''::'' + GLCODE AS NAME FROM VWGroupAccountTree WHERE TransactionType in (''G'',''B'')'
where TypeCode='GROUP_GOR_GL_HEADS'
