/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [T2TradeEntity_ID]
      ,[T2TradeId]
      ,[T2OrderId]
      ,[ExchangeType]
      ,[Symbol]
      ,[TradeId]
      ,[OrderId]
	  ,[TradeTime]
	  ,[IsBuyer]
      ,[Price]
      ,[Quantity]
      ,[QuoteQuantity]
      ,[Commission]
      ,[CommissionAsset]
      ,[Created]
      ,[Updated]
      ,[Timestamp]
      ,[IsBestMatch]
      ,[IsMaker]
      ,[T2SymbolInfoId]
      ,[QuoteUsdValue]
      ,[T2TradeGroupEntityId]
  FROM [TradingTools_DB].[dbo].[T2Trades]
  where symbol like '%LTC%'
  order by tradetime desc

--  SELECT Symbol, SUM(QuoteQuantity), AVG(Price)
--FROM [TradingTools_DB].[dbo].[T2Trades] 
--WHERE QuoteQuantity IS NOT NULL   
--    AND QuoteQuantity != 0.00   
--    AND Symbol LIKE 'RLC%'  
--	AND T2TradeEntity_ID >= 12553
--GROUP BY Symbol  
--ORDER BY Symbol;  
--GO  

