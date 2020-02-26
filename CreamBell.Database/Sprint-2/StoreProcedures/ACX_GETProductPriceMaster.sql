USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_GETProductPriceMaster]    Script Date: 10/31/2019 6:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
--exec [dbo].[ACX_GETProductPriceMaster] '','0000601426','PC-DL'    
ALTER Procedure [dbo].[ACX_GETProductPriceMaster] (@State nvarchar(10)='',@SiteId nvarchar(max)='',@PriceGroup nvarchar(10)='')    
As    
Begin    
----------------------------------------------------------------------    
IF OBJECT_ID('TEMPDB..#TMPPRICEGROUP') IS NOT NULL BEGIN DROP TABLE #TMPPRICEGROUP END    
CREATE TABLE #TMPPRICEGROUP (PRICEGROUP NVARCHAR(20),PRICEGROUPNAME NVARCHAR(50))    
    
IF @PriceGroup='All...'    
BEGIN INSERT INTO #TMPPRICEGROUP EXEC ACX_GETALLPRICEGROUP @SiteId END    
ELSE    
BEGIN INSERT INTO #TMPPRICEGROUP select groupid,name from [ax].[PRICEDISCGROUP] where groupid=@PriceGroup END    


IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@SITEID)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SITEID,',')   END
    
----------------------------------------------------------------------    
Select *,TaxAmount=Basic*Tax/100,SellingPrice=Basic+Basic*Tax/100,Short='A'+PRICEGROUP from     
(select inv.Product_Subcategory,inv.Itemid,inv.Product_name,inv.Product_mrp ,Prg.PRICEGROUP,Prg.PRICEGROUPNAME as Price_Group    
 --,PriceGroup=Prg.PRICEGROUP--@PriceGroup    
 --,Price_Group = (select top 1 B.Name as PriceGroupName from [ax].[PRICEDISCGROUP] B Where B.GroupId in (select PRICEGROUP from #TMPPRICEGROUP))--@PriceGroup)      
 ,Basic =isnull((Select top 1 Amount from [DBO].[ACX_UDF_GETPRICE](GETDATE(),Prg.PRICEGROUP,inv.itemid)),0)     
 ,Tax=(SELECT ISNULL(SUM(TAXPER),0) FROM VW_GSTTAXSETUP WHERE TAXTYPE=1 AND TAXSERIALNO IN (1,2) AND HSNCODE =INV.HSNCODE)  
 --,Tax=isnull(  
 --(Select top 1 case when H.ACXADDITIONALTAXBASIS=1 then H.TaxValue+H.ACXADDITIONALTAXVALUE else H.TaxValue+H.TaxValue*H.ACXADDITIONALTAXVALUE/100 end      
 --from [ax].inventsite G      
 --inner join InventTax H on G.TaxGroup=H.TaxGroup     
 --where H.ItemId=inv.itemid and G.Siteid=@SiteId),0)    
 from ax.inventtable inv    
 Left join #TMPPRICEGROUP Prg on 1=1    
 ) A    
    
 UNION ALL    
 ---------------Data For Special Price    
    
 SELECT *,TaxAmount=Basic*Tax/100,SellingPrice=Basic+Basic*(Tax/100),Short='Z'+PRICEGROUP FROM     
    (    
 SELECT DISTINCT INV.Product_Subcategory,INV.Itemid,INV.Product_name,INV.Product_mrp,PRI.ACCOUNTRELATION AS PriceGroup,CUSTOMER_NAME AS Price_Group    
 ,Basic =isnull((Select top 1 Amount from [DBO].[ACX_UDF_GETPRICE](GETDATE(),PRI.ACCOUNTRELATION,PRI.ITEMRELATION)),0)     
 ,Tax=(SELECT ISNULL(SUM(TAXPER),0) FROM VW_GSTTAXSETUP WHERE TAXTYPE=1 AND TAXSERIALNO IN (1,2) AND HSNCODE =INV.HSNCODE)  
 --,Tax=isnull((Select top 1 case when H.ACXADDITIONALTAXBASIS=1 then H.TaxValue+H.ACXADDITIONALTAXVALUE else H.TaxValue+H.TaxValue*H.ACXADDITIONALTAXVALUE/100 end      
 --from [ax].inventsite G     
 --inner join InventTax H on G.TaxGroup=H.TaxGroup     
 --where H.ItemId=inv.itemid and G.Siteid=@SiteId),0)    
 FROM ax.PRICEDISCTABLE PRI    
 LEFT JOIN ax.acxcustmaster CUST ON PRI.ACCOUNTRELATION=CUST.Customer_Code    
 LEFT JOIN ax.inventtable INV ON INV.ITEMID=PRI.ITEMRELATION    
 WHERE PRI.ACCOUNTRELATION IN (select Customer_Code From ax.acxcustmaster Where Site_Code  IN (SELECT SITEID FROM #TMPSITELIST) AND PRICEGROUP in (select PRICEGROUP from #TMPPRICEGROUP))    
 AND INV.Itemid IS NOT NULL    
 ) A       
 Order By Short,Price_Group,A.itemid    
 end    