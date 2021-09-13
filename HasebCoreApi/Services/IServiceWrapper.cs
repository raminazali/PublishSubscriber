using HasebCoreApi.Services.BankAccounts;
using HasebCoreApi.Services.Commodities;
using HasebCoreApi.Services.CommodityGroups;
using HasebCoreApi.Services.ContactUs;
using HasebCoreApi.Services.FixedDiscounts;
using HasebCoreApi.Services.Groups;
using HasebCoreApi.Services.Guilds;
using HasebCoreApi.Services.Modules;
using HasebCoreApi.Services.Plans;
using HasebCoreApi.Services.Products;
using HasebCoreApi.Services.RealPersons;
using HasebCoreApi.Services.Units;
using HasebCoreApi.Services.Periods;
using HasebCoreApi.Services.PriodeTypes;
using HasebCoreApi.Services.Poses;
using HasebCoreApi.Services.Currencies;
using HasebCoreApi.Services.CurrencyDefinitions;
using HasebCoreApi.Services.Banks;
using HasebCoreApi.Services.BankBranches;
using HasebCoreApi.Services.Commissions;
using HasebCoreApi.Services.InitialPosInventories;
using HasebCoreApi.Services.InitialInventoryCores;
using HasebCoreApi.Services.InitialBankInventories;
using HasebCoreApi.Services.InitialCashDeskInventories;
using HasebCoreApi.Services.InitialPersonInventories;

namespace HasebCoreApi
{
    public interface IServiceWrapper
    {
        IUserService User { get; }
        IBranchService Branch { get; }
        IProvinceService Province { get; }
        ICityService City { get; }
        ITitleService Title { get; }
        IContactService Contact { get; }
        IPlanService Plan { get; }
        IModuleService Module { get; }
        IGuildService Guild { get; }
        IProductService Product { get; }
        IGroupService Group { get; }
        ICommodityService Commodity { get; }
        IUnitService Unit { get; }
        ICommodityGroupService CommodityGroup { get; }
        ILegalRealPersonService RealPerson { get; }
        IBankAccountsService BankAccounts { get; }
        IFixedDiscountService FixedDiscount { get; }
        IPeriodService PricePeriod { get; }
        IPeriodeTypeService PeriodeType { get; }
        IPosService Pos { get; }
        ICurrencyService Currency { get; }
        ICurrencyDefinitionsService CurrencyDefinition { get; }
        IBankService Bank { get; }
        IBankBranchService BankBranch { get; }
        ICommissionService Commission { get; }
        IInitialPosInventoryService InitialPosInventory { get; }
        IInitialInventoryCoreService InitialInventoryCore { get; }
        IInitialBankInventoryService InitialBankInventory { get; }
        IInitialCashDeskInventoryService InitialCashDeskInventory { get; }
        IInitialPersonInventoryService InitialPersonInventory { get; }
    }
}