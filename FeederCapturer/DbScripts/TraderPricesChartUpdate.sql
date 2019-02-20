USE [GoldStreamerDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

alter PROCEDURE dbo.sp_traderpriceschart_update @global_price_id int --@ask decimal, @bid decimal, @capture_time datetime 
AS
	BEGIN
		SET NOCOUNT ON;
		declare @ask decimal(15,2), @bid decimal(15,2), @capture_time datetime
		declare @buy_average decimal(15,3), @sell_average decimal(15,3)
		select @ask = Ask, @bid = Bid, @capture_time = CaptureTime from GlobalPrice where Id = @global_price_id
		select @buy_average = round(AVG(Bid),2), @sell_average = round(AVG(Ask),2) from GlobalPrice where  cast(CaptureTime as varchar(10)) =  cast(@capture_time as varchar(10))
		if ((select count(*) from TraderPricesChart where  cast([Date] as varchar(10)) = cast(@capture_time as varchar(10)) and TraderID is null) > 0)
			begin
			   update TraderPricesChart set BuyAverage = @buy_average, SellAverage = @sell_average, SellClose = @ask, BuyClose = @bid where cast([Date] as varchar(10)) = cast(@capture_time as varchar(10)) 
			end
		else
			begin
				insert into TraderPricesChart(BuyAverage, SellAverage, BuyClose, SellClose, [Date]) values (@buy_average, @sell_average, @bid, @ask, convert(date, @capture_time))
			end
	END
GO
