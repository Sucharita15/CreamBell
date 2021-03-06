﻿USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[SP_SALEREGISTERITEMWISE]    Script Date: 10/8/2019 10:45:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[SP_SALEREGISTERITEMWISE]
(
@siteid NVARCHAR(max),
@begindate DATE,
@enddate DATE,
@customergroupname nvarchar(50) = '',
@BUCODE NVARCHAR(10)=''
)
AS
BEGIN

IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@SITEID)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SITEID,',')   END     

-------BUCODE-------  
   IF OBJECT_ID('tempdb..#tmpBuCode') IS NOT NULL BEGIN DROP TABLE #tmpBuCode  END   
   CREATE TABLE #tmpBuCode (BU_CODE NVARCHAR(10))  
   IF LEN(@BUCODE)>0  
   BEGIN INSERT INTO #tmpBuCode SELECT ID FROM DBO.CommaDelimitedToTable(@BUCODE,',') 
   END  
   ELSE
   BEGIN INSERT INTO #tmpBuCode SELECT BU_CODE FROM AX.ACXSITEBUMAPPING WHERE SITEID  IN (SELECT SITEID FROM #TMPSITELIST)
   END
 
SELECT [DIST. CODE] SITEID,[DIST. NAME],BU_CODE [BU CODE], BU_DESC [BU NAME],TRANTYPE,CUST_GROUP_CODE,[CUST GROUP],CUSTOMER_CODE,CUSTOMER_NAME,PRODUCT_NAME,INVOICE_NO,[INVOICE DATE],BOX+[SCHEME BOX] BOX,PCS+[SCHEME PCS] PCS,  
  CONVERT(DECIMAL(18,6),CASE WHEN BOXPCS<>'0' THEN [BOXPCS] ELSE [SCHEME BOXPCS] END) [TotalBoxPCS],[TOTAL QTY] TotalQtyConv,LTR,[LINE AMOUNT] [LINEAMOUNT],DISC_AMOUNT,
  [DISC2 AMT] + [SCHEME DISC AMT] as [Misc Disc Others],[DISC%],[TAX1 COMPONENT] ,[TAX2 COMPONENT] ,
  [TD%] TD_Per,[PE%] [PE_Per],[TD AMT] tdvalue,[PE AMT],[TAX1%] [TAX_CODE],[TAX1 AMT] TAX_AMOUNT,[TAX2%] ADDTAX_CODE,[TAX2 AMT] ADDTAX_AMOUNT,AMOUNT  
 FROM vw_SaleRegister WHERE [DIST. CODE] IN (SELECT SITEID FROM #TMPSITELIST) AND [INVOICE DATE]>=@begindate AND [INVOICE DATE]<=@enddate and CUST_GROUP_CODE like case when  @customergroupname = '' then '%' else @customergroupname end 
 AND BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)
END