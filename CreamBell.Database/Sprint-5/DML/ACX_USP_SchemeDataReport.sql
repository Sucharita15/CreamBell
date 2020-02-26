USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_SchemeDataReport]    Script Date: 1/4/2020 4:42:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [dbo].[ACX_USP_SchemeDataReport] '01-aug-2017','31-aug-2017','0','state','0000600018'  
  
ALTER procedure [dbo].[ACX_USP_SchemeDataReport]   
@FromDate smalldatetime,  
@ToDate smalldatetime,  
@SchemeCode Varchar(20),  
@SalesType Varchar(20),  
@UserCode nvarchar(max)  
  
AS  
begin  
IF OBJECT_ID('TEMPDB..#TMPSITE') IS NOT NULL BEGIN DROP TABLE #TMPSITE END              
CREATE TABLE #TMPSITE (SITEID NVARCHAR(20))            
IF LEN(@UserCode)>0          
BEGIN INSERT INTO #TMPSITE (SITEID) SELECT ID FROM DBO.CommaDelimitedToTable(@UserCode,',') END            

if object_id('tempdb..#tmpscheme') is not null begin drop table #tmpscheme end   
create table #tmpscheme(schemecode nvarchar(50))  
Declare @IsDistributor int,@statecode nvarchar(20)  
select @IsDistributor=Count(*) from ax.ACXSITEMENU where SITE_CODE IN (SELECT SITEID FROM #TMPSITE)   
SELECT TOP 1 @statecode=STATECODE FROM AX.INVENTSITE WHERE SITEID IN (SELECT SITEID FROM #TMPSITE)  
print @statecode
IF @IsDistributor=0  
BEGIN  
print @SalesType  
 IF @SalesType='0' --ALL  
  INSERT INTO #tmpscheme SELECT distinct SCHEMECODE FROM [dbo].[ACXAllSCHEMEVIEW] WHERE ((salescode in ('',@statecode)) or SALESCODE IN (SELECT SITEID FROM #TMPSITE))  
   AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
 IF @SalesType='state' --STATE  
  INSERT INTO #tmpscheme SELECT distinct SCHEMECODE FROM [dbo].[ACXAllSCHEMEVIEW] WHERE salescode in (@statecode)  
   AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
 IF @SalesType='site' --SITE  
  INSERT INTO #tmpscheme SELECT distinct SCHEMECODE FROM [dbo].[ACXAllSCHEMEVIEW] WHERE salescode IN (SELECT SITEID FROM #TMPSITE) 
   AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
END  
ELSE   
BEGIN  
 IF @SalesType='0' --ALL  
  INSERT INTO #tmpscheme SELECT distinct SCHEMECODE FROM [dbo].[ACXAllSCHEMEVIEW] WHERE [sales type] in ('ALL','Site','State')  
   AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
 IF @SalesType='state' --STATE  
  INSERT INTO #tmpscheme SELECT distinct SCHEMECODE FROM [dbo].[ACXAllSCHEMEVIEW] WHERE [sales type]='State'  
   AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
 IF @SalesType='site' --SITE  
  INSERT INTO #tmpscheme SELECT distinct SCHEMECODE FROM [dbo].[ACXAllSCHEMEVIEW] WHERE [sales type]='Site'  
   AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
END  
  
Select SCHEMECODE,[Scheme Description],[Scheme Type],[STARTINGDATE],[ENDINGDATE],[Sales Type],[SALESCODE],  
[SALESDESCRIPTION],[Type],[Code],[Scheme Item Type],[Scheme Item group],  
[Item Group Name],[MINIMUMQUANTITY],[MINIMUMVALUE],[free item code],[free Item Name],[FREEQTY] from [dbo].[ACXAllSCHEMEVIEW]  
where [STARTINGDATE] >= @FromDate and [STARTINGDATE] <= @ToDate   
AND SCHEMECODE IN (SELECT SCHEMECODE FROM #tmpscheme)  
--AND SCHEMECODE LIKE CASE ISNULL(@SchemeCode,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SchemeCode END  
--AND [Sales Type] like CASE ISNULL(@SalesType,'0') WHEN '0' THEN '%' WHEN '' THEN '%' ELSE @SalesType END    
end  