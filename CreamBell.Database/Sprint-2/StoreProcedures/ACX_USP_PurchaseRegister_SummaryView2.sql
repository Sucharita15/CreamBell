USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_PurchaseRegister_SummaryView2]    Script Date: 10/9/2019 8:18:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[ACX_USP_PurchaseRegister_SummaryView2] '000059144','01-Nov-2017','22-Dec-2017'   
ALTER proc [dbo].[ACX_USP_PurchaseRegister_SummaryView2]   
(    
@Site_Code nvarchar(max)=0,     
@StartDate Date = '',@EndDate Date='')    
As    
BEGIN    
SET NOCOUNT ON;  

IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@Site_Code)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@Site_Code,',')   END   

SELECT CAST(TOTAL_TAX_PER AS decimal(16,2)) AS [GST%],SUM(BOXQTY) AS UNITS,
cast(SUM(LTR) as decimal(16,2)) AS LTRS,cast(SUM(BASICVALUE) as decimal(16,2)) AS BASICVALUE,
cast(SUM(TRDDISCVALUE) as decimal(16,2)) AS TRDDISCVALUE,
cast(SUM(PRICE_EQUALVALUE) as decimal(16,2)) AS [PRICE EQUAL VALUE],
cast(SUM(SEC_DISC_AMOUNT) as decimal(16,2)) AS [SEC DISC AMOUNT],
cast(SUM(SCH_SPL_VAL) as decimal(16,2)) AS [SCH SPL VAL],
cast(SUM(AMOUNT-TOTAL_TAXAMOUNT) as decimal(16,2)) AS TAXABLEVALUE,  
Concat([TAX1 COMPONENT],' ',cast([Tax1] as decimal(18,2))) as [TAX1 COMPONENT],
cast(SUM([TAX1 AMT]) as decimal(16,2)) AS [TAX1 AMT],
Concat([TAX2 COMPONENT],' ',cast([Tax 2] as decimal(18,2))) as [TAX2 COMPONENT],  
cast(SUM([TAX2 AMT]) as decimal(16,2)) AS [TAX2 AMT],
cast(SUM(AMOUNT) as decimal(16,2)) AS [INV VALUE] FROM VW_PURCHASEREGISTER WHERE CUSTOMER_CODE IN (SELECT SITEID FROM #TMPSITELIST)    
 AND [SALE_INVOICEDATE]>= @StartDate and [SALE_INVOICEDATE]<= @EndDate  
 GROUP BY TOTAL_TAX_PER,Concat([TAX1 COMPONENT],' ',cast([Tax1] as decimal(18,2))),Concat([TAX2 COMPONENT],' ',cast([Tax 2] as decimal(18,2)))  
END  