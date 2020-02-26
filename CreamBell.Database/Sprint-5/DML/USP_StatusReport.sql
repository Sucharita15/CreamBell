USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[USP_StatusReport]    Script Date: 1/19/2020 9:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
--[dbo].[USP_StatusReport] '2019-01-01','2019-06-30','0000600752','','',''          
ALTER procedure [dbo].[USP_StatusReport]       
@FDate as nvarchar(20)='',       
@TDate as nvarchar(20)='',      
@SiteID as nvarchar(MAX)='',       
@psrcode as nvarchar(MAX) ='',      
@beatcode as nvarchar(MAX) = null,      
@statusshp as nvarchar(MAX) = null      
as      
BEGIN      
SET NOCOUNT ON           
SET ARITHABORT ON                
SET CONCAT_NULL_YIELDS_NULL ON                
SET QUOTED_IDENTIFIER OFF                
SET ANSI_NULLS ON                
SET ANSI_PADDING ON                
SET ANSI_WARNINGS ON                
SET NUMERIC_ROUNDABORT OFF                
----------------------------------------------------------------------------------------      
IF OBJECT_ID('tempdb..#tmpSITECODE') IS NOT NULL BEGIN DROP TABLE #tmpSITECODE END       
CREATE TABLE #tmpSITECODE (SiteID NVARCHAR(20))      
IF LEN(@SiteID)>0      
BEGIN INSERT INTO #tmpSITECODE SELECT ID FROM DBO.CommaDelimitedToTable(@SiteID,',')      
END      
ELSE      
BEGIN INSERT INTO #tmpSITECODE SELECT DISTINCT Site_Code FROM AX.acxusermaster      
END      
----------------------------------------------------------------------------------------      
IF OBJECT_ID('tempdb..#tmpPSRCODE') IS NOT NULL BEGIN DROP TABLE #tmpPSRCODE END       
CREATE TABLE #tmppsrcode (PSRCODE NVARCHAR(20))      
IF LEN(@psrcode)>0      
BEGIN INSERT INTO #tmpPSRCODE SELECT ID FROM DBO.CommaDelimitedToTable(@psrcode,',')      
END      
ELSE      
BEGIN INSERT INTO #tmpPSRCODE SELECT DISTINCT PSR_Code FROM AX.acxpsrmaster      
END      
-------------------------------------------------------------------------------------------      
IF OBJECT_ID('tempdb..#tmpBEATCODE') IS NOT NULL BEGIN DROP TABLE #tmpBEATCODE END       
CREATE TABLE #tmpBEATCODE (BEATCODE NVARCHAR(20))      
IF LEN(@beatcode)>0      
BEGIN INSERT INTO #tmpBEATCODE SELECT ID FROM DBO.CommaDelimitedToTable(@beatcode,',')      
END      
ELSE      
BEGIN INSERT INTO #tmpBEATCODE SELECT DISTINCT BeatCode FROM AX.acxpsrbeatmaster      
END      
--------------------------------------------------------------------------------------------      
IF OBJECT_ID('tempdb..#tmpstatusshp') IS NOT NULL BEGIN DROP TABLE #tmpstatusshp END       
CREATE TABLE #tmpstatusshp (BEATCODE NVARCHAR(20))      
IF LEN(@statusshp)>0      
BEGIN INSERT INTO #tmpstatusshp SELECT ID FROM DBO.CommaDelimitedToTable(@statusshp,',')      
END      
ELSE      
BEGIN INSERT INTO #tmpstatusshp SELECT DISTINCT StatusCode FROM AX.ACXShopStatusEntry      
END 
--IF OBJECT_ID('tempdb..#TMPSTATUSENTRY') IS NOT NULL BEGIN DROP TABLE #TMPSTATUSENTRY END       
--------------------------------------------------------------------------------------------      
SELECT  i.STATENAME as [State Name],"'"+SiteCode [Distributor Code],D.Name [Distributor Name],      
E.ROLEDESCRIPTION [Distributor Type],A.PSRCode [PSR Code],h.PSR_Name[PSR Name],      
g.BeatCode[Beat Code],g.BeatName[Beat Name],"'"+c.CUSTOMER_CODE[Customer Code],      
replace(C.CUSTOMER_NAME, '''', '') [Customer Name],C.CUST_GROUP[Customer Group],      
C.CHANNELGROUP[Channel Group],C.CHANNELTYPE[Channel Type],C.DEEP_FRIZER AS [Deep Freezer],      
cast(A.StartDateTime as time(0)) AS [Time In],      
cast(A.EndDateTime as time(0)) AS [Time Out],      
CONVERT(VARCHAR(12), DATEADD(SECOND, DATEDIFF(SECOND, A.StartDateTime, A.EndDateTime), 0), 108) AS [Time Spent],      
DATEDIFF(SECOND, A.StartDateTime, A.EndDateTime) AS [Time Spent(Seconds)],      
A.VISITFREQUENCY as [Frequency],CASE WHEN JUMPCALL=1 THEN 'No' WHEN JUMPCALL=0 THEN 'Yes' ELSE '' END AS [Call Status(Actual)],      
CASE WHEN JUMPCALL=1 THEN 'Yes' WHEN JUMPCALL=0 THEN 'No' ELSE '' END AS [Jump Call],      
cast(isnull((SH.SO_VALUE),0)as decimal(18,2)) as [Order Value],
--cast((Select isnull(sum(SO_VALUE),0) from ax.acxsalesheader WHERE StatusId = A.StatusId) as decimal(18,2)) as [Order Value],      
--(select count(*) from ax.acxsaleinvoiceline where INVOICE_NO IN (Select INVOICE_NO from ax.acxsalesheader WHERE StatusId = A.StatusId) and siteid in (select * from #tmpSITECODE)) as [TLSD],      
(select count(*) from ax.acxsaleinvoiceline SLL WHERE SLL.INVOICE_NO = ISNULL(SH.INVOICE_NO,'') AND SLL.SITEID=SH.SITEID  AND SLL.siteid in (select * from #tmpSITECODE)) as [TLSD],    
--STUFF((select ', ' + INVOICE_NO from ax.acxsalesheader WHERE StatusId = A.StatusId AND SITEID=A.SITECODE for xml path('')), 1, 2, '') as [Invoice No],      
ISNULL(SH.INVOICE_NO,'') as [Invoice No],
--STUFF((select ', ' + CONVERT(NVARCHAR(11), SIH.Invoic_date, 106) from ax.acxsaleinvoiceheader SIH JOIN ax.acxsalesheader SHH ON SIH.INVOICE_NO = SHH.INVOICE_NO     
--AND SIH.SITEID=A.SITECODE AND SIH.SITEID=SHH.SITEID     
--AND  SHH.StatusId = A.StatusId and SIH.siteid in (select * from #tmpSITECODE) for xml path(''))    
--, 1, 2, '') as [Invoice Date],      
STUFF((select ', ' + CONVERT(NVARCHAR(11), SIH.Invoic_date, 106) from ax.acxsaleinvoiceheader SIH WHERE SIH.INVOICE_NO = ISNULL(SH.INVOICE_NO,'')     
AND SIH.SITEID=A.SITECODE AND SIH.SITEID=SH.SITEID     
and SIH.siteid in (select * from #tmpSITECODE) for xml path(''))    
, 1, 2, '') as [Invoice Date],      
--cast((Select isnull(sum(SIH.INVOICE_VALUE),0) from ax.acxsaleinvoiceheader SIH JOIN ax.acxsalesheader SHH ON SIH.INVOICE_NO = SHH.INVOICE_NO AND  SHH.StatusId = A.StatusId and SIH.siteid = A.SITECODE) as decimal(18,2)) as [Invoice Value],      
cast((Select isnull(sum(SIH.INVOICE_VALUE),0) from ax.acxsaleinvoiceheader SIH WHERE SIH.INVOICE_NO = SH.INVOICE_NO AND  SH.StatusId = A.StatusId and SIH.siteid = A.SITECODE) as decimal(18,2)) as [Invoice Value],      
CONVERT(NVARCHAR(11), a.StatusDate, 106)[Capture ORD Date],FORMAT(CAST(a.StatusDate AS DATETIME),'hh:mm tt') [Capture ORD Time],      
CONVERT(NVARCHAR(11), SH.SO_APPROVEDATE, 106) [Order APRD Date],        
FORMAT(CAST(SH.SO_APPROVEDATE AS DATETIME),'hh:mm tt') [Order APRD Time],        
CONVERT(NVARCHAR(11),CASE WHEN A.StatusCode=5 THEN SH.CREATEDDATETIME ELSE A.CREATEDDATETIME END, 106) [Sync Date],        
FORMAT(CAST(CASE WHEN A.StatusCode=5 THEN SH.CREATEDDATETIME ELSE A.CREATEDDATETIME END AS DATETIME),'hh:mm tt') [Sync Time],      
B.ShopStatus_Name[Shop Status],ISNULL(A.IMEINO,'') [IMEI NO],ISNULL(A.MODELNO,'') [MODEL NO],ISNULL(A.VERSIONNO,'') [VERSION NO],    
ISNULL(A.LAT,'0') [LATITUDE],ISNULL(A.LONG,'0') [LONGITUDE]    
     
FROM AX.ACXShopStatusEntry A      
JOIN AX.ACXShopStatus B ON A.StatusCode = B.ShopStatus_Code   
JOIN AX.ACXCUSTMASTER C ON A.CustomerCode = C.CUSTOMER_CODE      
JOIN AX.InventSite D ON A.SiteCode = D.SiteID      
join ax.acxusermaster E on A.SiteCode=E.Site_Code      
LEFT OUTER JOIN ax.acxpsrbeatmaster g on A.PSRCode =g.PSRCode and C.PSR_BEAT = g.BeatCode      
join ax.acxpsrmaster h on A.PSRCODE=H.PSR_Code --g.PSRCode =h.PSR_Code      
join AX.LOGISTICSADDRESSSTATE i on D.STATECODE=i.STATEID --and C.PSR_BEAT = g.BeatCode      
LEFT OUTER JOIN AX.ACXSALESHEADER SH ON SH.StatusId=A.StatusId  
WHERE cast(A.StatusDate as date) >= @FDate AND cast(A.StatusDate as date) <= @TDate --and a.statusid='P0022502212019122145'      
AND E.Site_Code in (select * from #tmpSITECODE)       
and h.PSR_CODE in (select * from #tmpPSRCODE)       
--and A.PSRCODE in (select * from #tmpPSRCODE)       
and C.PSR_BEAT in (select * from #tmpBEATCODE)      
and a.StatusCode in (select * from #tmpstatusshp)  
AND NOT(B.ShopStatus_Name='Working' and SH.SO_APPROVEDATE is  null   )    
ORDER BY StatusDate DESC, C.CUSTOMER_CODE     
 
--DELETE FROM #TMPSTATUSENTRY WHERE [Shop Status]='Working' and [Order APRD Date] is null        
--SELECT * FROM #TMPSTATUSENTRY      
END     
