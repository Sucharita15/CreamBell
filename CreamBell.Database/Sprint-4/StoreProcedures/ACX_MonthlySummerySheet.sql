USE [7200]
GO
/****** Object:  StoredProcedure [ax].[ACX_MonthlySummerySheet]    Script Date: 11/12/2019 5:46:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [ax].[ACX_MonthlySummerySheet] '0000600750','DL','01-SEP-2018','30-SEP-2018'          
          
ALTER Procedure [ax].[ACX_MonthlySummerySheet]          
(@SITEID nVarchar(max)='',@SITESTATE nVarchar(50)='', @StartDate smalldatetime,@EndDate smalldatetime,@BUCODE NVARCHAR(10)='')          
As          
Begin          
SET NOCOUNT ON;          
--exec [ax].[ACX_MonthlySummerySheet] '172000','','01-Jul-2016','31-Jul-2016'          
--exec [ax].[ACX_MonthlySummerySheet] '0000600018','','2017-08-01','2017-08-31'    


IF OBJECT_ID('TEMPDB..#TMPSITELIST') IS NOT NULL BEGIN DROP TABLE #TMPSITELIST END          
CREATE TABLE #TMPSITELIST (SITEID NVARCHAR(20));                    
IF LEN(@SITEID)>0                     
BEGIN INSERT INTO #TMPSITELIST  SELECT ID FROM DBO.CommaDelimitedToTable(@SITEID,',')   END  

      
IF OBJECT_ID('TEMPDB..#TMPMonthlySummery') IS NOT NULL BEGIN DROP TABLE #TMPMonthlySummery END          
create table #TMPMonthlySummery          
(          
 PART VARCHAR(1),          
 SRL INT,          
    PARTYNAME nVarchar(100),           
    TOWN nVarchar(50),           
    SITESTATE nVarchar(50),           
    ClaimMonth nVarchar(50),          
    SECTION nVarchar(200),           
    Details  nVarchar(50),          
    BOX decimal(18,2),           
 LTR decimal(18,2),          
    AMOUNT decimal(18,2),           
    CLAIMAMOUNT decimal(18,2),          
 REMARK nVarchar(200)          
)          
Insert Into #TMPMonthlySummery           
values('A',0,'','','','','Distributor Primary and Secondry [ ICE CREAM ]','',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',1,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Opening Stock',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',2,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Add PrimaryPurchase',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',3,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Adjustment\Movement',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',4,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Total Stock',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',5,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Secondary Retail',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',6,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Secondary Vending Only VRS',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',7,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Secondary Sub Distributor Only Sub Distributor',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',8,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Total Secondary Sale',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',9,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Total Secondary Sale at Primary Price',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('A',10,'','',@SITESTATE,'','Distributor Primary and Secondry [ ICE CREAM ]','Closing Stock',0,0,0,0,'')          
          
          
Insert Into #TMPMonthlySummery           
values('B',0,'','','','','Distributor Primary and Secondry [FROZEN]','',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',1,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Opening Stock Frozen',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',2,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Primary Purchase Frozen',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',3,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Adjustment\Movement Frozen',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',4,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Total Stock Frozen',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',5,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Secondary Sale Frozen',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',6,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Total Secondary Sale Frozen',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('B',7,'','',@SITESTATE,'','Distributor Primary and Secondry [FROZEN]','Net Closing Stock Frozen',0,0,0,0,'')          
          
Insert Into #TMPMonthlySummery           
values('C',0,'','','','','Push Cart','',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('C',1,'','','','','Push Cart Total Available','',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('C',2,'','','','','Push Cart On Road','',0,0,0,0,'')  Insert Into #TMPMonthlySummery           
values('C',3,'','','','','Utilization %age','',0,0,0,0,'')          
Insert Into #TMPMonthlySummery           
values('C',4,'','','','','Number of VRS Count','',0,0,0,0,'')          
          
declare @OpeningBox decimal(18,2),@OpeningLtr decimal(18,2),@OpeningAmount decimal(18,2)          
  ,@AdjBox decimal(18,2),@AdjLtr decimal(18,2),@AdjAmount decimal(18,2)          
  ,@TotalPrimayBox decimal(18,2),@TotalPrimaryLtr decimal(18,2),@TotalPrimaryAmount decimal(18,2)           
  ,@ReturnPrimayBox decimal(18,2),@ReturnPrimaryLtr decimal(18,2),@ReturnPrimaryAmount decimal(18,2)           
  ,@SecRetailBox decimal(18,2),@SecRetailLtr decimal(18,2),@SecRetailAmount decimal(18,2)          
  ,@SecVRSBox decimal(18,2),@SecVRSLtr decimal(18,2),@SecVRSAmount decimal(18,2)          
  ,@SecSDBox decimal(18,2),@SecSDLtr decimal(18,2),@SecSDAmount decimal(18,2),@OpeningDate smalldatetime          
  ,@SecPurchaseAmount decimal(18,2),@ClosingAmount decimal(18,2)
              
declare @FOpeningBox decimal(18,2),@FOpeningLtr decimal(18,2),@FOpeningAmount  decimal(18,2)          
  ,@FAdjBox decimal(18,2),@FAdjLtr decimal(18,2),@FAdjAmount  decimal(18,2)          
       ,@FTotalPrimayBox decimal(18,2),@FTotalPrimaryLtr decimal(18,2),@FTotalPrimaryAmount decimal(18,2)          
    ,@FSecRetailBox decimal(18,2),@FSecRetailLtr decimal(18,2),@FSecRetailAmount decimal(18,2)          
    ,@FReturnPrimayBox decimal(18,2),@FReturnPrimaryLtr decimal(18,2),@FReturnPrimaryAmount decimal(18,2),@FOpeningDate smalldatetime,@FClosingAmount decimal(18,2)          
              
declare @TotalPushCart decimal(18,2),@OnRoadPushCart decimal(18,2),@NoOfVRS decimal,@IsOpening int, @IsFOpening int        
    -- and           
SELECT @IsOpening =0,@IsFOpening=0        

--SET @ClosingAmount = (SELECT SUM(AMOUNT) FROM 
--				(SELECT SUM(T1.TransQty) AS TransQty,T1.ProductCode,SUM(T1.TransQty)*(SELECT TOP 1 ISNULL((O.TAXRATE),0) FROM  Ax.Acxinventtrans O 
--					WHERE O.PRODUCTCODE=T1.ProductCode AND O.SiteCode IN (SELECT SITEID FROM #TMPSITELIST) and 
--					O.TransLocation=(SELECT TOP 1 MAINWAREHOUSE FROM ax.INVENTSITE WHERE SITEID IN (SELECT SITEID FROM #TMPSITELIST)) ORDER BY O.InventTransDate DESC) AS AMOUNT
--					FROM AX.ACXINVENTTRANS T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
--					JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION AND [PRODUCT_GROUP]<>'FROZEN'          
--					AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
--					WHERE InventTransDate<=@EndDate AND SITEID IN (SELECT SITEID FROM #TMPSITELIST)   
--					GROUP BY T1.ProductCode
--					HAVING SUM(T1.TransQty) !=0) AA  )

--SET @FClosingAmount = (SELECT SUM(AMOUNT) FROM 
--				(SELECT SUM(T1.TransQty) AS TransQty,T1.ProductCode,SUM(T1.TransQty)*(SELECT TOP 1 ISNULL((O.TAXRATE),0) FROM  Ax.Acxinventtrans O 
--					WHERE O.PRODUCTCODE=T1.ProductCode AND O.SiteCode IN (SELECT SITEID FROM #TMPSITELIST) and 
--					O.TransLocation=(SELECT TOP 1 MAINWAREHOUSE FROM ax.INVENTSITE WHERE SITEID IN (SELECT SITEID FROM #TMPSITELIST)) ORDER BY O.InventTransDate DESC) AS AMOUNT
--					FROM AX.ACXINVENTTRANS T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
--					JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION AND [PRODUCT_GROUP]='FROZEN'      
--					AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
--					WHERE InventTransDate<=@EndDate AND SITEID IN (SELECT SITEID FROM #TMPSITELIST)   
--					GROUP BY T1.ProductCode
--					HAVING SUM(T1.TransQty) !=0) AA  )


IF (select Count(*) from [ax].[ACXINVENTTRANS] T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
 JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION           
 AND [PRODUCT_GROUP]<>'FROZEN' AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END Where SiteCode IN (SELECT SITEID FROM #TMPSITELIST) and DocumentDate<@StartDate and DocumentType in (1,2,3,4,5,6,7,8,10))=0           
BEGIN          
 SELECT @IsOpening=1,@OpeningBox=ISNULL(SUM(TRANSQTY),0),@OpeningLtr=ISNULL(SUM(T1.LTR),0),          
 @OpeningDate=ISNULL(MIN(DOCUMENTDATE),'1900-01-01'),          
  @OpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN T1.PURCH_VAL ELSE  T1.AMOUNT END),0)          
  --@OpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN T1.AMOUNT ELSE  ABS(T1.AMOUNT) END),0)          
 FROM AX.ACXINVENTTRANS T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
 JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION AND T1.DocumentType=7 AND [PRODUCT_GROUP]<>'FROZEN'          
 AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
 WHERE DocumentDate>= @startdate and DocumentDate<=@enddate AND T1.SITECODE IN (SELECT SITEID FROM #TMPSITELIST)          
END          
ELSE          
BEGIN          
 SELECT @OpeningBox=ISNULL(SUM(TRANSQTY),0),@OpeningLtr=ISNULL(SUM(T1.LTR),0),@OpeningDate=@STARTDATE,          
  @OpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN PURCH_VAL ELSE  T1.AMOUNT END),0)          
  --@OpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN T1.AMOUNT ELSE  T1.AMOUNT END),0)          
 FROM AX.ACXINVENTTRANS T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
 JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION AND [PRODUCT_GROUP]<>'FROZEN'          
 AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
 WHERE DOCUMENTDATE<@StartDate AND SITEID IN (SELECT SITEID FROM #TMPSITELIST)          
END          

IF (select Count(*) from [ax].[ACXINVENTTRANS] T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
 JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION           
 AND [PRODUCT_GROUP]='FROZEN' AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END Where SiteCode IN (SELECT SITEID FROM #TMPSITELIST) and DocumentDate<@StartDate and DocumentType in (1,2,3,4,5,6,7,8,10))=0           
BEGIN          
 SELECT @IsFOpening=1,@FOpeningBox=ISNULL(SUM(TRANSQTY),0),@FOpeningLtr=ISNULL(SUM(T1.LTR),0),@FOpeningDate=ISNULL(MIN(DOCUMENTDATE),'1900-01-01'),          
  --@FOpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN T1.AMOUNT ELSE  T1.AMOUNT END),0)          
 @FOpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN PURCH_VAL ELSE  T1.AMOUNT END),0)          
 FROM AX.ACXINVENTTRANS T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID           
 JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION AND T1.DocumentType=7           
 AND [PRODUCT_GROUP]='FROZEN' AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END WHERE DocumentDate>= @startdate and DocumentDate<=@enddate AND T1.SITECODE IN (SELECT SITEID FROM #TMPSITELIST)          
END          
ELSE          
BEGIN          
 SELECT @FOpeningBox=ISNULL(SUM(TRANSQTY),0),@FOpeningLtr=ISNULL(SUM(T1.LTR),0),@FOpeningDate=@STARTDATE,          
  --@FOpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN T1.AMOUNT ELSE  T1.AMOUNT END),0)          
  @FOpeningAmount=ISNULL(SUM(CASE WHEN TRANSTYPE IN (1,5) THEN PURCH_VAL ELSE  T1.AMOUNT END),0)          
 FROM AX.ACXINVENTTRANS T1 JOIN AX.INVENTTABLE T2 ON T1.PRODUCTCODE=T2.ITEMID          
 JOIN ax.INVENTSITE S1 ON S1.SITEID=T1.SITECODE AND S1.MAINWAREHOUSE=T1.TRANSLOCATION AND [PRODUCT_GROUP]='FROZEN'          
 AND T2.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
 WHERE DOCUMENTDATE<@StartDate AND SITEID IN (SELECT SITEID FROM #TMPSITELIST)          
END          
--ADJUSTMENT          
 print @AdjAmount  
 print @StartDate  
 print @EndDate  
SELECT @AdjBox=ISNULL(SUM(BOX),0),@FAdjBox=ISNULL(SUM(FBOX),0),@AdjLtr=ISNULL(SUM(LTR),0),@FAdjLtr=ISNULL(SUM(FLTR),0),@AdjAmount=ISNULL(SUM(AMOUNT),0),@FAdjAmount=ISNULL(SUM(FAMOUNT),0)          
FROM (          
 SELECT ISNULL((CASE WHEN  B1.[PRODUCT_GROUP] <> 'FROZEN' THEN ISNULL(A.TransQty,0) ELSE 0 END),0) BOX,          
  ISNULL((CASE WHEN  B1.[PRODUCT_GROUP] = 'FROZEN' THEN ISNULL(A.TransQty,0) ELSE 0 END),0) FBOX,          
  CASE WHEN  B1.[PRODUCT_GROUP] <> 'FROZEN' THEN A.LTR ELSE 0 END LTR,          
  CASE WHEN  B1.[PRODUCT_GROUP] = 'FROZEN' THEN A.LTR ELSE 0 END FLTR,          
  (CASE WHEN  B1.[PRODUCT_GROUP] <> 'FROZEN' THEN A.TransQty * A.TAXRATE ELSE 0 END) AMOUNT,          
  (CASE WHEN  B1.[PRODUCT_GROUP] = 'FROZEN' THEN A.TransQty * A.TAXRATE ELSE 0 END) FAMOUNT          
  FROM [ax].[ACXINVENTTRANS] A          
  Inner Join [ax].[INVENTTABLE] B1 on B1.ITEMID=A.ProductCode          
  JOIN ax.INVENTSITE S1 ON S1.mainwarehouse=A.TRANSLOCATION AND A.DocumentType IN (3,6,8,10)       
  AND B1.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END  
        Where A.SiteCode IN (SELECT SITEID FROM #TMPSITELIST) AND DocumentDate>=@StartDate AND DocumentDate<=@EndDate           
 ) RSTADJUSTMENT          


 SELECT ISNULL(SUM(BOX),0) box,ISNULL(SUM(FBOX),0) fbox,ISNULL(SUM(LTR),0) ltr,ISNULL(SUM(FLTR),0) fltr,ISNULL(SUM(AMOUNT),0) amount,ISNULL(SUM(FAMOUNT),0) famount         
 into #tmpadj  
FROM ( 
 SELECT ISNULL((CASE WHEN  B1.[PRODUCT_GROUP] <> 'FROZEN' THEN ISNULL(A.TransQty,0) ELSE 0 END),0) BOX,          
  ISNULL((CASE WHEN  B1.[PRODUCT_GROUP] = 'FROZEN' THEN ISNULL(A.TransQty,0) ELSE 0 END),0) FBOX,          
  CASE WHEN  B1.[PRODUCT_GROUP] <> 'FROZEN' THEN A.LTR ELSE 0 END LTR,          
  CASE WHEN  B1.[PRODUCT_GROUP] = 'FROZEN' THEN A.LTR ELSE 0 END FLTR,          
  (CASE WHEN  B1.[PRODUCT_GROUP] <> 'FROZEN' THEN A.TransQty * A.TAXRATE ELSE 0 END) AMOUNT,          
  (CASE WHEN  B1.[PRODUCT_GROUP] = 'FROZEN' THEN A.TransQty * A.TAXRATE ELSE 0 END) FAMOUNT          
  FROM [ax].[ACXINVENTTRANS] A          
  Inner Join [ax].[INVENTTABLE] B1 on B1.ITEMID=A.ProductCode          
  JOIN ax.INVENTSITE S1 ON S1.mainwarehouse=A.TRANSLOCATION AND A.DocumentType IN (3,6,8,10)          
  AND B1.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
        Where A.SiteCode IN (SELECT SITEID FROM #TMPSITELIST) AND DocumentDate>=@StartDate AND DocumentDate<=@EndDate           
 ) RSTADJUSTMENT          
print @AdjAmount  
SELECT @OpeningBox=case when @OpeningBox<0 then 0 else @OpeningBox end          
      ,@OpeningLtr=case when @OpeningLtr<0 then 0 else @OpeningLtr end          
   ,@OpeningAmount=case when @OpeningAmount<0 then 0 else @OpeningAmount end          
   ,@FOpeningBox=case when @FOpeningBox<0 then 0 else @FOpeningBox end          
      ,@FOpeningLtr=case when @FOpeningLtr<0 then 0 else @FOpeningLtr end          
   ,@FOpeningAmount=case when @FOpeningAmount<0 then 0 else @FOpeningAmount end          
             

Update #TMPMonthlySummery set BOX=@OpeningBox,Ltr=@OpeningLtr,Amount=@OpeningAmount,Details=Details+ case when @OpeningDate='1900-01-01' then '' else ' as on '+ convert(nvarchar,@OpeningDate ,106) end where Details='Opening Stock'              
Update #TMPMonthlySummery set BOX=@FOpeningBox,Ltr=@FOpeningLtr,Amount=@FOpeningAmount,Details=Details+case when @FOpeningDate='1900-01-01' then '' else ' as on '+ convert(nvarchar,@FOpeningDate,106) end where Details='Opening Stock Frozen'          
--Primary Purchase          
          
select @TotalPrimayBox=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0 ELSE A.Box END),0) as decimal(18,2))          
   ,@TotalPrimaryLtr=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0 ELSE A.LTR END),0) as decimal(18,2))          
   ,@TotalPrimaryAmount=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0 ELSE A.Amount + A.TRDDISCVALUE END),0) as decimal(18,2))          
   ,@FTotalPrimayBox=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN A.Box ELSE 0 END),0) as decimal(18,2))          
   ,@FTotalPrimaryLtr=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN A.LTR ELSE 0 END),0) as decimal(18,2))          
   ,@FTotalPrimaryAmount=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN A.Amount + A.TRDDISCVALUE ELSE 0 END),0) as decimal(18,2))          
       FROM [ax].[ACXPURCHINVRECIEPTLINE] A          
            Inner Join [ax].[ACXPURCHINVRECIEPTHEADER] C on A.[PURCH_RECIEPTNO]=C.[PURCH_RECIEPTNO]          
   and C.[SITE_CODE]=A.[SITEID] and A.[DATAAREAID]=C.[DATAAREAID]          
            Inner Join [ax].[INVENTTABLE] B on B.ITEMID=A.[PRODUCT_CODE] AND B.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END 
            Where A.[SITEID] IN (SELECT SITEID FROM #TMPSITELIST) --and C.[SALE_INVOICEDATE]>=@StartDate and C.[SALE_INVOICEDATE]<=@EndDate          
   and CONVERT(SMALLDATETIME,CONVERT(NVARCHAR,C.[DOCUMENT_DATE],106))>=@StartDate and CONVERT(SMALLDATETIME,CONVERT(NVARCHAR,C.[DOCUMENT_DATE],106))<=@EndDate          
--Purchase Return          
select @ReturnPrimayBox=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0 ELSE A.Box END),0) as decimal(18,2))          
   ,@ReturnPrimaryLtr=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0 ELSE A.LTR END),0) as decimal(18,2))          
   ,@ReturnPrimaryAmount=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0 ELSE A.Amount + A.TRDDISCVALUE END),0) as decimal(18,2))          
  ,@FReturnPrimayBox=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN A.Box ELSE 0 END),0) as decimal(18,0))          
  ,@FReturnPrimaryLtr=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN A.LTR ELSE 0 END),0) as decimal(18,2))          
  ,@FReturnPrimaryAmount=cast(ISNULL(sum(CASE B.[PRODUCT_GROUP] WHEN 'FROZEN' THEN A.Amount + A.TRDDISCVALUE ELSE 0 END),0) as decimal(18,2))          
            from [ax].[ACXPURCHRETURNLINE] A          
            Inner Join [ax].[ACXPURCHRETURNHEADER] C on  A.Purch_ReturnNo=C.Purch_ReturnNo and  A.SITEID=C.SITE_Code                        
   Inner Join [ax].[INVENTTABLE] B on B.ITEMID=A.PRODUCT_CODE          AND B.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
            Where A.[SITEID] IN (SELECT SITEID FROM #TMPSITELIST) and C.Purch_ReturnDate>=@StartDate and C.Purch_ReturnDate<=@EndDate          
--Update Temp Table for Primary Purchase           
          
Update #TMPMonthlySummery set BOX=@TotalPrimayBox-@ReturnPrimayBox          
       ,Ltr=@TotalPrimaryLtr-@ReturnPrimaryLtr,Amount=@TotalPrimaryAmount-@ReturnPrimaryAmount          
        where Details='Add PrimaryPurchase'          
Update #TMPMonthlySummery set BOX=@FTotalPrimayBox-@FReturnPrimayBox          
       ,Ltr=@FTotalPrimaryLtr-@FReturnPrimaryLtr,Amount=@FTotalPrimaryAmount-@FReturnPrimaryAmount          
       where Details='Primary Purchase Frozen'          
          
Update #TMPMonthlySummery set BOX=@AdjBox           
       ,Ltr=@AdjLtr           
       ,Amount=@AdjAmount          
    where Details='Adjustment\Movement'             
          
Update #TMPMonthlySummery set BOX=@FAdjBox           
       ,Ltr=@FAdjLtr           
       ,Amount=@FAdjAmount          
    where Details='Adjustment\Movement Frozen'             
          
Update #TMPMonthlySummery set BOX=@TotalPrimayBox-@ReturnPrimayBox+@OpeningBox+@AdjBox           
       ,Ltr=@TotalPrimaryLtr-@ReturnPrimaryLtr+@OpeningLtr+@AdjLtr           
       ,Amount=@TotalPrimaryAmount-@ReturnPrimaryAmount+@OpeningAmount +@AdjAmount          
    where Details='Total Stock'             
Update #TMPMonthlySummery set BOX=@FTotalPrimayBox-@FReturnPrimayBox+@FOpeningBox+@FAdjBox          
       ,Ltr=@FTotalPrimaryLtr-@FReturnPrimaryLtr+@FOpeningLtr+@FAdjLtr           
       ,Amount=@FTotalPrimaryAmount-@FReturnPrimaryAmount+@FOpeningAmount +@FAdjAmount          
    where Details='Total Stock Frozen'             
--For Sale Details('CG0002'=VRS and 'CG0004'=SubDistributor)          
select @SecRetailBox=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0004' then 0 when 'CG0002' then 0 else           
           (case when A.TranType=2 then -A.[BOX] else A.[BOX] end ) END) END),0) as decimal(18,0))           
       ,@SecSDBox=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0004' then           
        (case when A.TranType=2 then -A.[BOX] else A.[BOX] end ) when 'CG0002' then 0 else 0 end) END),0) as decimal(18,0))          
    ,@SecVRSBox=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0002' then           
        (case when A.TranType=2 then -A.[BOX] else A.[BOX] end ) when 'CG0004' then 0 else 0 end) END),0) as decimal(18,0))              
    ,@SecRetailLtr=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0004' then 0 when 'CG0002' then 0 else           
        (case when A.TranType=2 then -A.LTR else A.[LTR] END ) END) END),0) as decimal(18,2))          
    ,@SecSDLtr=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0004' then           
        (case when A.TranType=2 then -A.[LTR] else A.[LTR] end ) when 'CG0002' then 0 else 0 end) END),0) as decimal(18,2))          
    ,@SecVRSLtr=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0002' then           
        (case when A.TranType=2 then -A.[LTR] else A.[LTR] end ) when 'CG0004' then 0 else 0 end) END),0) as decimal(18,2))          
          
    ,@SecRetailAmount=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0004' then 0 when 'CG0002' then 0 else           
   (case when A.TranType=2 then -A.[Amount]-coalesce(A.TDvalue,0) else A.[Amount]+coalesce(A.[TDvalue],0) end ) end) END),0) as decimal(18,2))          
       ,@SecSDAmount=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0004' then           
   (case when A.TranType=2 then -A.[Amount]-coalesce(A.TDvalue,0) else A.[Amount]+coalesce(A.[TDvalue],0) end ) ELSE 0 end) END),0) as decimal(18,2))          
    ,@SecVRSAmount=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN 0           
   ELSE (case B.CustGroup_Code when 'CG0002' then           
   (case when A.TranType=2 then -A.[Amount]-coalesce(A.TDvalue,0) else A.[Amount]+coalesce(A.[TDvalue],0) end ) ELSE 0 end) END),0) as decimal(18,2)),          
  @FSecRetailBox=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN'           
   THEN (case when A.TranType=2 then -A.[BOX] else A.[BOX] end ) ELSE 0 END),0) as decimal(18,0)),          
  @FSecRetailLtr=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN'           
   THEN (case when A.TranType=2 then -A.[LTR] else A.[LTR] end ) ELSE 0 END),0) as decimal(18,2)),          
   @FSecRetailAmount=Cast(ISNULL(sum(CASE C.[PRODUCT_GROUP] WHEN 'FROZEN' THEN           
   (case when A.TranType=2 then -A.[Amount]-coalesce(A.TDvalue,0) else A.[Amount]+coalesce(A.[TDvalue],0) end )          
   ELSE 0 end),0) as decimal(18,2))          
    -- else A.[Amount]+coalesce(A.[Amount],0) end) as decimal(18,2))          
     from [ax].[ACXSALEINVOICELINE] A          
        inner join  [ax].[ACXSALEINVOICEHEADER] B on A.INVOICE_NO=B.INVOICE_NO and A.Siteid=B.Siteid           
  and A.[CUSTOMER_CODE]=B.[CUSTOMER_CODE] and A.[DATAAREAID]=B.[DATAAREAID]           
Inner Join [ax].[INVENTTABLE] C on C.ITEMID=A.[PRODUCT_CODE] AND C.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
        Where A.[SITEID] IN (SELECT SITEID FROM #TMPSITELIST) and B.[INVOIC_DATE]>=@StartDate and B.[INVOIC_DATE]<=@EndDate           
  --and SchemeCode=''           
--Update Temp Table for sale           
Update #TMPMonthlySummery set BOX=@SecRetailBox,Ltr=@SecRetailLtr,Amount=@SecRetailAmount          
       where Details='Secondary Retail'            
Update #TMPMonthlySummery set BOX=@SecVRSBox,Ltr=@SecVRSLtr,Amount=@SecVRSAmount          
       where Details='Secondary Vending Only VRS'           
Update #TMPMonthlySummery set BOX=@SecSDBox,Ltr=@SecSDLtr,Amount=@SecSDAmount          
       where Details='Secondary Sub Distributor Only Sub Distributor'           
          
Update #TMPMonthlySummery set BOX=@SecRetailBox+@SecVRSBox+@SecSDBox          
                             ,Ltr=@SecRetailLtr+@SecVRSLtr+@SecSDLtr          
        ,Amount=@SecRetailAmount+@SecVRSAmount+@SecSDAmount          
       where Details='Total Secondary Sale'           
Update #TMPMonthlySummery set BOX=@SecRetailBox+@SecVRSBox+@SecSDBox          
                             ,Ltr=@SecRetailLtr+@SecVRSLtr+@SecSDLtr          
        ,Amount=@SecRetailAmount+@SecVRSAmount+@SecSDAmount          
       where Details='Total Secondary Sale at Primary Price'           
          
                                                           
          
--For Total Sale At Primary Price          
Select @SecPurchaseAmount=isnull(SUM(AMOUNT),0) FROM (SELECT (case when B.TranType=2 then -A.BOX else A.BOX end ) * TAXPURCHRATE  AMOUNT          
from [ax].[ACXSALEINVOICELINE] A          
        inner join  [ax].[ACXSALEINVOICEHEADER] B on A.INVOICE_NO=B.INVOICE_NO and A.Siteid=B.Siteid           
  and A.[CUSTOMER_CODE]=B.[CUSTOMER_CODE] and A.[DATAAREAID]=B.[DATAAREAID]           
  Inner Join [ax].[INVENTTABLE] C on C.ITEMID=A.[PRODUCT_CODE] and C.[PRODUCT_GROUP]<>'FROZEN' AND C.BU_CODE LIKE CASE WHEN LEN(@BUCODE)>0 THEN @BUCODE ELSE '%' END
        Where A.[SITEID] IN (SELECT SITEID FROM #TMPSITELIST) and B.[INVOIC_DATE]>=@StartDate and B.[INVOIC_DATE]<=@EndDate ) AA          
          
Update #TMPMonthlySummery set BOX=(@OpeningBox+@TotalPrimayBox-@ReturnPrimayBox)-(@SecRetailBox+@SecVRSBox+@SecSDBox)+@AdjBox          
                             ,Ltr=(@OpeningLtr+@TotalPrimaryLtr-@ReturnPrimaryLtr)-(@SecRetailLtr+@SecVRSLtr+@SecSDLtr)+@AdjLtr          
        ,Amount=(@OpeningAmount+@TotalPrimaryAmount-@ReturnPrimaryAmount)-@SecPurchaseAmount+@AdjAmount        
		--,Amount=@ClosingAmount
        ---(@SecRetailAmount+@SecVRSAmount+@SecSDAmount)          
       where Details='Closing Stock'          
Update #TMPMonthlySummery set Amount=@SecPurchaseAmount          
       where Details='Total Secondary Sale at Primary Price'          
    --@FSecRetailBox decimal,@FSecRetailLtr decimal,@FSecRetailAmount decimal          
Update #TMPMonthlySummery set BOX=@FSecRetailBox          
                             ,Ltr=@FSecRetailLtr          
        ,Amount=@FSecRetailAmount          
       where Details='Secondary Sale Frozen'          
Update #TMPMonthlySummery set BOX=@FSecRetailBox          
                  ,Ltr=@FSecRetailLtr          
        ,Amount=@FSecRetailAmount          
       where Details='Total Secondary Sale Frozen'           
Update #TMPMonthlySummery set BOX=(@FTotalPrimayBox-@FReturnPrimayBox+@FOpeningBox)-@FSecRetailBox+@FAdjBox          
       ,Ltr=(@FTotalPrimaryLtr-@FReturnPrimaryLtr+@FOpeningLtr)-@FSecRetailLtr+@FAdjLtr          
       ,Amount=(@FTotalPrimaryAmount-@FReturnPrimaryAmount+@FOpeningAmount)-@FSecRetailAmount +@FAdjAmount          
	   --,Amount=@FClosingAmount
       where Details='Net Closing Stock Frozen'          
--Push Cart          
Select @TotalPushCart =(select count(distinct Customer_Code) from VW_CUSTOMERMASTER_HISTORY where Site_Code IN (SELECT SITEID FROM #TMPSITELIST) and Cust_Group='CG0002')       
Select @OnRoadPushCart=(select count(distinct(VRS_Code)) from [ax].[ACXVRSISSUEHEADER] where siteid IN (SELECT SITEID FROM #TMPSITELIST) and VRSISSUE_Date between @StartDate and @EndDate)          
Select @NoOfVRS=(select count(distinct VRSCODE) from [ax].[ACXVRSVDLinkingMaster] where sitecode IN (SELECT SITEID FROM #TMPSITELIST))          
--Update Temp Table Push Cart          
Update #TMPMonthlySummery set Box=@TotalPushCart where SECTION='Push Cart Total Available'           
Update #TMPMonthlySummery set Box=@OnRoadPushCart where SECTION='Push Cart On Road'           
Update #TMPMonthlySummery set Box=case when @TotalPushCart=0 then 0 else (@OnRoadPushCart/@TotalPushCart)*100 end where SECTION='Utilization %age'          
Update #TMPMonthlySummery set Box=@NoOfVRS where SECTION='Number of VRS Count'           
          
--Select Temp Table          
select * from #TMPMonthlySummery ORDER BY part,SRL          
--select * from #tmpadj  
end 