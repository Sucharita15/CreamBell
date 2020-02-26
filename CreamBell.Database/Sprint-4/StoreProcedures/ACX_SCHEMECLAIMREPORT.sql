USE [7200]
GO
/****** Object:  StoredProcedure [ax].[ACX_SCHEMECLAIMREPORT]    Script Date: 11/12/2019 7:32:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[ax].[ACX_SCHEMECLAIMREPORT] '01-Aug-2018','31-Aug-2018','DL','','','',''      
ALTER PROCEDURE [ax].[ACX_SCHEMECLAIMREPORT]                        
(@FromDate smalldatetime,      
@ToDate smalldatetime,                    
@STATECODE NVARCHAR(20)='',                    
@SiteId Varchar(max)='',                       
@CUSTGROUP Varchar(MAX)='',      
@Scheme Varchar(MAX)='',        
@BUCODE NVARCHAR(MAX)=''                          
)                          
AS                        
BEGIN                         
-----STATE CODE--------------                          
IF OBJECT_ID('tempdb..#tmpState') IS NOT NULL BEGIN DROP TABLE #tmpState END                           
CREATE TABLE #tmpState (STATECODE NVARCHAR(20))                          
IF LEN(@STATECODE)>0                          
BEGIN INSERT INTO #tmpState SELECT ID FROM DBO.CommaDelimitedToTable(@STATECODE,',') END                          
ELSE                          
BEGIN INSERT INTO #tmpState SELECT DISTINCT STATECODE FROM AX.INVENTSITE WHERE LEN(ISNULL(STATECODE,''))>0  END                          
                          
--SELECT * FROM #TMPSTATE                          
                          
-------SITE---------------------------                          
IF OBJECT_ID('tempdb..#tmpsite') IS NOT NULL BEGIN DROP TABLE #tmpsite END                           
CREATE TABLE #tmpsite (SITEID NVARCHAR(20))                          
IF LEN(@SiteId)>0                          
BEGIN INSERT INTO #tmpsite SELECT DISTINCT ID FROM DBO.CommaDelimitedToTable(@SiteId,',') END                          
ELSE                          
BEGIN INSERT INTO #tmpsite SELECT DISTINCT SITEID FROM AX.INVENTSITE WHERE LEN(SITEID)>0 AND STATECODE IN (SELECT STATECODE FROM #tmpState) END                          
--SELECT * FROM #tmpsite                           
-------CUSTOMER GROUP NAME-------                          
                          
IF OBJECT_ID('tempdb..#tmpGroupName') IS NOT NULL BEGIN DROP TABLE #tmpGroupName END                           
CREATE TABLE #tmpGroupName (CUSTOMER_CODE NVARCHAR(255))                          
IF LEN(@CUSTGROUP)>0                          
BEGIN INSERT INTO #tmpGroupName SELECT ID FROM DBO.CommaDelimitedToTable(@CUSTGROUP,',')  END                          
ELSE                          
BEGIN INSERT INTO #tmpGroupName SELECT DISTINCT CUSTGROUP_CODE FROM [ax].[ACXCUSTGROUPMASTER] WHERE LEN(CUSTGROUP_CODE)>0  END                          
--SELECT * FROM #tmpGroupName                          
IF OBJECT_ID('TEMPDB..#TMPScheme') IS NOT NULL BEGIN DROP TABLE #TMPScheme END                          
CREATE TABLE #TMPScheme (Scheme NVARCHAR(100))                          
IF LEN(@Scheme)>0                          
BEGIN INSERT INTO #TMPScheme SELECT id from [dbo].[CommaDelimitedToTable](@Scheme,',') END                          
ELSE                          
BEGIN           
INSERT INTO #TMPScheme select distinct Schemecode from ACXAllSCHEMEVIEW           
INSERT INTO #TMPScheme VALUES ('')            
END                          
            
-------BUCODE-------          
IF OBJECT_ID('tempdb..#tmpBuCode') IS NOT NULL BEGIN DROP TABLE #tmpBuCode  END           
CREATE TABLE #tmpBuCode (BU_CODE NVARCHAR(255))          
IF LEN(@bucode)>0          
BEGIN INSERT INTO #tmpBuCode SELECT ID FROM DBO.CommaDelimitedToTable(@bucode,',')         
END         
else        
BEGIN INSERT INTO #tmpBuCode SELECT distinct BU_CODE FROM AX.ACXSITEBUMAPPING WHERE SITEID in(SELECT SITEID FROM #tmpsite)        
END        
        
        
            
IF OBJECT_ID('TEMPDB..#TMPSCHCAL') IS NOT NULL BEGIN DROP TABLE #TMPSCHCAL END            
            
SELECT R.*,ISNULL(S.PRODUCT_CODE,'') PRODUCT_CODE,ISNULL(S.PRODUCT_GROUP,'') PRODUCT_GROUP,ISNULL(PRODUCT_NAME,'') PRODUCT_NAME,            
ISNULL(BASICRATE,0) BASICRATE,ISNULL([TOTAL QTY],0) [TOTAL QTY],ISNULL(LTR,0) LTR,            
ISNULL((SELECT TOP 1 [SCHEME DESCRIPTION] FROM ACXAllSCHEMEVIEW SV WHERE R.SCHEMECODE=SV.SCHEMECODE),'') [SCHEME DESCRIPTION],            
ISNULL((SELECT TOP 1 CASE [Scheme Type] WHEN 0 THEN 'Quantity Based' WHEN 1 THEN 'Value Based' WHEN 2 THEN 'Percent Based' WHEN 3 THEN 'ValueOff Based' END              
FROM ACXAllSCHEMEVIEW SV WHERE R.SCHEMECODE=SV.SCHEMECODE),'') [SCHEME TYPE],            
isnull((SELECT TOP 1 MINIMUMQUANTITY +MINIMUMVALUE + MINIMUMQUANTITYPCS FROM ACXAllSCHEMEVIEW SV WHERE R.SCHEMECODE=SV.SCHEMECODE AND SV.[SCHEME TYPE]=R.SCHEMETYPE                
AND R.SCHREFRECID=SV.SCHEMELINENO),0) [SLAB],            
isnull((SELECT TOP 1 CASE WHEN MINIMUMQUANTITY>0 THEN 'BOX' WHEN MINIMUMVALUE>0 THEN 'VALUE' WHEN MINIMUMQUANTITYPCS>0 THEN 'PCS' END             
FROM ACXAllSCHEMEVIEW SV WHERE R.SCHEMECODE=SV.SCHEMECODE AND SV.[SCHEME TYPE]=R.SCHEMETYPE  AND R.SCHREFRECID=SV.SCHEMELINENO),'') [SLAB TYPE],               
isnull((SELECT TOP 1 FREEQTY+FREEQTYPCS+PERCENTSCHEME FROM ACXAllSCHEMEVIEW SV WHERE R.SCHEMECODE=SV.SCHEMECODE AND SV.[SCHEME TYPE]=R.SCHEMETYPE                
AND R.SCHREFRECID=SV.SCHEMELINENO),0) SCHEME,                
isnull((SELECT TOP 1 CASE WHEN PERCENTSCHEME>0 THEN 'Percent' WHEN FREEQTY+FREEQTYPCS>0 THEN 'BOX/PCS' ELSE '' END                  
FROM ACXAllSCHEMEVIEW SV WHERE R.SCHEMECODE=SV.SCHEMECODE AND SV.[SCHEME TYPE]=R.SCHEMETYPE                
AND R.SCHREFRECID=SV.SCHEMELINENO),'') [SCHEME BASED]            
INTO #TMPSCHCAL            
 FROM (            
SELECT *,(SELECT TOP 1 CASE WHEN ISNULL(REFSCHEMECODE,'')='' THEN SV.SCHEMECODE ELSE REFSCHEMECODE END            
FROM FLAT_SALEREGISTER SV WHERE SV.[DIST. CODE]=RST.[DIST. CODE] AND SV.INVOICE_NO=RST.INVOICE_NO ) SCHEMECODE,            
(SELECT TOP 1 SCHEMETYPE FROM FLAT_SALEREGISTER SV WHERE SV.[DIST. CODE]=RST.[DIST. CODE] AND SV.INVOICE_NO=RST.INVOICE_NO ) SCHEMETYPE,            
(SELECT TOP 1 SCHREFRECID FROM FLAT_SALEREGISTER SV WHERE SV.[DIST. CODE]=RST.[DIST. CODE] AND SV.INVOICE_NO=RST.INVOICE_NO ) SCHREFRECID            
 FROM (            
SELECT [DIST. CODE],[DIST. NAME],[CUST GROUP],SR.[CUSTOMER_CODE],SR.[CUSTOMER_NAME],TRANTYPE DOCUMENT_TYPE,[INVOICE_NO],[INVOICE DATE],                        
[REF DOC NO],[REF DOC DATE],SUM(AMOUNT) [INVOICE VALUE],      
SUM([ADD SCHEME DISC AMT]) [ADD SCH VALUE],      
SUM([SCHEME DISC AMT]) [EXPENSE VALUE]                
FROM FLAT_SALEREGISTER SR       
INNER JOIN #tmpsite TS ON TS.SITEID = SR.[DIST. CODE]     
AND [INVOICE DATE]>='2017-07-01'            
AND [INVOICE DATE]>=@FromDate AND [INVOICE DATE]<=@ToDate          
INNER JOIN  #tmpBuCode BU ON SR.BU_CODE = BU.BU_CODE      
INNER JOIN #tmpGroupName TGN ON TGN.CUSTOMER_CODE = SR.[CUST_GROUP_CODE]      
WHERE [INVOICE DATE]>='2017-07-01'            
AND [INVOICE DATE]>=@FromDate AND [INVOICE DATE]<=@ToDate         
GROUP BY [DIST. CODE],[DIST. NAME],[CUST GROUP],SR.[CUSTOMER_CODE],SR.[CUSTOMER_NAME],TRANTYPE,[INVOICE_NO],[INVOICE DATE],                        
[REF DOC NO],[REF DOC DATE] HAVING SUM([SCHEME DISC AMT]+[ADD SCHEME DISC AMT])<>0)  RST            
) R            
LEFT OUTER JOIN FLAT_SALEREGISTER S ON S.[DIST. CODE]=R.[DIST. CODE] AND S.INVOICE_NO=R.INVOICE_NO AND S.SCHEMECODE=R.SCHEMECODE AND R.SCHEMECODE<>''          
      
SELECT [DIST. CODE],[DIST. NAME],[CUST GROUP],CUSTOMER_CODE,CUSTOMER_NAME,DOCUMENT_TYPE,INVOICE_NO,[INVOICE DATE],[REF DOC NO],            
[REF DOC DATE],[INVOICE VALUE],SCHEMECODE,[SCHEME DESCRIPTION],[Scheme Type],[Slab],[Slab Type],T.[Scheme],[SCHEME BASED],            
PRODUCT_GROUP,PRODUCT_CODE,PRODUCT_NAME,BASICRATE,[TOTAL QTY],LTR,BASICRATE*[TOTAL QTY] [BASIC VALUE],[ADD SCH VALUE],      
[EXPENSE VALUE]/(SELECT COUNT(*) FROM #TMPSCHCAL C WHERE C.INVOICE_NO=T.INVOICE_NO AND C.[DIST. CODE]=T.[DIST. CODE]) [SCHEME VALUE],      
([ADD SCH VALUE] + [EXPENSE VALUE]/(SELECT COUNT(*) FROM #TMPSCHCAL C WHERE C.INVOICE_NO=T.INVOICE_NO AND C.[DIST. CODE]=T.[DIST. CODE])) [TOTAL EXPENSE]      
FROM #TMPSCHCAL T            
JOIN #TMPScheme TSS ON T.SCHEMECODE = TSS.Scheme      
--WHERE            
--SCHEMECODE IN (SELECT Scheme FROM #TMPScheme)            
ORDER BY [DIST. CODE],[INVOICE_NO]            
END 