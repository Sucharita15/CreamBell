ALTER PROCEDURE [dbo].[SP_SALEREGISTER]



 (@SiteId NVARCHAR(20),



 @StartDate SMALLDATETIME,



 @EndDate SMALLDATETIME,



 @customergroupname nvarchar(50) = '',



 @customername nvarchar(20) = '',



 @productgroup nvarchar(20) = '',



 @subcategory nvarchar(30) = '',



 @productname nvarchar(100) = '',

 

 @BUCODE NVARCHAR(10)='')





 --SELECT * FROM vw_SaleRegister



AS    



BEGIN    



-------BUCODE-------  

IF OBJECT_ID('tempdb..#tmpBuCode') IS NOT NULL BEGIN DROP TABLE #tmpBuCode  END   

CREATE TABLE #tmpBuCode (BU_CODE NVARCHAR(10))  

IF LEN(@BUCODE)>0  

BEGIN INSERT INTO #tmpBuCode SELECT ID FROM DBO.CommaDelimitedToTable(@BUCODE,',') 

END  

ELSE

BEGIN INSERT INTO #tmpBuCode SELECT BU_CODE FROM AX.ACXSITEBUMAPPING WHERE SITEID IN ( @SiteId)

END



 select  [DIST. CODE] Siteid,[DIST. NAME] Dist_Name,CUST_GROUP_CODE,[CUST GROUP] Cust_Group,CUSTOMER_CODE,CUSTOMER_NAME,

 BU_CODE [BU CODE], BU_DESC [BU NAME], [INVOICE DATE] as  Invoice_Date,PRODUCT_GROUP,



PRODUCT_SUBCATEGORY,PRODUCT_CODE,PRODUCT_NAME,sum(LTR)as LTR,SUM(AMOUNT)AS AMOUNT



 FROM vw_SaleRegister  WHERE [DIST. CODE]IN (@SiteId) AND [INVOICE DATE]>=@StartDate AND [INVOICE DATE]<=@EndDate 



 and  CUST_GROUP_CODE like case when  @customergroupname = '' then '%' else @customergroupname end



	   and CUSTOMER_CODE like case when  @customername = '' then '%' else @customername end



     and Product_Group like case when  @productgroup = '' then '%' else @productgroup end



	   and Product_Subcategory like case when  @subcategory = '' then '%' else @subcategory end



	   and Product_Name like case when  @productname = '' then '%' else @productname end



	   AND BU_CODE IN (SELECT BU_CODE FROM #tmpBuCode)



	   Group by [DIST. CODE] ,[DIST. NAME],CUST_GROUP_CODE,[CUST GROUP],CUSTOMER_CODE,CUSTOMER_NAME,BU_CODE,BU_DESC,[INVOICE DATE],PRODUCT_GROUP,



       PRODUCT_SUBCATEGORY,PRODUCT_CODE,PRODUCT_NAME



	   end
