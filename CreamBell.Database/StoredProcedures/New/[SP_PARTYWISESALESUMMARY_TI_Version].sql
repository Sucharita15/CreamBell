--[SP_PARTYWISESALESUMMARY] '0000600752','2019-05-01','2019-05-31','',''

--1.17, 30019





CREATE PROCEDURE [dbo].[SP_PARTYWISESALESUMMARY_TI_Version] 



(

@SiteId NVARCHAR(20),

@StartDate SMALLDATETIME,

@EndDate SMALLDATETIME,

@customergroupname nvarchar(50) = '',

@customername nvarchar(20) = '',

@BUCODE NVARCHAR(10)='')      



AS      



BEGIN      

	SELECT 

		[DIST. CODE] SITEID,

		[DIST. NAME],

		SR.BU_CODE [BU CODE], 

		BU_DESC [BU NAME],

		TRANTYPE,

		CUST_GROUP_CODE,

		[CUST GROUP],

		CUSTOMER_CODE,

		CUSTOMER_NAME,

		INVOICE_NO,

		[INVOICE DATE],

		BOX+[SCHEME BOX] BOX,

		PCS+[SCHEME PCS] PCS, 

		[TAXABLE AMT] TAXABLEAMT, 

		CONVERT(DECIMAL(18,6),

		CASE 

			WHEN 

				BOXPCS<>'0' 

			THEN 

				[BOXPCS] 

			ELSE 

				[SCHEME BOXPCS] 

		END) [TotalBoxPCS],

		[TOTAL QTY] TotalQtyConv,

		LTR,

		[LINE AMOUNT] [LINEAMOUNT],

		DISC_AMOUNT,

		[DISC2 AMT] SEC_DISC_AMOUNT,    

		[TD%] TD_Per,

		[PE%] [PE_Per],

		[TD AMT] tdvalue,

		[PE AMT],

		[SCHEME DISC AMT],

		[TAX1%] [TAX_CODE],

		[TAX1 AMT] TAX_AMOUNT,

		[TAX2%] ADDTAX_CODE,

		[TAX2 AMT] ADDTAX_AMOUNT,

		AMOUNT,  

		([TOTAL QTY]*MRP) as MRP_Value  

	FROM

		vw_SaleRegister SR 
		JOIN AX.ACXSITEBUMAPPING B ON
		SR.BU_CODE = B.BU_CODE 
		AND B.SITEID = @SiteId 
		AND ((@BUCODE  = '') OR (B.BU_CODE = @BUCODE))
		AND [DIST. CODE] = @SiteId 
		AND [INVOICE DATE] between @StartDate AND @EndDate 
		AND ((@customergroupname = '') OR (CUST_GROUP_CODE = @customergroupname)) 
		AND ((@customername = '') OR (CUSTOMER_CODE = @customername ))
END 








