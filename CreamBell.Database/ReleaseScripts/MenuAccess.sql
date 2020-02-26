DECLARE @list varchar(8000)
DECLARE @pos INT
DECLARE @len INT
DECLARE @value varchar(8000)

SET @list = 'M038,M039,M040,M041,M042,M044,M045,M046,M047,M048,M049,M050,M063,M069,M071,M073,M074,M075,M076,M099,M101,M102,M110,M111,M114,M115,M116,M118,M119,M120,M121,M122,M123,M124,M126,M160,'

set @pos = 0
set @len = 0

WHILE CHARINDEX(',', @list, @pos+1)>0
BEGIN
    set @len = CHARINDEX(',', @list, @pos+1) - @pos
    set @value = SUBSTRING(@list, @pos, @len)
            
    PRINT @value   
    
   ----------------------------------------------
   If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R001' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R001','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R002' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R002','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R004' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R004','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R005' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R005','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R006' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R006','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R007' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R007','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	If Not Exists(select * from [ax].[ACXMENUROLEMASTER] where ROLECODE='R008' and MENUCODE = @value)
	Begin
	 Insert into [ax].[ACXMENUROLEMASTER] (ROLECODE,DATAAREAID,RECVERSION, PARTITION, RECID, MENUCODE) values 
									  ('R008','dat','1','5637144576', (select max(RECID)+1 from [ax].[ACXMENUROLEMASTER]) ,@value)
	End

	   -----------------------------------------------

    set @pos = CHARINDEX(',', @list, @pos+@len) +1
END
