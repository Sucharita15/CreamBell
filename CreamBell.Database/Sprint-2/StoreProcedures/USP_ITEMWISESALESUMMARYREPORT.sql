USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[USP_ITEMWISESALESUMMARYREPORT]    Script Date: 10/9/2019 6:44:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC USP_ITEMWISESALESUMMARYREPORT '000059144','1718-000561'
ALTER Procedure [dbo].[USP_ITEMWISESALESUMMARYREPORT] 
(                
@SiteID nvarchar(max),
@Invoice_No nvarchar(max)
)        
as
begin

 
IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@SITEID)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SITEID,',')   END     

---------------------------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#tmpINVOICENO') IS NOT NULL BEGIN DROP TABLE #tmpINVOICENO END 
CREATE TABLE #tmpINVOICENO (INVOICENO NVARCHAR(20))
IF LEN(@Invoice_No)>0
BEGIN	INSERT INTO #tmpINVOICENO SELECT ID FROM DBO.CommaDelimitedToTable(@Invoice_No,',')	END
---------------------------------------------------------------------------------------------------

Select SITEID, PRODUCT_PACKSIZE, PRODUCT_NAME, cast(PRODUCT_MRP as decimal(9, 2)) as PRODUCT_MRP, 
cast(Sum(Box) as decimal(9, 2)) as Box , cast(Sum(PCS) as decimal(9, 2)) as PCS,
cast(Sum([TotalBoxPCS]) as decimal(9,2)) as [TotalBoxPCS],Cast(Sum(TotalQtyConv) as Decimal(9, 2)) as TotalQtyConv,  
cast(Sum(AMOUNT) as decimal(9, 2)) as Amount, 
ROW_NUMBER() OVER(ORDER BY SITEID,Product_SubCategory ,PRODUCT_NAME, SITEID, PRODUCT_PACKSIZE, PRODUCT_MRP DESC) 
AS No from  
(SELECT PROD.PRODUCT_SUBCATEGORY,AIC.SITEID, AIC.TRANSTYPE, CAST(PROD.PRODUCT_PACKSIZE as decimal(9, 2)) as PRODUCT_PACKSIZE ,  
AIC.INVOICE_DATE, AIC.INVOICE_NO,  AIC.PRODUCT_NAME,AIC.PRODUCT_MRP, AIC.RATE,  isnull(BOXQty, '0') as Box,  
isnull(PCSQTY, '0') as PCS, isnull(BOXPCS, '0') as [TotalBoxPCS],  BOX as TotalQtyConv, AIC.LINEAMOUNT,  
AIC.AMOUNT FROM ACX_SALESUMMARY_PARTY_ITEM_WISE AIC  
left join ax.Inventtable PROD ON AIC.PRODUCT_CODE = PROD.ITEMID  
where SITEID IN (SELECT SITEID FROM #TMPSITELIST) and INVOICE_NO in (Select INVOICENO from #tmpINVOICENO) ) as a  
group by PRODUCT_SUBCATEGORY,PRODUCT_NAME, SITEID, PRODUCT_PACKSIZE, PRODUCT_NAME, PRODUCT_MRP 
--Order By Product_SubCategory ,PRODUCT_NAME, SITEID, PRODUCT_PACKSIZE, PRODUCT_MRP,    

end

--'SR-1718-000074','SR-1718-000073',
