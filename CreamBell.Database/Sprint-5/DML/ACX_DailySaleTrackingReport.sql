USE [7200]
GO
/****** Object:  StoredProcedure [ax].[ACX_DailySaleTrackingReport]    Script Date: 12/22/2019 11:40:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [ax].[ACX_DailySaleTrackingReport]   ( @FromDate datetime, @ToDate datetime, @SiteId nvarchar(max),  
@PSR_Code nvarchar(max), @STATECODE nvarchar(max), @UserCode nvarchar(50)='', @UserType nvarchar(50)=''  )  

as  
begin  

declare @LastYearMonthFromdate datetime  
declare @LastYearMonthTodatedate datetime  
declare @LastYearMonthLastdate datetime  
declare @TotalMonthDay int  
declare @TotalMonthBalanceDay int  
DECLARE @ADate DATETIME  
declare @TotalDay int  

SET @LastYearMonthFromdate=dateadd(yy,-1,@FromDate)  
SET @LastYearMonthTodatedate=dateadd(yy,-1,@ToDate)  
SET @TotalMonthDay= DATEPART(day,@ToDate) ------current day  
SET @TotalDay= DAY(EOMONTH(@ToDate))  

if(@TotalDay=0)  
begin  
SET @TotalDay=1  
end  

SET @TotalMonthBalanceDay=(@TotalDay-@TotalMonthDay)----  

if(@TotalMonthBalanceDay=0)  
begin  
SET @TotalMonthBalanceDay=1  
end  

SET @LastYearMonthLastdate = DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@LastYearMonthTodatedate)+1,0))  
print @LastYearMonthLastdate  
print @TotalMonthBalanceDay  
print @TotalDay  
print @LastYearMonthFromdate  
print @LastYearMonthTodatedate  

IF OBJECT_ID('tempdb..#tmpState') IS NOT NULL BEGIN DROP TABLE #tmpState END       
CREATE TABLE #tmpState (STATECODE NVARCHAR(max))      
IF LEN(@STATECODE)>0      
	BEGIN INSERT INTO #tmpState SELECT ID FROM DBO.CommaDelimitedToTable(@STATECODE,',') END      
ELSE      
BEGIN INSERT INTO #tmpState SELECT DISTINCT STATEID FROM AX.LOGISTICSADDRESSSTATE WHERE LEN(STATEID)>0 END      

IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #tmpsite END          
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
BEGIN   INSERT INTO #TMPCHNGROUP SELECT DISTINCT CUST_GROUP FROM AX.ACXCUSTMASTER  END        
ELSE        
BEGIN  INSERT INTO #TMPCHNGROUP SELECT CHANNELGROUPCODE FROM DBO.UDF_GETCHANNELGROUPBYUSER(@UserCode)  END        

------------PSR------------  
IF OBJECT_ID('tempdb..#tmpPSR') IS NOT NULL BEGIN DROP TABLE #tmpPSR END   
CREATE TABLE #tmpPSR (PSR_Code NVARCHAR(20))  
IF LEN(@PSR_Code)>0  
BEGIN	INSERT INTO #tmpPSR SELECT ID FROM DBO.CommaDelimitedToTable(@PSR_Code,',')		END  
ELSE  
BEGIN	INSERT INTO #tmpPSR SELECT DISTINCT PSRCode  FROM ax.ACXPSRSITELinkingMaster WHERE LEN(PSRCode)>0 AND Site_Code in (select SITEID from #TMPSITELIST)  
END  
----------------------------------  

;with cte as   
(----GET Distributor and PSR Detail  
SELECT DISTINCT ax.INVENTSITE.SITEID, ax.INVENTSITE.NAME, ax.ACXPSRSITELinkingMaster.PSRCode, ax.ACXPSRMaster.PSR_Name  
FROM  ax.INVENTSITE   
INNER JOIN ax.ACXPSRSITELinkingMaster ON ax.INVENTSITE.SITEID = ax.ACXPSRSITELinkingMaster.Site_Code   
INNER JOIN ax.ACXPSRMaster ON ax.ACXPSRSITELinkingMaster.PSRCode = ax.ACXPSRMaster.PSR_Code  
WHERE ax.INVENTSITE.SITEID in (select SITEID from #TMPSITELIST)  
AND ax.ACXPSRMaster.PSR_Code in ( select PSR_Code from #tmpPSR)  
AND AX.ACXPSRMASTER.PSR_CODE IN (SELECT DISTINCT PSR_CODE FROM AX.ACXCUSTMASTER WHERE CUST_GROUP IN (SELECT CHANNELGROUPCODE FROM #TMPCHNGROUP))
),   cte2 as (  


------GET YagoFullMonthSale------  
SELECT RST.SITEID,RST.PSR_CODE,sum(coalesce(SL.LTR,0)) as YagoFullMonthSale  
FROM (SELECT DISTINCT [DIST. CODE] SITEID,PSR_CODE,INVOICE_NO  FROM VW_SALEREGISTER
WHERE [INVOICE DATE] >=@LastYearMonthFromdate ----Month First Date--Last Year  
AND [INVOICE DATE] <=@LastYearMonthLastdate---Month Last Date--Last Year  
AND PSR_CODE in (SELECT PSR_Code FROM #tmpPSR)  
AND [DIST. CODE] in (SELECT SITEID FROM #TMPSITELIST)  
AND CUST_GROUP_CODE IN (SELECT CHANNELGROUPCODE FROM #TMPCHNGROUP)
) RST INNER JOIN VW_SALEREGISTER SL ON SL.[DIST. CODE]=RST.SITEID AND SL.INVOICE_NO=RST.INVOICE_NO  
GROUP BY RST.SITEID,RST.PSR_CODE  ),  cte3 as  
 
(--------MTD Sale-, Current Rate=MTD Sale/@TotalDay ---  
SELECT RST.SITEID,RST.PSR_CODE, sum(coalesce(SL.LTR,0)) as MTDSale  
FROM (SELECT DISTINCT [DIST. CODE] SITEID,PSR_CODE,INVOICE_NO  FROM VW_SALEREGISTER
WHERE [INVOICE DATE]>=@FromDate----Current Month First Date  
AND [INVOICE DATE]<=@ToDate----Current Month To Date  
AND PSR_CODE in (SELECT PSR_Code FROM #tmpPSR)  
AND [DIST. CODE] in (SELECT SITEID FROM #TMPSITELIST)  
AND CUST_GROUP_CODE IN (SELECT CHANNELGROUPCODE FROM #TMPCHNGROUP)
) RST INNER JOIN VW_SALEREGISTER SL ON SL.[DIST. CODE]=RST.SITEID AND SL.INVOICE_NO=RST.INVOICE_NO
 GROUP BY RST.SITEID,RST.PSR_CODE),cte4 as

 (------YagoMTD------------
SELECT RST.SITEID,RST.PSR_CODE,SUM(coalesce(SL.LTR,0))  as YagoMTD
FROM (SELECT DISTINCT [DIST. CODE] SITEID,PSR_CODE,INVOICE_NO  FROM VW_SALEREGISTER
WHERE [INVOICE DATE]>=@LastYearMonthFromdate ----Month First Date--Last Year
AND [INVOICE DATE]<=@LastYearMonthTodatedate---Month To Date--Last Year
AND PSR_CODE in (SELECT PSR_Code FROM #tmpPSR)
AND [DIST. CODE] in (SELECT SITEID FROM #TMPSITELIST)
AND CUST_GROUP_CODE IN (SELECT CHANNELGROUPCODE FROM #TMPCHNGROUP)
) RST INNER JOIN VW_SALEREGISTER SL ON SL.[DIST. CODE]=RST.SITEID AND SL.INVOICE_NO=RST.INVOICE_NO
GROUP BY RST.SITEID,RST.PSR_CODE ),  cte5 as  
(----  Target-----  
SELECT INS.SITEID,INS.NAME,PSR.PSR_CODE,PSR.PSR_NAME,  
(SELECT ISNULL(SUM(TARGET),0) FROM AX.acxtargetline TL WHERE TL.SITEID=INS.SITEID   
AND TARGETSUBOBJECT=PSR.PSR_CODE AND BASETARGET=1 AND CALCULATIONON='LTR'
AND TL.STARTDATE >=@LastYearMonthFromdate ----Month First Date--Last Year  
AND TL.ENDDATE <=@LastYearMonthLastdate---Month Last Date--Last Year  
AND INS.SITEID in (select SITEID from #TMPSITELIST)  
AND PSR.PSR_CODE in ( select PSRCode from #tmpPSR)  
) as FullMonthTarget  FROM ax.ACXPSRMaster PSR   
INNER JOIN ax.ACXPSRSITELinkingMaster PLM ON PLM.PSRCode = PSR.PSR_Code   
INNER JOIN ax.INVENTSITE INS ON INS.SITEID = PLM.Site_Code)  

-------------------------SELECT ALL DETAIL  
SELECT cte.SITEID, cte.NAME, cte.PSRCode, cte.PSR_Name, coalesce(cte2.YagoFullMonthSale,0) as YagoFullMonthSale,  
coalesce(cte3.MTDSale,0) as MTDSale, (coalesce(cte3.MTDSale,0)/@TotalDay) as CurrentRate,  
((coalesce(cte2.YagoFullMonthSale,0)-coalesce(cte3.MTDSale,0))/@TotalMonthBalanceDay) as RRYago,  
((coalesce(cte2.YagoFullMonthSale,0)-coalesce(cte3.MTDSale,0))) as BalancetoYago,   
coalesce(cte4.YagoMTD,0) as YagoMTD, coalesce(cte5.FullMonthTarget,0) as FullMonthTarget,  
(coalesce(cte5.FullMonthTarget,0)-(coalesce(cte3.MTDSale,0)/@TotalDay)) as RRTarget, 
(coalesce(cte3.MTDSale,0)/ (case when coalesce(cte5.FullMonthTarget,0)=0 then 1 else coalesce(cte5.FullMonthTarget,0) end)) AchPecTarget ,  
((coalesce(cte3.MTDSale,0)/ (case when coalesce(cte4.YagoMTD,0)=0 then 1 else coalesce(cte4.YagoMTD,0) end))) as Growth,  
(coalesce(cte5.FullMonthTarget,0)-coalesce(cte3.MTDSale,0)) as BalancetoTarget  
FROM cte  
LEFT JOIN cte2 on cte.SITEID = cte2.SITEID AND cte.PSRCode = cte2.PSR_CODE
LEFT JOIN cte3 ON cte.SITEID = cte3.SITEID AND cte.PSRCode = cte3.PSR_CODE
LEFT JOIN cte4 ON cte.SITEID = cte4.SITEID AND cte.PSRCode = cte4.PSR_CODE
LEFT JOIN cte5 ON cte.SITEID = cte5.SITEID AND cte.PSRCode = cte5.PSR_CODE
END







