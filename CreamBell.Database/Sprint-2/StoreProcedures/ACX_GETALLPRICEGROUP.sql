USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_GETALLPRICEGROUP]    Script Date: 10/31/2019 6:39:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[ACX_GETALLPRICEGROUP] (@SITEID NVARCHAR(Max)='')  
AS  
BEGIN  

IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@SITEID)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SITEID,',')   END   

select distinct A.Pricegroup ,B.Name as PriceGroupName   
from ax.acxcustmaster A   
left join [ax].[PRICEDISCGROUP] B on B.GroupId=A.Pricegroup   
where site_code IN (SELECT SITEID FROM #TMPSITELIST) and Pricegroup!='' and B.Name is not null--Order by PricegroupName  
UNION   
select I.PRICEGROUP,B.Name as PriceGroupName    From  [ax].[inventsite] I
join [ax].[PRICEDISCGROUP] B on B.GroupId=I.PRICEGROUP
WHERE siteid IN (SELECT SITEID FROM #TMPSITELIST)
--Select D1.ACX_PRICEGROUP AS Pricegroup ,B.Name as PriceGroupName   
--From [ax].logisticsaddressstate D1  
--left join [ax].[PRICEDISCGROUP] B on B.GroupId=D1.ACX_PRICEGROUP   
--Where D1.STATEID =
--(select statecode From  [ax].[inventsite] where siteid=@SITEID)  
Order by PricegroupName  
END
