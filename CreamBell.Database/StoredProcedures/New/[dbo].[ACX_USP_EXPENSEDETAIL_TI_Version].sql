USE [7200]
GO
/****** Object:  StoredProcedure [dbo].[ACX_USP_EXPENSEDETAIL]    Script Date: 01-09-2019 16:53:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
--[dbo].[ACX_USP_EXPENSEDETAIL] '01-dec-2018','31-dec-2018','','0000600777','','','BU1'    
CREATE PROCEDURE [dbo].[ACX_USP_EXPENSEDETAIL_TI_Version](@FROMDATE SMALLDATETIME,@TODATE SMALLDATETIME,@Claim_Type NVARCHAR(20),    
@SITEID NVARCHAR(20),@Claim_Category NVARCHAR(40),@Claim_SubCategory NVARCHAR(40),@BUCode NVARCHAR(20))    
AS    
BEGIN    
		Select 
			I.StateCode as State,
			I.Siteid as DistributorCode,
			I.Name DistributorName,    
			Case 
				when Claim_Type = '0' 
				then 'Sale' 
				else 'Purchase' 
			end as ClaimType,    
			Document_Code as ClaimDocNo,
			DOCUMENT_DATE as ClaimDocDate,    
			from_Date,
			To_Date,    
			Case
				WHEN ISNULL(OBJECT,'') ='' 
				THEN '' 
				when OBJECT =0 
				then 'PSR' 
				when OBJECT =1 
				then 'SITE'  
				when OBJECT =2 
				then 'Customer Group' 
			end as Object,    
			Object_Code,  
			Object_Name1 = ISNULL(
			(Select 
				Case   
				when isnull(OBJECT,'') = '' 
				then (
					SELECT TOP 1 [NAME] FROM 
						(	select 
								Top 1 PSR_Name [NAME] 
							from 
								[ax].[ACXPSRMaster] P
							where 
								PSR_Code = Object_Code  
						UNION  
							select 
								Top 1 [NAME] 
							from 
								[ax].[INVENTSITE] 
							where 
								SiteID = Object_Code  

						UNION   
							select 
								Top 1 CUSTGROUP_NAME [NAME] 
							from 
								[ax].[ACXCUSTGROUPMASTER] 
							where 
								CustGroup_Code = Object_Code  
						union 
							Select 
								Top 1 CUSTOMER_NAME 
							from 
								AX.ACXCUSTMASTER 
							where 
								Customer_Code=Object_Code)  AS AA)  
				when [OBJECT] = 0 
				then (
					select 
						Top 1 PSR_Name 
					from 
						[ax].[ACXPSRMaster] P 
					where 
						PSR_Code = Object_Code)     
				when [OBJECT] = 1 
				then  (
					select 
						Top 1 Name 
					from 
						[ax].[INVENTSITE] 
					where 
						SiteID = Object_Code)    
				when [OBJECT] = 2 
				then (
					select 
						Top 1 CUSTGROUP_NAME 
					from 
						[ax].[ACXCUSTGROUPMASTER] 
					where 
						CustGroup_Code = Object_Code )end ),''),    

			Object_SubCode,
			Target_GROUP,
			Object_SubName = ISNULL((
			Select 
				Case   
				when OBJECT = 2 
				then (
					Select 
						Top 1 CUSTOMER_NAME 
					from 
						AX.ACXCUSTMASTER 
					where 
						Customer_Code= Object_SubCode)  
				else (
					SELECT 
						TOP 1 [NAME] 
					FROM (
						select 
							Top 1 PSR_Name [NAME] 
						from 
							[ax].[ACXPSRMaster] P 
						where 
							PSR_Code = Object_SubCode  
					UNION 
					select
						Top 1 [NAME] 
					from 
						[ax].[INVENTSITE] 
					where 
						SiteID = Object_SubCode  
					UNION  
					select 
						Top 1 CUSTGROUP_NAME [NAME] 
					from 
						[ax].[ACXCUSTGROUPMASTER] 
					where 
						CustGroup_Code = Object_SubCode  
					union 
					Select 
						Top 1 CUSTOMER_NAME 
					from 
						AX.ACXCUSTMASTER 
					where 
						Customer_Code=Object_SubCode)  AS AA) end),''),
				CM.BU_CODE,    
				Target_Code,
				Target_Description,
				CM.Target,
				ACHIVEMENT,
				ACTUAL_INCENTIVE,
				TARGET_CODE,
				tl.CAlCULATIONON,
				Target_INCENTIVE,
				TC.Name as Cat,
				TS.Name as Subcat,
				CM.CALCULATION_PATTERN 
		from 
		(Select 
			*
		from
			[ax].[ACXCLAIMMASTER] 
		WHERE 
				CLAIM_MONTH >= @FROMDATE AND 
				CLAIM_MONTH <= @TODATE  AND 
				BU_CODE = @BUCode AND
				SITE_CODE = @SITEID AND 
				--Claim_Category LIKE CASE WHEN @Claim_Category='' THEN '%' ELSE @Claim_Category END  AND 
				--CLAIM_SubCategory LIKE CASE WHEN @Claim_SubCategory='' THEN '%' ELSE @Claim_SubCategory END AND 
				--Claim_Type LIKE CASE WHEN @Claim_Type='' THEN '%' ELSE @Claim_Type END 
				 ((@Claim_Category='') OR  (Claim_Category= @Claim_Category )) AND 
				 ((@Claim_SubCategory='') OR(CLAIM_SubCategory= @Claim_SubCategory ))AND 
				((@Claim_Type='' ) OR (Claim_Type = @Claim_Type)) ) as CM  
			
				LEFT JOIN  
				[ax].[ACXTARGETCATEGORY] TC on CM.Claim_Category = TC.CATEGORY LEFT JOIN 
				[ax].[ACXTARGETSUBCATEGORY] TS on CM.CLAIM_SUBCATEGORY = TS.Subcategory LEFT JOIN 
				[ax].[ACXtargetheader] th on CM.Target_Code = th.targetcode LEFT JOIN 
				[ax].[ACXTARGETLINE] tl on th.TargetCode = tl.TargetCode 
					AND th.DataAreaId=tl.DataAreaId    
					AND CM.OBJECT=TL.TARGETOBJECT 
					AND CM.OBJECT_SUBCODE=TL.TARGETSUBOBJECT 
					AND CM.OBJECT_CODE=TL.OBJECTCODE 
					AND CM.FROM_DATE=TL.STARTDATE 
					AND CM.TO_DATE=TL.ENDDATE 
					AND CM.TARGET=TL.TARGET LEFT JOIN 
				[ax].[INVENTSITE] I on CM.SITE_CODE = I.Siteid   
END    
  
--SELECT * FROM [ax].[ACXCLAIMMASTER] WHERE SITE_CODE='0000600777' AND DOCUMENT_DATE>='2018-12-01'