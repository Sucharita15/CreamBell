USE [7200]
GO
/****** Object:  UserDefinedFunction [dbo].[Fn_GetOpeningBalanceForVenderLedger]    Script Date: 10/31/2019 7:41:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Function [dbo].[Fn_GetOpeningBalanceForVenderLedger]
(@FromDate smalldatetime,@SiteCode nvarchar(max)='',@PlantCode nvarchar(255)='') 
returns numeric(18,4)                  

AS        

BEGIN --START OF FUNCTION        
  declare @openingBalance as  numeric(18,2)   
  declare @Payment numeric(18,2) 
  declare @Adjustment numeric(18,2)  
  
DECLARE @TMPSITELIST TABLE (SITEID NVARCHAR(20));                           
IF LEN(@SiteCode)>0                     
BEGIN INSERT INTO @TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SiteCode,',')   END

  
	SELECT @Adjustment =ISNULL(SUM(ADHLI.Adjvalue),0)
	FROM [ax].ACXADJUSTMENTENTRYHEADER ADJH
	INNER JOIN [ax].[ACXADJUSTMENTENTRYLINE] ADHLI ON ADHLI.DOCUMENT_NO = ADJH.DOCUMENT_NO
	WHERE ADJH.DOCUMENT_DATE<@FromDate AND ADJH.Site_Code  IN (SELECT SITEID FROM @TMPSITELIST)
	      
 if (Len(@PlantCode)>0)
 BEGIN
   set @openingBalance=(
	select Sum(Amount)  As Amount from [ax].ACXVENDORTRANS       
	where Document_Date<@FromDate and SiteCode IN (SELECT SITEID FROM @TMPSITELIST) and Dealer_Code=@PlantCode and RemainingAmount<>0
   )    
	              
   set @Payment=(select Sum(Payment_Amount)  As Amount from [ax].[ACXPAYMENTENTRY]   
   where Payment_Date<@FromDate and SiteCode IN (SELECT SITEID FROM @TMPSITELIST) and Plant_Code=@PlantCode)   
END
ELSE
BEGIN
   set @openingBalance=(select Sum(Amount)  As Amount from [ax].ACXVENDORTRANS       
   where Document_Date<@FromDate and SiteCode IN (SELECT SITEID FROM @TMPSITELIST)  and RemainingAmount<>0)    
             
   set @Payment=(select Sum(Payment_Amount)  As Amount from [ax].[ACXPAYMENTENTRY]   
   where Payment_Date<@FromDate and SiteCode IN (SELECT SITEID FROM @TMPSITELIST) )
     
END       
 if (@Payment is null )    
	 begin
	 SET @Payment=0;
 end
 
  if (@openingBalance is NOT null )    
  return @openingBalance-@Payment+@Adjustment;     
  
  if(@openingBalance is null)    
  begin    
    set @openingBalance=0-@Payment+@Adjustment;    
  end    

  return @openingBalance-@Payment+@Adjustment;          

END --END OF FUCTION
