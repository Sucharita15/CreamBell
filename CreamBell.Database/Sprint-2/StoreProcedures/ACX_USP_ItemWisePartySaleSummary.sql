﻿USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_ItemWisePartySaleSummary]    Script Date: 10/8/2019 11:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[ACX_USP_ItemWisePartySaleSummary]  
(@SITEID NVARCHAR(max),  
@FROMDATE DATETIME,  
@TODATE DATETIME,  
@CUST_GROUP nvarchar(40),  
@CUSTOMER_CODE nvarchar(40),
@BUCODE NVARCHAR(10)=''  
)  
as  
begin  

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

SELECT SR.[DIST. CODE] SITEID,BU_CODE [BU CODE], BU_DESC [BU NAME], SR.CUSTOMER_NAME,SR.[CUST GROUP] CUST_GROUP,SR.CUSTOMER_CODE,SR.PRODUCT_SUBCATEGORY,SR.NAMEALIAS,SR.AMOUNT,isnull(SR.BOX + SR.[SCHEME BOX] ,'0') as Box,
isnull(PCS + [SCHEME PCS],'0') as PCS,CONVERT(DECIMAL(9,2),isnull(BOXPCS,'0')) + CONVERT(DECIMAL(9,2),isnull([SCHEME BOXPCS],'0')) as [TotalBoxPCS],
[TOTAL QTY] as TotalQtyConv,SR.LTR from 
vw_SaleRegister SR
WHERE CUST_GROUP_CODE like case when @CUST_GROUP=''then '%' else @CUST_GROUP end  
AND CUSTOMER_CODE like case when  @CUSTOMER_CODE =''then '%' else @CUSTOMER_CODE end  
AND [INVOICE DATE] >= @FROMDATE and  [INVOICE DATE]<= @TODATE
AND [DIST. CODE] IN (SELECT SITEID FROM #TMPSITELIST)
AND BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)
ORDER BY CUSTOMER_NAME ASC   
end