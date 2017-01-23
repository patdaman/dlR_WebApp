USE [Golden]
GO

/****** Object:  StoredProcedure [dbo].[USER_SP_MOBILE_SAVED_ORDERS_EXIST]    Script Date: 7/18/2013 9:29:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Kevin Key
-- Create date: 2013-07-18
-- Description:	Check whether there are any saved orders.
-- =============================================

--EXEC USER_SP_MOBILE_SAVED_ORDERS_EXIST

CREATE PROCEDURE [dbo].[USER_SP_MOBILE_SAVED_ORDERS_EXIST]
AS
BEGIN --> 1
	SET NOCOUNT ON

	SELECT COUNT(*)
	FROM USER_SCANNED_ORDER
	WHERE SCAN_ORD_STATUS_CODE = 0		
END --< 1





GO


