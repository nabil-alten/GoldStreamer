USE [GoldStreamerDB]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.sp_globalprice_insert @id int output, @ask decimal(15,2), @bid decimal(15,2), @open decimal(15,2), @high decimal(15,2), @low decimal(15,2), @close decimal(15,2), @capture_time datetime 
AS
	BEGIN
		SET NOCOUNT ON;
		insert into GlobalPrice(Ask, Bid, [Open], High, Low, [Close], CaptureTime) values (@ask, @bid, @open, @high, @low, @close, @capture_time)
		set @id =SCOPE_IDENTITY()
	END
GO
