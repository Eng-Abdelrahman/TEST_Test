---------------------------------------tbl_Accessories--------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Accessories_ForInsert
ON [dbo].[tbl_Accessories]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Accessories_Users_CreatedBy] from inserted
 declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'كماليات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Accessories_ForUpdate
on [dbo].[tbl_Accessories]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Accessories_Users_ModidfiedBy]from inserted
      declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)  
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Accessories_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_Accessories_Users_ModidfiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'كماليات','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'كماليات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'كماليات','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End

go
----------------------------------------[dbo].[tbl_AvailableUnits]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_AvailableUnits_ForInsert
ON [dbo].[tbl_AvailableUnits]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id =  [FK_AvaliableUnits_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض وحدة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_AvailableUnits_ForUpdate
on [dbo].[tbl_AvailableUnits]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_AvaliableUnits_Users_ModifiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_AvaliableUnits_Users_ModifiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_AvaliableUnits_Users_ModifiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض وحدة','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض وحدة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض وحدة','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End

go

-------------------------------------------[dbo].[tbl_Branches]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Branches_ForInsert
ON [dbo].[tbl_Branches]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Branches_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'فرع','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Branches_ForUpdate
on [dbo].[tbl_Branches]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Branches_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Branches_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Branches_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'فرع','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'فرع','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'فرع','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Categories]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Categories_ForInsert
ON [dbo].[tbl_Categories]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Categories_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Categories','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Categories_ForUpdate
on [dbo].[tbl_Categories]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Categories_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Categories_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Categories_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Categories','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Categories','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Categories','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Clients]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Clients_ForInsert
ON [dbo].[tbl_Clients]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Clients_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمبل','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Clients_ForUpdate
on [dbo].[tbl_Clients]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Clients_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Clients_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Clients_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمبل','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمبل','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمبل','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_ClientsCalls]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_ClientsCalls_ForInsert
ON [dbo].[tbl_ClientsCalls]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_MarketingCalls_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مكالمات العميل','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_ClientsCalls_ForUpdate
on [dbo].[tbl_ClientsCalls]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_MarketingCalls_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_MarketingCalls_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_MarketingCalls_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مكالمات العميل','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مكالمات العميل','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مكالمات العميل','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Commissions]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Commissions_ForInsert
ON [dbo].[tbl_Commissions]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Commissions_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمولة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Commissions_ForUpdate
on [dbo].[tbl_Commissions]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Commissions_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Commissions_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Commissions_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمولة','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمولة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عمولة','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_ContractArchives]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_ContractArchives_ForInsert
ON [dbo].[tbl_ContractArchives]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_ContractArchives_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقود مؤرشفة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_ContractArchives_ForUpdate
on [dbo].[tbl_ContractArchives]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_ContractArchives_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_ContractArchives_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_ContractArchives_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقود مؤرشفة','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقود مؤرشفة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقود مؤرشفة','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_DemandUnits]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_DemandUnits_ForInsert
ON [dbo].[tbl_DemandUnits]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_DemandUnits_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب وحده','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_DemandUnits_ForUpdate
on [dbo].[tbl_DemandUnits]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_DemandUnits_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_DemandUnits_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_DemandUnits_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب وحده','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب وحده','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب وحده','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Departements]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Departements_ForInsert
ON [dbo].[tbl_Departements]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Depts_Users_CreatedBy]from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'قسم وظيفي','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Departements_ForUpdate
on [dbo].[tbl_Departements]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Depts_Users_ModidfiedBy]from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Depts_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Depts_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'قسم وظيفي','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'قسم وظيفي','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'قسم وظيفي','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_ExpectedContracts]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_ExpectedContracts_ForInsert
ON [dbo].[tbl_ExpectedContracts]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_ExpectContracts_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الإتفاقيات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_ExpectedContracts_ForUpdate
on [dbo].[tbl_ExpectedContracts]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_ExpectContracts_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_ExpectContracts_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_ExpectContracts_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الإتفاقيات','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الإتفاقيات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الإتفاقيات','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_FinancialItems]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_FinancialItems_ForInsert
ON [dbo].[tbl_FinancialItems]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_FinancialItems_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الحسابات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_FinancialItems_ForUpdate
on [dbo].[tbl_FinancialItems]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_FinancialItems_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_FinancialItems_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_FinancialItems_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الحسابات','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الحسابات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'الحسابات','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Finishings]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Finishings_ForInsert
ON [dbo].[tbl_Finishings]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Finishings_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'التشطيبات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Finishings_ForUpdate
on [dbo].[tbl_Finishings]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Finishings_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Finishings_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Finishings_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'التشطيبات','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'التشطيبات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'التشطيبات','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Offers]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Offers_ForInsert
ON [dbo].[tbl_Offers]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Offers_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Offers','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Offers_ForUpdate
on [dbo].[tbl_Offers]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Offers_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Offers_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Offers_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Offers','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Offers','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Offers','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_PaymentBasis]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_PaymentBasis_ForInsert
ON [dbo].[tbl_PaymentBasis]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_PaymentBasis_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'أسس الدفع','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_PaymentBasis_ForUpdate
on [dbo].[tbl_PaymentBasis]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_PaymentBasis_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_PaymentBasis_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_PaymentBasis_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'أسس الدفع','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'أسس الدفع','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'أسس الدفع','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Regions]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Regions_ForInsert
ON [dbo].[tbl_Regions]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Regions_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'المناطق','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Regions_ForUpdate
on [dbo].[tbl_Regions]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Regions_Users_ModidfiedBy] from inserted
       declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id) 
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Regions_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Regions_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'المناطق','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'المناطق','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'المناطق','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_RentAgreementHeaders]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_RentAgreementHeaders_ForInsert
ON [dbo].[tbl_RentAgreementHeaders]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_RentAgreements_Users_EmpId] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد إيجار','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_RentAgreementHeaders_ForUpdate
on [dbo].[tbl_RentAgreementHeaders]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_RentAgreements_Users_EmpId] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_RentAgreements_Users_EmpId],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_RentAgreements_Users_EmpId]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد إيجار','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد إيجار','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد إيجار','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_SaleAgreementHeaders]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_SaleAgreementHeaders_ForInsert
ON [dbo].[tbl_SaleAgreementHeaders]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_SalesHeaders_Users_EmpId]from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد بيع','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_SaleAgreementHeaders_ForUpdate
on [dbo].[tbl_SaleAgreementHeaders]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_SalesHeaders_Users_EmpId] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_SalesHeaders_Users_EmpId],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_SalesHeaders_Users_EmpId]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد بيع','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد بيع','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عقد بيع','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Specializations]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Specializations_ForInsert
ON [dbo].[tbl_Specializations]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Specialization_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'تخصص وظيفي','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Specializations_ForUpdate
on [dbo].[tbl_Specializations]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Specialization_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Specialization_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Specialization_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'تخصص وظيفي','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'تخصص وظيفي','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'تخصص وظيفي','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_StaticContracts]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_StaticContracts_ForInsert
ON [dbo].[tbl_StaticContracts]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_StatContract_Users_CreatedBy]from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_StaticContracts','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_StaticContracts_ForUpdate
on [dbo].[tbl_StaticContracts]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_StatContract_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_StatContract_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_StatContract_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_StaticContracts','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_StaticContracts','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_StaticContracts','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Transactions]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Transactions_ForInsert
ON [dbo].[tbl_Transactions]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Transactions_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Transactions','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Transactions_ForUpdate
on [dbo].[tbl_Transactions]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Transactions_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Transactions_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Transactions_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Transactions','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Transactions','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,'tbl_Transactions','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Views]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Views_ForInsert
ON [dbo].[tbl_Views]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_Views_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'إطلالة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Views_ForUpdate
on [dbo].[tbl_Views]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_Views_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_Views_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_Views_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'إطلالة','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'إطلالة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'إطلالة','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
-------------------------------------------[dbo].[tbl_Users]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_Users_ForInsert
ON [dbo].[tbl_Users]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مستخدم','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_Users_ForUpdate
on [dbo].[tbl_Users]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id =[ModifiedBy]from inserted
       declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id) 
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [ModifiedBy],
            @NewIsDeleted = [IsActive]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsActive]
            from deleted where  [ModifiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مستخدم','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مستخدم','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'مستخدم','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End

go

-------------------------------------------[dbo].[tbl_PreviewDetails]----------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_PreviewDetails_ForInsert
ON [dbo].[tbl_PreviewDetails]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_PreviewDetails_Users_ModidfiedBy] from inserted
 Declare @HeaderId int
 select @HeaderId=[Fk_PreviewDetails_PreviewHeaders_Id] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date],[Description])
 values(Cast(@Id as nvarchar(5)),@UserName,N'معاينة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100),'PreviewHeaders_Id = ' + Cast(@HeaderId as nvarchar(5)))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_PreviewDetails_ForUpdate
on [dbo].[tbl_PreviewDetails]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id =[FK_PreviewDetails_Users_ModidfiedBy] from inserted
        Declare @HeaderId int
 select @HeaderId=[Fk_PreviewDetails_PreviewHeaders_Id] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_PreviewDetails_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where  [FK_PreviewDetails_Users_ModidfiedBy]= @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date],[Description])
 values(Cast(@Id as nvarchar(5)),@UserName,N'معاينة','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100),'PreviewHeaders_Id = ' + Cast(@HeaderId as nvarchar(5)))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date],[Description])
 values(Cast(@Id as nvarchar(5)),@UserName,N'معاينة','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100),'PreviewHeaders_Id = ' + Cast(@HeaderId as nvarchar(5)))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date],[Description])
 values(Cast(@Id as nvarchar(5)),@UserName,N'معاينة','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100),'PreviewHeaders_Id = ' + Cast(@HeaderId as nvarchar(5)))
  end
End
go
----------------------------------------[dbo].[dbo].[tbl_VillasAvailables]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_VillasAvailables_ForInsert
ON [dbo].[tbl_VillasAvailables]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id =  [FK_VillasAvailables_Users_CreatedBy]from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض فيلا','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_VillasAvailables_ForUpdate
on [dbo].[tbl_VillasAvailables]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id =[FK_VillasAvailables_Users_ModidfiedBy]  from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_VillasAvailables_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_VillasAvailables_Users_ModidfiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض فيلا','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض فيلا','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض فيلا','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End

go
----------------------------------------[dbo].[tbl_VillasDemands]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_VillasDemands_ForInsert
ON [dbo].[tbl_VillasDemands]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id =  [FK_VillasDemands_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب فيلا','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_VillasDemands_ForUpdate
on [dbo].[tbl_VillasDemands]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_VillasDemands_Users_ModidfiedBy]from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_VillasDemands_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_VillasDemands_Users_ModidfiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب فيلا','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب فيلا','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب فيلا','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
----------------------------------------[dbo].[tbl_AvailableLands]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
Alter TRIGGER tr_tbl_AvailableLands_ForInsert
ON [dbo].[tbl_AvailableLands]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_AvaliableLands_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض أرض','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_AvailableLands_ForUpdate
on [dbo].[tbl_AvailableLands]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_AvaliableLands_Users_ModifiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_AvaliableLands_Users_ModifiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_AvaliableLands_Users_ModifiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض أرض','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض أرض','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض أرض','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go 
----------------------------------------[dbo].[tbl_LandsDemands]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_LandsDemands_ForInsert
ON [dbo].[tbl_LandsDemands]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_LandsDemands_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب أرض','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_LandsDemands_ForUpdate
on [dbo].[tbl_LandsDemands]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_LandsDemands_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_LandsDemands_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_LandsDemands_Users_ModidfiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب أرض','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب أرض','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب أرض','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go
----------------------------------------[dbo].[tbl_ShopAvailable]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_ShopAvailable_ForInsert
ON [dbo].[tbl_ShopAvailable]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_ShopAvailable_Users_CreatedBy]from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض محلات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_ShopAvailable_ForUpdate
on [dbo].[tbl_ShopAvailable]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_ShopAvailable_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_ShopAvailable_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_ShopAvailable_Users_ModidfiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض محلات','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض محلات','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'عرض محلات','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End
go 
----------------------------------------[dbo].[tbl_ShopDemands]-------------------------------------------------------------------
------------------------------------------Insert-------------------------------------------------------------------------
ALTER TRIGGER tr_tbl_ShopDemands_ForInsert
ON [dbo].[tbl_ShopDemands]
FOR INSERT
AS
BEGIN

 Declare @Id int
 Select @Id = [FK_ShopDemands_Users_CreatedBy] from inserted
  declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب محل','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
 
END
go
--------------------------------------------------Update---------------------------------------------------------------
ALTER trigger tr_tbl_ShopDemands_ForUpdate
on [dbo].[tbl_ShopDemands]
for Update
as
Begin

	  -- Declare variables to hold old and updated data
      Declare @Id int
 Select @Id = [FK_ShopDemands_Users_ModidfiedBy] from inserted
        declare @UserName Nvarchar(50)
 set @UserName =(select CONCAT([FirstName],' ',[LastName]) from [dbo].[tbl_Users] where [PK_Users_Id]=@Id)
  Select *
      into #TempTable
      from inserted
	  Declare @OldIsDeleted int, @NewIsDeleted int
	  Select Top 1 @Id = [FK_ShopDemands_Users_ModidfiedBy],
            @NewIsDeleted = [IsDeleted]
            from #TempTable
           
            -- Select the corresponding row from deleted table
            Select @OldIsDeleted = [IsDeleted]
            from deleted where [FK_ShopDemands_Users_ModidfiedBy] = @Id
   if(@NewIsDeleted =1)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب محل','Delete',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if((@OldIsDeleted <>@NewIsDeleted )and @NewIsDeleted=0)
   begin
 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب محل','Insert',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
     end 
	 if(@OldIsDeleted = @NewIsDeleted)
	 begin
	 insert into  [dbo].[tbl_EventLogs]
 ([FK_Event_Users_Id],[UserName],[TableName],[EventType],[Date])
 values(Cast(@Id as nvarchar(5)),@UserName,N'طلب محل','Update',convert(nvarchar(20),dateadd(MINUTE,datepart(tz,cast(getUTCdate() as datetime) AT Time Zone 'Egypt Standard Time'),Getutcdate()),100))
  end

End