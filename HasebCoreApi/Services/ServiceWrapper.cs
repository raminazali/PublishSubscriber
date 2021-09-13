using HasebCoreApi.Helpers;
using HasebCoreApi.Models;
using HasebCoreApi.Services;
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
using Microsoft.Extensions.Options;
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
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly AppSettings _appSettings;
        private readonly SmtpConfig _config;
        private readonly FTPServer _ftp;

        private IUserService _user;
        private IProvinceService _province;
        private ICityService _city;
        private ITitleService _title;
        private IPlanService _plan;
        private IContactService _contact;
        private IGuildService _guild;
        private IModuleService _module;
        private IProductService _product;
        private IGroupService _group;
        private IBranchService _branch;
        private ICommodityService _commodity;
        private IUnitService _unit;
        private ICommodityGroupService _commodityGroup;
        private ILegalRealPersonService _realPerson;
        private IBankAccountsService _bankaccount;
        private IFixedDiscountService _fixedDiscount;
        private IPeriodService _pricePeriod;
        private IPeriodeTypeService _periodeType;
        private IPosService _pos;
        private ICurrencyService _currency;
        private ICurrencyDefinitionsService _currencyDefinitions;
        private IBankService _bank;
        private IBankBranchService _bankBranch;
        private ICommissionService _commission;
        private IInitialPosInventoryService _initialPosInventory;
        private IInitialInventoryCoreService _initialInventoryCore;
        private IInitialBankInventoryService _initialBankInventory;
        private IInitialCashDeskInventoryService _initialCashDeskInventory;
        private IInitialPersonInventoryService _initialPersonInventory;

        readonly IMongoRepository<UserInfo> _userRepo;
        readonly IMongoRepository<Validation> _validationRepo;
        readonly IMongoRepository<Province> _provinceRepo;
        readonly IMongoRepository<City> _cityRepo;
        readonly IMongoRepository<Title> _titleRepo;
        readonly IMongoRepository<Contactus> _contactusRepo;
        readonly IMongoRepository<Plan> _planRepo;
        readonly IMongoRepository<Guild> _guildRepo;
        readonly IMongoRepository<Module> _moduleRepo;
        readonly IMongoRepository<Product> _productRepo;
        readonly IMongoRepository<Group> _groupRepo;
        readonly IMongoRepository<Branch> _branchRepo;
        readonly IMongoRepository<Commodity> _commodityRepo;
        readonly IMongoRepository<Unit> _unitRepo;
        readonly IMongoRepository<CommodityGroup> _commodityGroupRepo;
        readonly IMongoRepository<Town> _townRepo;
        readonly IMongoRepository<LegalRealPerson> _realPersonRepo;
        readonly IMongoRepository<BankAccount> _bankAccountRepo;
        readonly IMongoRepository<FixedDiscount> _fixedDiscountRepo;
        readonly IMongoRepository<Period> _pricePeriodRepo;
        readonly IMongoRepository<PeriodType> _periodeTypeRepo;
        readonly IMongoRepository<Pos> _posRepo;
        readonly IMongoRepository<Currency> _currencyRepo;
        readonly IMongoRepository<CurrencyDefinition> _currencyDefinitionRepo;
        readonly IMongoRepository<Bank> _bankRepo;
        readonly IMongoRepository<BankBranch> _bankBranchRepo;
        readonly IMongoRepository<CommissionTbl> _commissionTblRepo;
        readonly IMongoRepository<InitialPosInventory> _initialPosInventoryRepo;
        readonly IMongoRepository<InitialInventoryCore> _initialInventoryCoreRepo;
        readonly IMongoRepository<InitialBankInventory> _initialBankInventoryRepo;
        readonly IMongoRepository<InitialCashDeskInventory> _initialCashDeskInventoryRepo;
        readonly IMongoRepository<InitialPersonInventory> _initialPersonInventoryRepo;
        public ServiceWrapper(
         IOptions<AppSettings> appSettings,
         IOptions<SmtpConfig> config,
         IOptions<FTPServer> ftp,
         //-------------------------------------
         IMongoRepository<UserInfo> userRepo,
         IMongoRepository<Validation> validationRepo,
         IMongoRepository<Province> provinceRepo,
         IMongoRepository<City> cityRepo,
         IMongoRepository<Title> titleRepo,
         IMongoRepository<Contactus> contactusRepo,
         IMongoRepository<Town> townRepo,
         IMongoRepository<Plan> planRepo,
         IMongoRepository<Guild> guildRepo,
         IMongoRepository<Module> moduleRepo,
         IMongoRepository<Product> productRepo,
         IMongoRepository<Group> groupRepo,
         IMongoRepository<Branch> branchRepo,
         IMongoRepository<Commodity> commodityRepo,
         IMongoRepository<Unit> unitRepo,
         IMongoRepository<CommodityGroup> commodityGroupRepo,
         IMongoRepository<LegalRealPerson> realPersonRepo,
         IMongoRepository<FixedDiscount> fixedDiscountRepo,
         IMongoRepository<BankAccount> bankAccountRepo,
         IMongoRepository<Period> pricePeriodRepo,
         IMongoRepository<PeriodType> periodeType,
         IMongoRepository<Pos> posRepo,
         IMongoRepository<Currency> currencyRepo,
         IMongoRepository<CurrencyDefinition> currencyDefinitionRepo,
         IMongoRepository<Bank> bankRepo,
         IMongoRepository<BankBranch> bankBranchRepo,
         IMongoRepository<CommissionTbl> commissionTblRepo,
         IMongoRepository<InitialPosInventory> initialPosInventoryRepo,
         IMongoRepository<InitialInventoryCore> initialInventoryCoreRepo,
         IMongoRepository<InitialBankInventory> initialBankInventoryRepo,
         IMongoRepository<InitialCashDeskInventory> initialCashDeskInventoryRepo,
         IMongoRepository<InitialPersonInventory> initialPersonInventoryRepo
            )
        {
            _appSettings = appSettings.Value;
            _config = config.Value;
            _ftp = ftp.Value;
            _userRepo = userRepo;
            _validationRepo = validationRepo;
            _provinceRepo = provinceRepo;
            _cityRepo = cityRepo;
            _titleRepo = titleRepo;
            _contactusRepo = contactusRepo;
            _townRepo = townRepo;
            _unitRepo = unitRepo;
            _commodityGroupRepo = commodityGroupRepo;
            _commodityRepo = commodityRepo;
            _planRepo = planRepo;
            _guildRepo = guildRepo;
            _moduleRepo = moduleRepo;
            _productRepo = productRepo;
            _groupRepo = groupRepo;
            _branchRepo = branchRepo;
            _realPersonRepo = realPersonRepo;
            _bankAccountRepo = bankAccountRepo;
            _fixedDiscountRepo = fixedDiscountRepo;
            _pricePeriodRepo = pricePeriodRepo;
            _periodeTypeRepo = periodeType;
            _posRepo = posRepo;
            _currencyRepo = currencyRepo;
            _currencyDefinitionRepo = currencyDefinitionRepo;
            _bankRepo = bankRepo;
            _bankBranchRepo = bankBranchRepo;
            _commissionTblRepo = commissionTblRepo;
            _initialPosInventoryRepo = initialPosInventoryRepo;
            _initialInventoryCoreRepo = initialInventoryCoreRepo;
            _initialBankInventoryRepo = initialBankInventoryRepo;
            _initialCashDeskInventoryRepo = initialCashDeskInventoryRepo;
            _initialPersonInventoryRepo = initialPersonInventoryRepo;
        }

        public IUserService User
        {
            get
            {
                if (_user == null)
                {
                    // _user = new UserService(_appSettings, _config, _userRepo, _validationRepo);
                }

                return _user;
            }
        }
        public IInitialPersonInventoryService InitialPersonInventory
        {
            get
            {
                if (_initialPersonInventory == null)
                {
                    _initialPersonInventory = new InitialPersonInventoryService(_initialPersonInventoryRepo);
                }

                return _initialPersonInventory;
            }
        }
        public IInitialCashDeskInventoryService InitialCashDeskInventory
        {
            get
            {
                if (_initialCashDeskInventory == null)
                {
                    _initialCashDeskInventory = new InitialCashDeskInventoryService(_initialCashDeskInventoryRepo);
                }
                return _initialCashDeskInventory;
            }
        }
        public IInitialBankInventoryService InitialBankInventory
        {
            get
            {
                if (_initialBankInventory == null)
                {
                    _initialBankInventory = new InitialBankInventoryService(_initialBankInventoryRepo);
                }
                return _initialBankInventory;
            }
        }
        public IInitialInventoryCoreService InitialInventoryCore
        {
            get
            {
                if (_initialInventoryCore == null)
                {
                    _initialInventoryCore = new InitialInventoryCoreService(_initialPosInventoryRepo, _initialInventoryCoreRepo, _initialBankInventoryRepo, _initialCashDeskInventoryRepo, _initialPersonInventoryRepo);
                }
                return _initialInventoryCore;
            }
        }
        public IInitialPosInventoryService InitialPosInventory
        {
            get
            {
                if (_initialPosInventory == null)
                {
                    _initialPosInventory = new InitialPosInventoryService(_initialPosInventoryRepo);
                }

                return _initialPosInventory;
            }
        }
        public ICurrencyDefinitionsService CurrencyDefinition
        {
            get
            {
                if (_currencyDefinitions == null)
                {
                    _currencyDefinitions = new CurrencyDefinitionsService(_currencyDefinitionRepo, _branchRepo, _currencyRepo);
                }

                return _currencyDefinitions;
            }
        }
        public ICommissionService Commission
        {
            get
            {
                if (_commission == null)
                {
                    _commission = new CommissionService(_commissionTblRepo, _branchRepo);
                }

                return _commission;
            }
        }
        public IBankBranchService BankBranch
        {
            get
            {
                if (_bankBranch == null)
                {
                    _bankBranch = new BankBranchService(_bankBranchRepo, _branchRepo, _bankAccountRepo);

                }
                return _bankBranch;
            }
        }
        public IBankService Bank
        {
            get
            {
                if (_bank == null)
                {
                    _bank = new BankService(_bankRepo);
                }

                return _bank;
            }
        }
        public ICurrencyService Currency
        {
            get
            {
                if (_currency == null)
                {
                    _currency = new CurrencyService(_currencyRepo);
                }

                return _currency;
            }
        }
        public IPosService Pos
        {
            get
            {
                if (_pos == null)
                {
                    _pos = new PosService(_posRepo, _currencyRepo, _bankAccountRepo, _branchRepo, _initialPosInventoryRepo);
                }

                return _pos;
            }
        }
        public IPeriodeTypeService PeriodeType
        {
            get
            {
                if (_periodeType == null)
                {
                    _periodeType = new PeriodeTypeService(_periodeTypeRepo, _branchRepo);
                }

                return _periodeType;
            }
        }
        public IFixedDiscountService FixedDiscount
        {
            get
            {
                if (_fixedDiscount == null)
                {
                    _fixedDiscount = new FixedDiscountService(_fixedDiscountRepo, _branchRepo);
                }

                return _fixedDiscount;
            }
        }
        public IBankAccountsService BankAccounts
        {
            get
            {
                if (_bankaccount == null)
                {
                    _bankaccount = new BankAccountsService(_bankAccountRepo, _branchRepo, _currencyRepo, _bankBranchRepo, _bankRepo, _initialBankInventoryRepo);
                }

                return _bankaccount;
            }
        }
        public ILegalRealPersonService RealPerson
        {
            get
            {
                if (_realPerson == null)
                {
                    _realPerson = new LegalRealPersonService(_realPersonRepo, _branchRepo , _initialPersonInventoryRepo);
                }

                return _realPerson;
            }
        }
        public ICommodityGroupService CommodityGroup
        {
            get
            {
                if (_commodityGroup == null)
                {
                    _commodityGroup = new CommodityGroupService(_commodityGroupRepo, _commodityRepo);
                }

                return _commodityGroup;
            }
        }
        public IUnitService Unit
        {
            get
            {
                if (_unit == null)
                {
                    _unit = new UnitService(_unitRepo, _branchRepo, _commodityRepo);
                }

                return _unit;
            }
        }
        public ICommodityService Commodity
        {
            get
            {
                if (_commodity == null)
                {
                    _commodity = new CommodityService(_ftp, _commodityRepo, _commodityGroupRepo, _branchRepo, _unitRepo, _pricePeriodRepo);
                }

                return _commodity;
            }
        }
        public IGroupService Group
        {
            get
            {
                if (_group == null)
                {
                    _group = new GroupService(_groupRepo);
                }

                return _group;
            }
        }
        public IBranchService Branch
        {
            get
            {
                if (_branch == null)
                {
                    _branch = new BranchService(_branchRepo, _userRepo, _planRepo);
                }

                return _branch;
            }
        }
        public IGuildService Guild
        {
            get
            {
                if (_guild == null)
                {
                    _guild = new GuildService(_guildRepo);
                }

                return _guild;
            }
        }
        public IProductService Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductService(_productRepo);
                }

                return _product;
            }
        }
        public IProvinceService Province
        {
            get
            {
                if (_province == null)
                {
                    _province = new ProvinceService(_provinceRepo);
                }

                return _province;
            }
        }
        public ICityService City
        {
            get
            {
                if (_city == null)
                {
                    _city = new CityService(_cityRepo, _provinceRepo);
                }

                return _city;
            }
        }
        public ITitleService Title
        {
            get
            {
                if (_title == null)
                {
                    _title = new TitleService(_titleRepo);
                }

                return _title;
            }
        }

        public IPlanService Plan
        {
            get
            {
                if (_plan == null)
                {
                    _plan = new PlanService(_planRepo);
                }

                return _plan;
            }
        }

        public IContactService Contact
        {
            get
            {
                if (_contact == null)
                {
                    _contact = new ContactService(_contactusRepo);
                }

                return _contact;
            }
        }

        public IModuleService Module
        {
            get
            {
                if (_module == null)
                {
                    _module = new ModuleService(_moduleRepo);
                }
                return _module;
            }
        }

        public IPeriodService PricePeriod
        {
            get
            {
                if (_pricePeriod == null)
                {
                    _pricePeriod = new PeriodService(_pricePeriodRepo, _commodityRepo, _branchRepo);
                }
                return _pricePeriod;
            }
        }
    }
}