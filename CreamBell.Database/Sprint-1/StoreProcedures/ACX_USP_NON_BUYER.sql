﻿USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_NON_BUYER]    Script Date: 8/31/2019 8:32:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC ACX_USP_NON_BUYER '0000600753,0000600752,0000600757,10000583,10000582,10000585,10048626,10000250,0000601995,10000251,0000601457,10000587,10052107,0000601435','01-Apr-2018','15-Apr-2018','','','','','DL','70000771',3             
--ACX_USP_NON_BUYER '','01-FEB-2019','28-FEB-2019','','','','','DL','0000600750','2','BU2'
ALTER PROCEDURE [dbo].[ACX_USP_NON_BUYER]          
( @SITEID nvarchar(max)='', @STARTDATE smalldatetime , @ENDDATE smalldatetime, @Category nvarchar(20)='',          
@SubCategory nvarchar(50)='', @Product nvarchar(10)='', @CustGroup nvarchar(10)='', @StateCode nvarchar(10)='',      
@UserCode nvarchar(50)='', @UserType nvarchar(50)='',@BUCODE NVARCHAR(10)='')      
AS        
--DECLARE @SITEID nvarchar(max)='', @STARTDATE smalldatetime='01-FEB-2019' ,  
--@ENDDATE smalldatetime='16-FEB-2019', @Category nvarchar(20)='',            
--@SubCategory nvarchar(50)='', @Product nvarchar(10)='', @CustGroup nvarchar(10)='', @StateCode nvarchar(10)='',        
--@UserCode nvarchar(50)='KUMAR', @UserType nvarchar(50)='1',@BUCODE NVARCHAR(10)=''  
print 'start'
Print CONVERT(VARCHAR(8), GETDATE(), 108)        
IF OBJECT_ID('TEMPDB..#TMPCHNGROUP') IS NOT NULL BEGIN DROP TABLE #TMPCHNGROUP END           
CREATE TABLE #TMPCHNGROUP (CHANNELGROUPCODE NVARCHAR(50));          
        
IF (@UserType != 3)           
BEGIN   INSERT INTO #TMPCHNGROUP SELECT DISTINCT CUST_GROUP FROM AX.ACXCUSTMASTER  END        
ELSE        
BEGIN  INSERT INTO #TMPCHNGROUP SELECT CHANNELGROUPCODE FROM DBO.UDF_GETCHANNELGROUPBYUSER(@UserCode)  END        
  
IF OBJECT_ID('TEMPDB..#TMPCUSTGROUP') IS NOT NULL BEGIN DROP TABLE #TMPCUSTGROUP END           
CREATE TABLE #TMPCUSTGROUP (CUSTGROUP NVARCHAR(50));          
        
IF (LEN(@CustGroup)>0)           
BEGIN   INSERT INTO #TMPCUSTGROUP SELECT @CustGroup END        
ELSE        
BEGIN  INSERT INTO #TMPCUSTGROUP SELECT DISTINCT CUST_GROUP FROM AX.ACXCUSTMASTER  END        
          
IF OBJECT_ID('TEMPDB..#TMPSTATE') IS NOT NULL BEGIN DROP TABLE #TMPSTATE END           
CREATE TABLE #TMPSTATE (STATEID NVARCHAR(50));          
        
IF LEN(@StateCode)>0          
BEGIN   INSERT INTO #TMPSTATE SELECT ID FROM DBO.CommaDelimitedToTable(@StateCode,',') END          
ELSE          
BEGIN INSERT INTO #TMPSTATE SELECT DISTINCT STATEID FROM AX.LOGISTICSADDRESSSTATE WHERE LEN(STATEID)>0 END           
  
IF OBJECT_ID('TEMPDB..#TMPPRODGROUP') IS NOT NULL BEGIN DROP TABLE #TMPPRODGROUP END          
CREATE TABLE #TMPPRODGROUP (PRODUCTGROUP NVARCHAR(50));                    
IF LEN(@Category)>0  
INSERT INTO #TMPPRODGROUP SELECT @Category  
ELSE   
INSERT INTO #TMPPRODGROUP SELECT DISTINCT PRODUCT_GROUP FROM AX.INVENTTABLE  
  
IF OBJECT_ID('TEMPDB..#TMPPRODSUBGROUP') IS NOT NULL BEGIN DROP TABLE #TMPPRODSUBGROUP END          
CREATE TABLE #TMPPRODSUBGROUP (SUBPRODUCTGROUP NVARCHAR(50));                    
IF LEN(@SubCategory)>0  
INSERT INTO #TMPPRODSUBGROUP SELECT @SubCategory  
ELSE   
INSERT INTO #TMPPRODSUBGROUP SELECT DISTINCT PRODUCT_SUBCATEGORY FROM AX.INVENTTABLE  
  
IF OBJECT_ID('TEMPDB..#TMPPROD') IS NOT NULL BEGIN DROP TABLE #TMPPROD END          
CREATE TABLE #TMPPROD (PRODUCTCODE NVARCHAR(50));                    
IF LEN(@Product)>0  
INSERT INTO #TMPPROD SELECT @Product  
ELSE   
INSERT INTO #TMPPROD SELECT DISTINCT ITEMID FROM AX.INVENTTABLE  
          
IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
          
IF LEN(@SITEID)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SITEID,',')   END          
ELSE IF (@UserType != 3)          
BEGIN INSERT INTO #TMPSITELIST select SITEID from AX.InventSite IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE WHERE LEN(SITEID)>0  AND STATECODE IN (SELECT STATEID FROM #TMPSTATE) AND  UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' END  
ELSE           
--BEGIN INSERT INTO #TMPSITELIST SELECT Distinct DISTRIBUTOR  FROM DBO.UDF_GetSALESHIERARCHY(@UserCode) WHERE [STATE] IN (SELECT STATEID FROM #TMPSTATE)  END          
BEGIN INSERT INTO #TMPSITELIST select Distinct SITEID from AX.InventSite IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE WHERE LEN(SITEID)>0  AND STATECODE IN (SELECT STATEID FROM #TMPSTATE)  
AND  UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND IV.SITEID IN (SELECT Distinct DISTRIBUTOR  FROM DBO.UDF_GetSALESHIERARCHY(@UserCode) WHERE [STATE] IN (SELECT STATEID FROM #TMPSTATE)) END  
  
IF OBJECT_ID('tempdb..#tmpBuCode') IS NOT NULL BEGIN DROP TABLE #tmpBuCode  END     
CREATE TABLE #tmpBuCode (BU_CODE NVARCHAR(10))    
IF LEN(@BUCODE)>0    
BEGIN INSERT INTO #tmpBuCode SELECT ID FROM DBO.CommaDelimitedToTable(@BUCODE,',')   
END    
ELSE  
BEGIN INSERT INTO #tmpBuCode SELECT distinct BU_CODE FROM AX.ACXSITEBUMAPPING WHERE SITEID IN (SELECT SITEID FROM #TMPSITELIST)  
END  
  
  
IF OBJECT_ID('TEMPDB..#TMPREVERSAL') IS NOT NULL BEGIN DROP TABLE #TMPREVERSAL END                
IF OBJECT_ID('TEMPDB..#TMPSALE') IS NOT NULL BEGIN DROP TABLE #TMPSALE END           
IF OBJECT_ID('TEMPDB..#TMPALLSALE') IS NOT NULL BEGIN DROP TABLE #TMPALLSALE END                
IF OBJECT_ID('TEMPDB..#TMPCUSTLIST') IS NOT NULL BEGIN DROP TABLE #TMPCUSTLIST END          
CREATE TABLE #TMPCUSTLIST (CUSTOMER_CODE NVARCHAR(20),SITEID NVARCHAR(20),PURCHASEDATE SMALLDATETIME,ORDERDATE SMALLDATETIME);     
Print 'All'               
PRINT CONVERT(VARCHAR(8), GETDATE(), 108)     

SELECT  SH.CUSTOMER_CODE,SH.SITEID [DIST. CODE],SH.SO_NO  [INVOICE_NO],SH.TranType, SH.INVOIC_DATE,     
SUM(SL.AMOUNT) INVAMT INTO #TMPALLSALE FROM AX.ACXSALEINVOICEHEADER SH JOIN AX.ACXSALEINVOICELINE SL 
ON SL.INVOICE_NO=SH.INVOICE_NO AND SH.SITEID=SL.SITEID  AND SH.INVOIC_DATE >=@STARTDATE AND SH.INVOIC_DATE<=@ENDDATE        
JOIN #TMPSITELIST S ON S.SITEID=SH.SITEID      
JOIN AX.INVENTTABLE I ON I.ITEMID=SL.PRODUCT_CODE
JOIN #TMPPRODGROUP PG ON PG.PRODUCTGROUP=I.PRODUCT_GROUP
JOIN #TMPPRODSUBGROUP PSG ON PSG.SUBPRODUCTGROUP=I.PRODUCT_SUBCATEGORY  
JOIN #TMPPROD PR ON PR.PRODUCTCODE=SL.PRODUCT_CODE  
WHERE I.BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)  
GROUP BY SH.CUSTOMER_CODE,SH.SITEID,SH.SO_NO, SH.TranType,SH.INVOIC_DATE

print 'after all'
Print CONVERT(VARCHAR(8), GETDATE(), 108)
SELECT DISTINCT CUSTOMER_CODE,[DIST. CODE],[INVOICE_NO], INVAMT INTO #TMPREVERSAL FROM #TMPALLSALE
WHERE TranType=2 
--GROUP BY CUSTOMER_CODE,[DIST. CODE],[INVOICE_NO] 

print 'after revers'
Print CONVERT(VARCHAR(8), GETDATE(), 108)

SELECT DISTINCT CUSTOMER_CODE,[DIST. CODE],[INVOICE_NO], INVAMT INTO #TMPSALE FROM #TMPALLSALE
WHERE TranType<>2

print 'after sale'
Print CONVERT(VARCHAR(8), GETDATE(), 108)
--GROUP BY CUSTOMER_CODE,[DIST. CODE],[INVOICE_NO] 
  
--SELECT DISTINCT SH.CUSTOMER_CODE,SH.SITEID [DIST. CODE],SH.SO_NO  [INVOICE_NO],      
--SUM(SL.AMOUNT) INVAMT INTO #TMPREVERSAL FROM AX.ACXSALEINVOICEHEADER SH JOIN AX.ACXSALEINVOICELINE SL 
--ON SL.INVOICE_NO=SH.INVOICE_NO AND SH.SITEID=SL.SITEID  AND SH.INVOIC_DATE >=@STARTDATE AND SH.INVOIC_DATE<=@ENDDATE        
--JOIN #TMPSITELIST S ON S.SITEID=SH.SITEID
--AND SH.TranType=2       
--JOIN AX.INVENTTABLE I ON I.ITEMID=SL.PRODUCT_CODE
--JOIN #TMPPRODGROUP PG ON PG.PRODUCTGROUP=I.PRODUCT_GROUP
--JOIN #TMPPRODSUBGROUP PSG ON PSG.SUBPRODUCTGROUP=I.PRODUCT_SUBCATEGORY  
--JOIN #TMPPROD PR ON PR.PRODUCTCODE=SL.PRODUCT_CODE  
--WHERE I.BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)  
--GROUP BY SH.CUSTOMER_CODE,SH.SITEID,SH.SO_NO
      
--PRINT GETDATE()      
--SELECT DISTINCT SH.CUSTOMER_CODE,SH.SITEID [DIST. CODE],SH.SO_NO  [INVOICE_NO],      
--SUM(SL.AMOUNT) INVAMT INTO #TMPSALE FROM AX.ACXSALEINVOICEHEADER SH JOIN AX.ACXSALEINVOICELINE SL 
--ON SL.INVOICE_NO=SH.INVOICE_NO AND SH.SITEID=SL.SITEID  AND SH.INVOIC_DATE >=@STARTDATE AND SH.INVOIC_DATE<=@ENDDATE        
--JOIN #TMPSITELIST S ON S.SITEID=SH.SITEID
--AND SH.TranType<>2       
--JOIN AX.INVENTTABLE I ON I.ITEMID=SL.PRODUCT_CODE
--JOIN #TMPPRODGROUP PG ON PG.PRODUCTGROUP=I.PRODUCT_GROUP
--JOIN #TMPPRODSUBGROUP PSG ON PSG.SUBPRODUCTGROUP=I.PRODUCT_SUBCATEGORY  
--JOIN #TMPPROD PR ON PR.PRODUCTCODE=SL.PRODUCT_CODE  
--WHERE I.BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)  
--GROUP BY SH.CUSTOMER_CODE,SH.SITEID,SH.SO_NO

--SELECT DISTINCT INV.CUSTOMER_CODE,[DIST. CODE],[INVOICE_NO],      
--SUM(AMOUNT) INVAMT INTO #TMPSALE FROM VW_SALEREGISTER INV        
--JOIN #TMPSITELIST SL ON SL.SITEID=INV.[DIST. CODE]          
--AND INV.[INVOICE DATE] >=@STARTDATE AND INV.[INVOICE DATE]<=@ENDDATE        
--AND TRANTYPE<>'Sale Reversal'        
--JOIN #TMPPRODGROUP PG ON PG.PRODUCTGROUP=INV.PRODUCT_GROUP  
--JOIN #TMPPRODSUBGROUP PSG ON PSG.SUBPRODUCTGROUP=INV.PRODUCT_SUBCATEGORY  
--JOIN #TMPPROD PR ON PR.PRODUCTCODE=INV.PRODUCT_CODE  
--WHERE INV.BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)  
----AND INV.[DIST. CODE] IN (SELECT SITEID FROM #TMPSITELIST)          
----WHERE    
------AND     
------PRODUCT_GROUP LIKE CASE WHEN LEN(@Category)>0 THEN @Category ELSE '%' END          
------AND PRODUCT_SUBCATEGORY LIKE CASE WHEN LEN(@SubCategory)>0 THEN @SubCategory ELSE '%' END         AND   
----PRODUCT_CODE LIKE CASE WHEN LEN(@Product)>0 THEN @Product ELSE '%' END        
--GROUP BY CUSTOMER_CODE,[DIST. CODE],[INVOICE_NO]      
print 'before cust'
Print CONVERT(VARCHAR(8), GETDATE(), 108)   
  
INSERT INTO #TMPCUSTLIST (CUSTOMER_CODE,SITEID)       
SELECT CUSTOMER_CODE,SITE_CODE FROM AX.ACXCUSTMASTER WHERE       
CONCAT(CUSTOMER_CODE,SITE_CODE) NOT IN (      
SELECT DISTINCT CONCAT(INV.CUSTOMER_CODE,INV.[DIST. CODE]) FROM #TMPSALE INV      
LEFT OUTER JOIN #TMPREVERSAL SR ON         
INV.[DIST. CODE]=SR.[DIST. CODE] AND SR.[INVOICE_NO]=INV.[INVOICE_NO]      
AND SR.[CUSTOMER_CODE]=INV.[CUSTOMER_CODE]      
WHERE ABS(INV.INVAMT+ISNULL(SR.INVAMT,0))>1)      
print 'after cust'
Print CONVERT(VARCHAR(8), GETDATE(), 108)    
    
UPDATE CU SET     
PURCHASEDATE=(select top 1 invoic_date from #TMPALLSALE    
 where siteid=cu.SITEID AND INVOIC_DATE <@STARTDATE     
 AND customer_code=cu.customer_code order by invoic_date desc)    ,
 ORDERDATE=(select top 1 SO_DATE from vw_salesheader    
 where siteid=cu.SITEID --AND SO_DATE <@STARTDATE     
 AND customer_code=cu.customer_code order by SO_DATE desc)   
FROM #TMPCUSTLIST CU    
 
print 'after purchase update'
Print CONVERT(VARCHAR(8), GETDATE(), 108)  


--UPDATE CU SET     
--ORDERDATE=(select top 1 SO_DATE from vw_salesheader    
-- where siteid=cu.SITEID --AND SO_DATE <@STARTDATE     
-- AND customer_code=cu.customer_code order by SO_DATE desc)    
--FROM #TMPCUSTLIST CU    

print 'after order update'
Print CONVERT(VARCHAR(8), GETDATE(), 108)  

--SELECT * FROM #TMPCUSTLIST  
--SELECT *  
--FROM #TMPCUSTLIST CL       
--JOIN [ax].[ACXCUSTMASTER] CU ON CL.CUSTOMER_CODE=CU.CUSTOMER_CODE            
--JOIN #TMPSITELIST SL ON CU.SITE_CODE=SL.SITEID      
--JOIN ax.acxpsrmaster p on cu.psr_code=p.psr_code            
--JOIN ax.acxpsrbeatmaster b on cu.psr_Beat=BeatCode  AND CU.SITE_CODE=b.Site_Code and cu.psr_code=b.PSRcode       
  
      
BEGIN                  
SELECT ROW_NUMBER() OVER (ORDER BY CU.SITE_CODE,CU.CUSTOMER_NAME) AS SRLNO,  
CL.CUSTOMER_CODE,        
CU.CUSTOMER_NAME,CU.DEEP_FRIZER AS [DEEP FREEZER],        
CASE WHEN CU.applicableschemediscount = 2 OR CU.applicableschemediscount = 3 THEN 'Yes' else 'No' END as [SCHEME DISCOUNT],          
CU.SITE_CODE,CU.ADDRESS1,CU.PSR_CODE PSRcode,CU.PSR_BEAT BeatCode,  
BeatName,cu.PSR_CODE,PSR_Name ,CU.PSR_BEAT,    
CL.PURCHASEDATE     Purchase_Date,   
CONVERT(VARCHAR(10), CL.ORDERDATE, 105) [Last Visit],  
CU.SITENAME    
FROM #TMPCUSTLIST CL  
LEFT OUTER JOIN VW_CUSTOMERMASTER CU ON CL.CUSTOMER_CODE=CU.CUSTOMER_CODE           
WHERE SITEID IN (SELECT SITEID FROM #TMPSITELIST)  
AND CU.CUST_GROUP IN (SELECT CUSTGROUP FROM #TMPCUSTGROUP)  
AND CU.[CUST_GROUP] IN (SELECT CHANNELGROUPCODE FROM #TMPCHNGROUP)          
AND CU.BLOCKED <> 1          
END 
print 'End'
Print CONVERT(VARCHAR(8), GETDATE(), 108)  