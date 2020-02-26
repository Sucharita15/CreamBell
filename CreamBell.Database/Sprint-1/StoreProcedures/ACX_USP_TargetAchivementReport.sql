USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_TargetAchivementReport]    Script Date: 9/11/2019 8:28:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec ACX_USP_TargetAchivementReport '','2018-11-01','2018-12-30','','','','','P00030','P00030','',''       

ALTER Procedure [dbo].[	]       

(@SiteId nvarchar(max)='',@FromDate smalldatetime='',@ToDate smalldatetime='',            

@StateId nvarchar(10)='',@ObjectCode nvarchar(20)='',@TargetCategory nvarchar(10)='',      

@TargetSubCategory nvarchar(10)='',@targetsubobject nvarchar(20)='',      

@UserCode nvarchar(50)='',@UserType nvarchar(50)='',@BUCODE NVARCHAR(10)='') as          

--DECLARE @SiteId nvarchar(max),@FromDate smalldatetime='2018-11-01',      

--@ToDate smalldatetime='2018-11-30',@StateId nvarchar(10)='',      

--@ObjectCode nvarchar(20)='',@TargetCategory nvarchar(10)='',      

--@TargetSubCategory nvarchar(10)='',@targetsubobject nvarchar(20)='P00061',      

--@UserCode nvarchar(50)='P00061',@UserType nvarchar(50)='0',@BUCODE NVARCHAR(10)=''    

Begin           

--select getdate()        

IF (SELECT COUNT(PSR_CODE) FROM AX.ACXPSRMASTER WHERE PSR_CODE=@targetsubobject)>0 

BEGIN

SELECT * FROM AX.ACXPSR_PROCESS_TARGETACHIVEMENT WHERE targetsubobject=@targetsubobject

END

ELSE

BEGIN

IF OBJECT_ID('tempdb..#tmpState') IS NOT NULL BEGIN DROP TABLE #tmpState END               

CREATE TABLE #tmpState (STATECODE NVARCHAR(max))            

IF LEN(@StateId)>0            

BEGIN INSERT INTO #tmpState SELECT ID FROM DBO.CommaDelimitedToTable(@StateId,',') END            

ELSE            

BEGIN INSERT INTO #tmpState SELECT DISTINCT STATEID FROM AX.LOGISTICSADDRESSSTATE WHERE LEN(STATEID)>0 END             

IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END                

CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(50));              

IF LEN(@SITEID)>0               

BEGIN INSERT INTO #TMPSITELIST SELECT ID FROM DBO.CommaDelimitedToTable(@SiteId,',') END            

ELSE IF (@UserType != 3)              

BEGIN  INSERT INTO #TMPSITELIST SELECT SITEID FROM AX.InventSite WHERE LEN(SITEID)>0 AND STATECODE IN (SELECT STATECODE FROM #tmpState)          

END            

ELSE            

BEGIN  INSERT INTO #TMPSITELIST SELECT Distinct DISTRIBUTOR  FROM DBO.UDF_GetSALESHIERARCHY(@UserCode) WHERE STATE IN  (SELECT STATECODE FROM #tmpState)  END                 

IF OBJECT_ID('TEMPDB..#TMPCHNGROUP') IS NOT NULL BEGIN DROP TABLE #TMPCHNGROUP END                  

CREATE TABLE #TMPCHNGROUP (CHANNELGROUPCODE NVARCHAR(50));                

IF (@UserType != 3)                

BEGIN INSERT INTO #TMPCHNGROUP SELECT DISTINCT CUST_GROUP FROM AX.ACXCUSTMASTER END              

ELSE              

BEGIN INSERT INTO #TMPCHNGROUP SELECT CHANNELGROUPCODE FROM DBO.UDF_GETCHANNELGROUPBYUSER(@UserCode) END              

IF OBJECT_ID('tempdb..#tmptarget') IS NOT NULL BEGIN DROP TABLE #tmptarget  END      

IF OBJECT_ID('tempdb..#tmpsaleview') IS NOT NULL BEGIN DROP TABLE #tmpsaleview END      

create table #tmpsaleview ([DIST. CODE] nvarchar(20),[INVOICE DATE] smalldatetime,PRODUCT_CODE nvarchar(20),       

PSR_CODE nvarchar(20),CUST_GROUP_CODE nvarchar(20),CUSTOMER_CODE nvarchar(20),[TOTAL QTY] decimal(32,16),      

[LTR] decimal(32,16),[AMOUNT] decimal(32,16))    

-------BUCODE-------        

IF OBJECT_ID('tempdb..#tmpBuCode') IS NOT NULL BEGIN DROP TABLE #tmpBuCode  END         

CREATE TABLE #tmpBuCode (BU_CODE NVARCHAR(10))        

IF LEN(@BUCODE)>0        

BEGIN INSERT INTO #tmpBuCode SELECT ID FROM DBO.CommaDelimitedToTable(@BUCODE,',')       

END        

ELSE      

BEGIN INSERT INTO #tmpBuCode SELECT distinct BU_CODE FROM AX.ACXSITEBUMAPPING WHERE SITEID IN (SELECT SITEID FROM #TMPSITELIST)      

END      

--select getdate()        

--IF (SELECT COUNT(*) FROM AX.ACXPSRMASTER WHERE PSR_CODE=@UserCode)>0    

--BEGIN    

--insert into #tmpsaleview    

--SELECT SR.[DIST. CODE],[INVOICE DATE],SR.PRODUCT_CODE,      

--PSR_CODE,CUST_GROUP_CODE,CUSTOMER_CODE,SUM([TOTAL QTY]) [TOTAL QTY],      

--SUM([LTR]) [LTR],SUM([AMOUNT]) [AMOUNT] FROM VW_SALEREGISTER SR      

--WHERE [INVOICE DATE]>=@FromDate and [INVOICE DATE]<=@ToDate    

--AND TRANTYPECODE IN (1,2) AND PSR_CODE=@targetsubobject  

--GROUP BY SR.[DIST. CODE],[INVOICE DATE],SR.PRODUCT_CODE,      

--PSR_CODE,CUSTOMER_CODE,CUST_GROUP_CODE        

  

----SELECT SR.[DIST. CODE],[INVOICE DATE],SR.PRODUCT_CODE,      

----PSR_CODE,CUST_GROUP_CODE,CUSTOMER_CODE,SUM([TOTAL QTY]),      

----SUM([LTR]),SUM([AMOUNT]) [AMOUNT] FROM VW_GETPSRTARGETACHIVEMENT SR      

----where [INVOICE DATE]>=@FromDate and [INVOICE DATE]<=@ToDate    

----AND PSR_CODE=@UserCode    

----GROUP BY SR.[DIST. CODE],[INVOICE DATE],SR.PRODUCT_CODE,      

----PSR_CODE,CUSTOMER_CODE,CUST_GROUP_CODE        

    

--END    

----select getdate()        

--ELSE    

BEGIN    

insert into #tmpsaleview    

SELECT SR.[DIST. CODE],[INVOICE DATE],SR.PRODUCT_CODE,      

PSR_CODE,CUST_GROUP_CODE,CUSTOMER_CODE,SUM([TOTAL QTY]) [TOTAL QTY],      

SUM([LTR]) [LTR],SUM([AMOUNT]) [AMOUNT] FROM VW_SALEREGISTER SR      

JOIN #TMPSITELIST SL ON SL.SITEID=SR.[DIST. CODE]      

AND [INVOICE DATE]>=@FromDate and [INVOICE DATE]<=@ToDate    

AND TRANTYPECODE IN (1,2)      

JOIN #TMPCHNGROUP CG ON CG.CHANNELGROUPCODE=SR.CUST_GROUP_CODE      

GROUP BY SR.[DIST. CODE],[INVOICE DATE],SR.PRODUCT_CODE,      

PSR_CODE,CUSTOMER_CODE,CUST_GROUP_CODE        

END    

    

    

SELECT CASE TL.TARGETTYPE WHEN 0 THEN 'SALE' WHEN 1 THEN 'PURCHASE' ELSE '' END TARGETTYPE,      

TH.TARGETCODE,TH.DESCRIPTION,BU.BU_CODE [BU CODE],BU.BU_DESC [BU NAME],TL.STARTDATE [From_Date],      

TL.ENDDATE [To_Date],TARGETCALCULATIONPATTERN,CALCULATIONON [Cal_On],      

ACXINCENTIVEAPPLICABILITY INCENTIVEBASE,TH.SITEID,tl.productgroup,      

tl.targetcategory,tc.NAME [Target_Cat] ,tl.targetsubcategory,      

TSG.NAME [Target_Sub_Cat],tl.objectcode,tl.targetobject,      

tl.targetsubobject,tl.itemtype,tl.TARGET,tl.incentive [Incentive],          

ISNULL((CASE TL.TARGETOBJECT   ---CALCULATION OF SALE ACHIVEMENT          

WHEN 1 ----ACHIVEMENT CALCULATION FOR SITE          

 THEN      

 CASE WHEN TL.ITEMTYPE=2 THEN       

  (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND [DIST. CODE]=TL.OBJECTCODE)      

 ELSE       

  (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  PRODUCT_CODE IN (SELECT * FROM DBO.ACX_UDF_SELECT_ITEM(TL.ITEMTYPE,TL.PRODUCTGROUP))  ---Selected Product Selection       

  AND [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND [DIST. CODE]=TL.OBJECTCODE)      

 END        

WHEN 0----ACHIVEMENT CALCULATION FOR PSR          

 THEN      

 CASE WHEN TL.ITEMTYPE=2 THEN       

 (CASE WHEN LEN(TL.TARGETSUBOBJECT)>0 THEN      

 (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND PSR_CODE=TL.TARGETSUBOBJECT)      

 --22      

 ELSE      

  (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID)      

  --33      

 END)      

 ELSE       

 (CASE WHEN LEN(TL.TARGETSUBOBJECT)>0 THEN      

 (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND    

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND PRODUCT_CODE IN (SELECT * FROM DBO.ACX_UDF_SELECT_ITEM(TL.ITEMTYPE,TL.PRODUCTGROUP))  ---Selected Product Selection          

  AND PSR_CODE=TL.TARGETSUBOBJECT)      

 --44      

 ELSE      

  (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  PRODUCT_CODE IN (SELECT * FROM DBO.ACX_UDF_SELECT_ITEM(TL.ITEMTYPE,TL.PRODUCTGROUP))  ---Selected Product Selection          

  AND [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID)      

 --55      

 END )      

 END      

WHEN 2 ----ACHIVEMENT CALCULATION FOR CUSTORMER GROUP          

        

 THEN       

 CASE WHEN TL.ITEMTYPE=2 THEN       

 (CASE WHEN LEN(TL.TARGETSUBOBJECT)>0 THEN      

 (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND CUSTOMER_CODE=TL.TARGETSUBOBJECT)      

 --22      

 ELSE      

  (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND CUSTOMER_CODE IN  (SELECT CUSTOMER_CODE       

  FROM AX.ACXCUSTMASTER CM  --Select Customer Group or Individual Customer Selection          

  WHERE CM.SITE_CODE=TH.SITEID AND CM.CUST_GROUP LIKE       

  CASE WHEN LEN(TL.OBJECTCODE)>0 THEN TL.OBJECTCODE ELSE '%' END)          

  )      

  --33      

 END)      

 ELSE       

 (CASE WHEN LEN(TL.TARGETSUBOBJECT)>0 THEN      

 (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND PRODUCT_CODE IN (SELECT * FROM DBO.ACX_UDF_SELECT_ITEM(TL.ITEMTYPE,TL.PRODUCTGROUP))  ---Selected Product Selection          

  AND CUSTOMER_CODE=TL.TARGETSUBOBJECT)      

 --44      

 ELSE      

  (SELECT CASE TL.CALCULATIONON WHEN 'BOX' THEN SUM([TOTAL QTY])       

  WHEN 'LTR' THEN SUM(LTR) WHEN 'VALUE' THEN SUM(AMOUNT) ELSE 0 END FROM #tmpsaleview SR      

  WHERE [INVOICE DATE]>=TL.STARTDATE AND       

  PRODUCT_CODE IN (SELECT * FROM DBO.ACX_UDF_SELECT_ITEM(TL.ITEMTYPE,TL.PRODUCTGROUP))  ---Selected Product Selection          

  AND [INVOICE DATE]<=TL.ENDDATE AND [DIST. CODE]=TH.SITEID      

  AND CUSTOMER_CODE IN  (SELECT CUSTOMER_CODE       

  FROM AX.ACXCUSTMASTER CM  --Select Customer Group or Individual Customer Selection          

  WHERE CM.SITE_CODE=TH.SITEID AND CM.CUST_GROUP LIKE       

  CASE WHEN LEN(TL.OBJECTCODE)>0 THEN TL.OBJECTCODE ELSE '%' END)          

  )      

 --55      

 END )      

 END      

END) ,0) [Achivement], '0' Actual_Incentive          

INTO #tmptarget          

FROM AX.acxtargetline TL join ax.acxtargetheader TH       

ON th.targetcode=tl.targetcode AND TH.SITEID=TL.SITEID          

JOIN AX.ACXTARGETSUBCATEGORY TSG       

ON TSG.SUBCATEGORY=TL.TARGETSUBCATEGORY           

JOIN AX.ACXTARGETCATEGORY TC on TC.CATEGORY=TL.TARGETCATEGORY          

JOIN #TMPSITELIST SL ON SL.SITEID=TH.SITEID      

JOIN [AX].[ACXBUMASTER] BU ON BU.BU_CODE = TL.BU_CODE      

WHERE TH.status=1          

AND TL.STARTDATE>=@FromDate and TL.ENDDATE<=@ToDate           

AND TL.OBJECTCODE LIKE CASE WHEN LEN(@ObjectCode)>0 THEN  @ObjectCode ELSE '%' END           

AND TL.targetsubobject LIKE CASE WHEN LEN(@targetsubobject)>0 THEN  @targetsubobject ELSE '%' END           

AND TL.TARGETCATEGORY LIKE CASE WHEN LEN(@TargetCategory)>0 THEN  @TargetCategory ELSE '%' END           

AND TL.TARGETSUBCATEGORY LIKE CASE WHEN LEN(@TargetSubCategory)>0 THEN  @TargetSubCategory ELSE '%' END           

and TL.TARGETTYPE IN (0,1) AND TL.BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)      

ORDER BY TL.TARGETTYPE, TH.TARGETCODE          

       

select TT.TARGETTYPE,TT.siteid,INV.NAME SITENAME,LS.NAME STATENAME,TT.[BU CODE],TT.[BU NAME],TARGETCODE,DESCRIPTION,          

CONVERT(NVARCHAR,[From_Date],106) [From_Date],CONVERT(NVARCHAR,[To_Date],106) [To_Date],        

Case targetobject when 0 then 'PSR' when 1 then 'Site' when 2 then 'Customer Group' end [Object],        

CASE targetcalculationpattern when 0 then 'Fix' when 1 then 'Multiple' else '' end [Pattern] ,[Cal_On],incentivebase,        

CASE when len(productgroup)='' then 'All' else productgroup end productgroup,targetcategory,[Target_Cat],        

targetsubcategory,[Target_Sub_Cat],objectcode,        

CASE WHEN OB.NAME IS NOT NULL THEN OB.NAME       

WHEN PM.PSR_Name IS NOT NULL THEN PM.PSR_Name      

WHEN CG.CUSTGROUP_NAME IS NOT NULL THEN CG.CUSTGROUP_NAME      

WHEN CGM.CUSTGROUP_NAME IS NOT NULL THEN CGM.CUSTGROUP_NAME      

ELSE CU.CUSTOMER_NAME END ObjectName,targetsubobject,          

ISNULL(CASE WHEN TSPM.PSR_Name IS NOT NULL THEN TSPM.PSR_NAME      

WHEN TSCGM.CUSTGROUP_NAME IS NOT NULL THEN TSCGM.CUSTGROUP_NAME       

ELSE TSCU.CUSTOMER_NAME END,'')  [Sub_Object_Name],          

itemtype,TARGET,incentive [Incentive], Achivement ,       
(cast([Achivement] as decimal(18,2))/cast([TARGET] as decimal(18,2)))*100 AS AchivementPer

FROM #tmptarget TT           
JOIN AX.INVENTSITE AS INV ON INV.SITEID=TT.SITEID          
LEFT OUTER JOIN AX.INVENTSITE AS OB ON OB.SITEID=TT.OBJECTCODE      
LEFT OUTER JOIN AX.ACXPSRMaster PM ON PM.PSR_Code=TT.OBJECTCODE      
LEFT OUTER JOIN ax.ACXCUSTGROUPMASTER CG ON CG.custgroup_code=TT.OBJECTCODE      
LEFT OUTER JOIN ax.ACXCUSTGROUPMASTER CGM ON CGM.custgroup_code=TT.targetsubobject      
LEFT OUTER JOIN ax.ACXCUSTMASTER CU ON CU.CUSTOMER_CODE=TT.OBJECTCODE      
LEFT OUTER JOIN AX.ACXPSRMaster TSPM ON TSPM.PSR_Code=TT.targetsubobject      
LEFT OUTER JOIN ax.ACXCUSTGROUPMASTER TSCGM ON TSCGM.custgroup_code=TT.targetsubobject      
LEFT OUTER JOIN ax.ACXCUSTMASTER TSCU ON TSCU.CUSTOMER_CODE=TT.targetsubobject      
JOIN AX.LOGISTICSADDRESSSTATE LS ON INV.STATECODE=LS.STATEID          
WHERE tt.TARGETCATEGORY like case when len(@TargetCategory)>0 then @TargetCategory else '%' end         
AND tt.TARGETSUBCATEGORY like case when len(@TargetSubCategory)>0 then @TargetSubCategory else '%' end          
order by SITEID,TARGETCODE,TARGETOBJECT,OBJECTCODE,TARGETSUBOBJECT,TARGETCATEGORY,TARGETSUBCATEGORY,PRODUCTGROUP,[TARGET]      
End 
END