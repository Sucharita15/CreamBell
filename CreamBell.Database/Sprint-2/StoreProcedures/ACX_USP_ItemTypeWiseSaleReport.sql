USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_ItemTypeWiseSaleReport]    Script Date: 10/8/2019 11:22:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[ACX_USP_ItemTypeWiseSaleReport]  
 (  
 @SiteId nvarchar(max),  
 @FromDate date,  
 @ToDate date,  
 @CustomerGroup NVARCHAR(20) = '',  
 @Customer NVARCHAR(20) ='',
 @BUCODE NVARCHAR(10)=''
 )  
 As   
 Begin  

  
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
 BEGIN INSERT INTO #tmpBuCode SELECT BU_CODE FROM AX.ACXSITEBUMAPPING WHERE SITEID IN (SELECT SITEID FROM #TMPSITELIST)
 END

 SELECT BU_CODE [BU CODE], BU_DESC [BU NAME],TRANTYPE,[DIST. CODE],[CUST GROUP],CUSTOMER_NAME,CUST_GROUP_CODE, PRODUCT_SUBCATEGORY, AMOUNT,BOX, PCS,cast([BoxPCS] as decimal(9,2)) as [BoxPCS] ,   
  Cast([TOTAL QTY] as decimal(9,2)) as TotalQtyConv, LTR  from vw_saleRegister  
  where [DIST. CODE] IN (SELECT SITEID FROM #TMPSITELIST) and [INVOICE DATE] >= @FromDate  
  and [INVOICE DATE] <= @ToDate and [CUST_GROUP_CODE] like case when @CustomerGroup='' then '%' else @CustomerGroup end    
   and CUSTOMER_CODE like case when @Customer='' then '%' else @Customer end  
   AND BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)
    ORDER BY [INVOICE DATE] ASC , [INVOICE_NO] ASC  
 end