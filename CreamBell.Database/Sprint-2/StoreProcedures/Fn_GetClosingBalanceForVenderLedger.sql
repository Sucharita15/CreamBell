USE [7200]
GO
/****** Object:  UserDefinedFunction [dbo].[Fn_GetClosingBalanceForVenderLedger]    Script Date: 10/31/2019 7:47:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Function [dbo].[Fn_GetClosingBalanceForVenderLedger]
(@FromDate smalldatetime,@SiteCode nvarchar(max)='',@PlantCode nvarchar(255)='') 
returns numeric(18,4)                  
AS        
BEGIN --START OF FUNCTION        


   declare @ClosingBalance as  numeric(18,4) 
   declare @Adjustment numeric(18,2)  
	
	DECLARE @TMPSITELIST TABLE (SITEID NVARCHAR(20));                           
	IF LEN(@SiteCode)>0                     
	BEGIN INSERT INTO @TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SiteCode,',')   END

	SELECT @Adjustment =ISNULL(SUM(ADHLI.Adjvalue),0)
	FROM [ax].ACXADJUSTMENTENTRYHEADER ADJH
	INNER JOIN [ax].[ACXADJUSTMENTENTRYLINE] ADHLI ON ADHLI.DOCUMENT_NO = ADJH.DOCUMENT_NO
	WHERE ADJH.DOCUMENT_DATE<=@FromDate AND ADJH.Site_Code  IN (SELECT SITEID FROM @TMPSITELIST)
	                  
   if (Len(@PlantCode)>0)
   Begin
   set @ClosingBalance=(select Sum(RemainingAmount)  As RemainingAmount from [ax].ACXVENDORTRANS       
   where Document_Date<@FromDate and SiteCode IN (SELECT SITEID FROM @TMPSITELIST) and Dealer_Code=@PlantCode)    
  end
  else
  begin
  set @ClosingBalance=(select Sum(RemainingAmount)  As RemainingAmount from [ax].ACXVENDORTRANS       
   where Document_Date<@FromDate and SiteCode IN (SELECT SITEID FROM @TMPSITELIST) )    
  end
   
  if (@ClosingBalance is NOT null )    
    return @ClosingBalance;     
  
  if(@ClosingBalance is null)    
  begin    
    set @ClosingBalance=0;    
  end    

  return (@ClosingBalance+@Adjustment); 
           
END --END OF FUCTION
