USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_SALESREGISTERMONTHLY]    Script Date: 10/7/2019 8:46:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ACX_USP_SALESREGISTERMONTHLY]    
 (    
 @SITEID NVARCHAR(Max),    
 @FROMDATE DATE,    
 @TODATE DATE,    
 @CustomerGroup nvarchar(20),    
 @Customer Nvarchar(20),    
 @ProductGroup nvarchar(100),    
 @SubCategory nvarchar(100),    
 @Product nvarchar(250),
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

      SELECT [INVOICE_DATE], [CUST_GROUP_CODE],[CUST GROUP],CUSTOMER_CODE , CUSTOMER_NAME,
	   [BU CODE], [BU NAME], PRODUCT_GROUP,PRODUCT_SUBCATEGORY,PRODUCT_CODE,PRODUCT_NAME,SUM(LTR) AS LTR,    
 SUM(AMOUNT) AS AMOUNT ,sum([AMOUNT]) as Total 
  FROM (  
   Select substring(convert(nvarchar, [INVOICE DATE],106),4,10) [INVOICE_DATE], [CUST_GROUP_CODE],[CUST GROUP],CUSTOMER_CODE , CUSTOMER_NAME, 
   BU_CODE [BU CODE], BU_DESC [BU NAME],PRODUCT_GROUP,PRODUCT_SUBCATEGORY,PRODUCT_CODE,PRODUCT_NAME,(LTR) AS LTR,    
 (AMOUNT) AS AMOUNT ,([AMOUNT]) as Total FROM VW_SALEREGISTER      
   where MONTH([INVOICE DATE])>=MONTH(@FROMDATE)  and MONTH([INVOICE DATE])<= MONTH(@TODATE)      
  and YEAR([INVOICE DATE])>=YEAR(@FROMDATE)  and  YEAR([INVOICE DATE])<=YEAR(@TODATE)    
  and [DIST. CODE]  IN (SELECT SITEID FROM #TMPSITELIST)       
  and [CUST_GROUP_CODE] like case when @CustomerGroup='' then '%' else @CustomerGroup end    
   and CUSTOMER_CODE like case when @Customer='' then '%' else @Customer end    
   AND BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)
   ) AS RST 
   WHERE --[CUST_GROUP_CODE] like case when @CustomerGroup='' then '%' else @CustomerGroup end    
  --and CUSTOMER_CODE like case when @Customer='' then '%' else @Customer end    
  PRODUCT_GROUP like case when @ProductGroup='' then '%' else @ProductGroup end    
  and PRODUCT_SUBCATEGORY like case when @SubCategory='' then '%' else @SubCategory end    
  and PRODUCT_NAME like case when @Product='' then '%' else @Product end      
   group by CUSTOMER_CODE,[CUST_GROUP_CODE],[CUST GROUP],CUSTOMER_NAME,[BU CODE],[BU NAME],
   PRODUCT_GROUP,PRODUCT_SUBCATEGORY,PRODUCT_CODE,PRODUCT_NAME,[INVOICE_DATE]
   order by [INVOICE_DATE] desc
END