use CycleDB
go
select * from Companies
select * from CycleModels
select * from Cycles
select * from StockInfoes
go
---for cycle Insert--
create proc spInsertCycle @CycleName nvarchar(50),
						  @EntryDate datetime,
						  @Status bit,
						  @Picture nvarchar(max),
						  @CompanyId int,
						  @ModelId int
as
begin
	insert into Cycles(CycleName,EntryDate,Status,Picture,CompanyId,ModelId)
	values(@CycleName,@EntryDate,@Status,@Picture,@CompanyId,@ModelId)
	select SCOPE_IDENTITY() as CycleId
	return
end
go
exec spInsertCycle 'Hero-M1','11-12-2022',1,'img-1.jpg',1,1
go
--insert stockinfo--
create proc spInsertStock @Category int,
						  @Amount decimal(18,0),
						  @Qty int,
						  @CycleId int
as
begin
	insert into StockInfoes(Category,Amount,Qty,CycleId)
	values(@Category,@Amount,@Qty,@CycleId)
	select SCOPE_IDENTITY() as StockId
	return
end
go
--for delete stock by cycleId--
create proc spDeleteStock @CycleId int
as
begin
	delete from StockInfoes
	where CycleId=@CycleId
	return
end
go

select * from Cycles
select * from StockInfoes


