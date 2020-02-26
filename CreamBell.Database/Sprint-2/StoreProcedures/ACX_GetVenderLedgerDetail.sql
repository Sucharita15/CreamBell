USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_GetVenderLedgerDetail]    Script Date: 10/31/2019 7:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [dbo].[ACX_GetVenderLedgerDetail] '20-Sep-2017','16-Sep-2018', '0000600755','7200'
ALTER procedure [dbo].[ACX_GetVenderLedgerDetail] --'20-Sep-2017','16-Sep-2018', '0000600755','7200'
(
	@FromDate datetime,
	@ToDate datetime, 
	@SiteCode nvarchar(max),
	@Plant_Code nvarchar(255)
)
as
begin

IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@SiteCode)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SiteCode,',')   END 

IF OBJECT_ID('TEMPDB..#TMPPlant') IS NOT NULL BEGIN DROP TABLE #TMPPlant END
CREATE TABLE #TMPPlant (PlantCode NVARCHAR(50))

IF LEN(@Plant_Code)>0
BEGIN	INSERT INTO #TMPPlant SELECT id from [dbo].[CommaDelimitedToTable](@Plant_Code,',') END
ELSE
BEGIN	INSERT INTO #TMPPlant SELECT CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTCODE] ELSE A.[ACXPLANTNAME] END [ACXPLANTCODE] from ax.inventsite A Where A.SITEID IN (SELECT SITEID FROM #TMPSITELIST)union select SITEID from AX.INVENTSITE A 
Where A.SITEID IN (SELECT DISTINCT OTHER_SITE FROM AX.ACX_SDLinking WHERE SubDistributor IN (SELECT SITEID FROM #TMPSITELIST) AND BLOCKED=0)	END

declare @OpeningBalance numeric(18,2)                          
declare @ClosingValue numeric(18,2)   

SELECT @OpeningBalance=  dbo.[Fn_GetOpeningBalanceForVenderLedger](@FromDate,@SiteCode,@Plant_Code) 
SELECT @ClosingValue=  dbo.Fn_GetClosingBalanceForVenderLedger(@ToDate,@SiteCode,@Plant_Code)    

select Invoice_No,SiteCode,Dealer_Code,DealerName,Cast(Debit as decimal(18,2)) as Debit,Cast(Credit as decimal(18,2)) as Credit, DocumentType,RefNo_DocumentNo,CONVERT(nvarchar(20), Document_Date, 106)Document_Date,Remark,@OpeningBalance as OpeningBalance,



@ClosingValue as ClosingValue   from 
--select * from 
(
--declare @FromDate datetime set @FromDate='01-Jul-2016'
--declare @ToDate datetime set @ToDate='30-jul-2016'
--declare @SiteCode nvarchar(255) set @SiteCode='00008'
--declare @Plant_Code nvarchar(255) set @Plant_Code='000059144'

Select [ax].ACXVENDORTRANS.Document_No as Invoice_No, [ax].[ACXVENDORTRANS].SiteCode, [ax].[ACXVENDORTRANS].Dealer_Code,
DealerName=isnull((Select top 1 name from ax.inventsite where Siteid=@Plant_Code ),(select top 1 ACXPLANTNAME from AX.INVENTSITE where ACXPLANTCODE=@Plant_Code)),
case when [ax].[ACXVENDORTRANS].Amount>0 then [ax].[ACXVENDORTRANS].Amount else 0 end as Debit,--Credit, 
case when [ax].[ACXVENDORTRANS].Amount>0 then 0 else ABS([ax].[ACXVENDORTRANS].Amount) end as Credit,--Debit, 
(case when [ax].[ACXVENDORTRANS].DocumentType=2 then 'Purchase Return' else 'Purchase Receive' end) as DocumentType, 
[ax].[ACXVENDORTRANS].RefNo_DocumentNo,[ax].[ACXVENDORTRANS].Document_Date, '' as Remark,CREATEDDATETIME  
From [ax].[ACXVENDORTRANS]
where 
cast([ax].[ACXVENDORTRANS].Document_Date as date)>=@FromDate and cast([ax].[ACXVENDORTRANS].Document_Date as date)<=@ToDate
and  [ax].[ACXVENDORTRANS].SiteCode IN (SELECT SITEID FROM #TMPSITELIST) and [ax].[ACXVENDORTRANS].Dealer_Code IN (SELECT PLANTCODE FROM #TMPPlant)

Union 
 
Select  
[ax].[ACXPAYMENTENTRY].Document_No as Invoice_No,[ax].[ACXPAYMENTENTRY].SiteCode,[ax].[ACXPAYMENTENTRY].Plant_Code as Dealer_Code,
DealerName=isnull((Select top 1 name from ax.inventsite where Siteid=@Plant_Code ),(select top 1 ACXPLANTNAME from AX.INVENTSITE where ACXPLANTCODE=@Plant_Code)), 
0 as Debit,[ax].[ACXPAYMENTENTRY].Payment_Amount as Credit,'Payment' DocumentType, 
[ax].[ACXPAYMENTENTRY].Ref_DocumentNo,[ax].[ACXPAYMENTENTRY].Payment_Date as Document_Date,[ax].[ACXPAYMENTENTRY].Remark,CREATEDDATETIME 
From [ax].[ACXPAYMENTENTRY] 
where 
[ax].[ACXPAYMENTENTRY].Payment_Date>=@FromDate and [ax].[ACXPAYMENTENTRY].Payment_Date<=@ToDate
and  [ax].[ACXPAYMENTENTRY].SiteCode IN (SELECT SITEID FROM #TMPSITELIST) and [ax].[ACXPAYMENTENTRY].Plant_Code IN (SELECT PLANTCODE FROM #TMPPlant)

Union 
 
SELECT ADJH.Document_No As Invoice_No,MAX(ADJH.Site_Code) As SiteCode,@Plant_Code As Dealer_Code,
DealerName=isnull((Select top 1 name from ax.inventsite where Siteid=@Plant_Code ),
(select top 1 ACXPLANTNAME from AX.INVENTSITE where ACXPLANTCODE=@Plant_Code)), 
CASE WHEN ISNULL(SUM(ADHLI.Adjvalue),0) >0 THEN SUM(ADHLI.Adjvalue) ELSE 0 END AS Debit,
CASE WHEN ISNULL(SUM(ADHLI.Adjvalue),0) <0 THEN ABS(SUM(ADHLI.Adjvalue)) ELSE 0 END AS Credit,
--SUM(CASE WHEN ADHLI.Adjvalue > 0 THEN CAST(ADHLI.Adjvalue AS decimal(18,4)) ELSE 0 END) AS Debit,
--SUM(CASE WHEN ADHLI.Adjvalue < 0 THEN CAST(ADHLI.Adjvalue AS decimal(18,4)) ELSE 0 END)*-1 AS Credit,
'Adjustment' As DocumentType, 
MAX(ADJH.PURCH_RECIEPTNO) AS Ref_DocumentNo,MAX(ADJH.Document_Date) AS Document_Date,'' As Remark,
MAX(ADJH.CREATEDDATETIME) CREATEDDATETIME
FROM [ax].ACXADJUSTMENTENTRYHEADER ADJH
INNER JOIN [ax].[ACXADJUSTMENTENTRYLINE] ADHLI ON ADHLI.DOCUMENT_NO = ADJH.DOCUMENT_NO
WHERE ADJH.DOCUMENT_DATE Between @FromDate AND @ToDate AND ADJH.Site_Code  IN (SELECT SITEID FROM #TMPSITELIST)
GROUP BY ADJH.Document_No
) as t1 ORDER BY CREATEDDATETIME

END
