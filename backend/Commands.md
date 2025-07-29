  ~~~sh
dotnet ef migrations add --project App.DAL.EF --startup-project WebApp --context AppDbContext addedVolume

dotnet ef migrations --project App.DAL.EF --startup-project WebApp remove

dotnet ef database --project App.DAL.EF --startup-project WebApp update
dotnet ef database --project App.DAL.EF --startup-project WebApp drop

~~~

MVC controllers
~~~sh
cd WebApp

dotnet aspnet-codegenerator controller -name ActionsController -actions -m App.Domain.Logic.ActionEntity -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ActionTypesController -actions -m App.Domain.Logic.ActionTypeEntity -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name AddressesController -actions -m App.Domain.Logic.Address -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ApprovalRequestsController -actions -m App.Domain.Logic.ApprovalRequests -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CurrentStocksController -actions -m App.Domain.Logic.CurrentStock -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name InventoriesController -actions -m App.Domain.Logic.Inventory -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductsController -actions -m App.Domain.Logic.Product -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductCategoriesController -actions -m App.Domain.Logic.ProductCategory -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProductInStorageRoomsController -actions -m App.Domain.Logic.ProductInStorageRoom -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ReasonsController -actions -m App.Domain.Logic.Reason -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name StockAuditsController -actions -m App.Domain.Logic.StockAudit -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name StockMovementsController -actions -m App.Domain.Logic.StockMovement -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name StorageRoomsController -actions -m App.Domain.Logic.StorageRoom -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name StorageRoomInInventoriesController -actions -m App.Domain.Logic.StorageRoomInInventory -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name SuppliersController -actions -m App.Domain.Logic.Supplier -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -name MonthlyStatisticsController -actions -m App.Domain.Logic.MonthlyStatistics -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

~~~

Api controllers
~~~sh
cd WebApp

dotnet aspnet-codegenerator controller -name ActionsController -actions -m App.Domain.Logic.ActionEntity -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ActionTypesController -actions -m App.Domain.Logic.ActionTypeEntity -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name AddressesController -actions -m App.Domain.Logic.Address -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ApprovalRequestsController -actions -m App.Domain.Logic.ApprovalRequests -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name CurrentStocksController -actions -m App.Domain.Logic.CurrentStock -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name InventoriesController -actions -m App.Domain.Logic.Inventory -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ProductsController -actions -m App.Domain.Logic.Product -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ProductCategoriesController -actions -m App.Domain.Logic.ProductCategory -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ProductInStorageRoomsController -actions -m App.Domain.Logic.ProductInStorageRoom -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ReasonsController -actions -m App.Domain.Logic.Reason -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name StockAuditsController -actions -m App.Domain.Logic.StockAudit -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name StockMovementsController -actions -m App.Domain.Logic.StockMovement -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name StorageRoomsController -actions -m App.Domain.Logic.StorageRoom -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name StorageRoomInInventoriesController -actions -m App.Domain.Logic.StorageRoomInInventory -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name SuppliersController -actions -m App.Domain.Logic.Supplier -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f

~~~