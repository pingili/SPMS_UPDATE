

Update TypeQueries 
set TypeQuery='SELECT DISTINCT GLAccountId ID, GLNAME + ''::'' + GLCODE AS NAME FROM VWGroupAccountTree WHERE TransactionType IN (''G'')'
where TypeCode='GROUP_GOJ_FROM_GL_HEADS'