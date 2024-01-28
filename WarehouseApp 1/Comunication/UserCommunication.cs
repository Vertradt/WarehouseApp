using WarehouseApp.Data;
using WarehouseApp.DataProviders;
using WarehouseApp.Entities;
using WarehouseApp.Repositores;

namespace WarehouseApp.Comunication
{
    public class UserCommunication : IUserComunication
    {
        private SqlRepository<Ski> _skiRepository;
        private SqlRepository<Snowboard> _snowboardRepository;
        private SqlRepository<Shoe> _shoeRepository;
        private readonly SqlRepository<Helmet> _helmetsRepository;
        private readonly WarehouseAppDbContext _warehouseAppDbContext;
        private readonly IHelmetsProvider _helmetsProvider;

        public UserCommunication(SqlRepository<Ski> skiRepository,
            SqlRepository<Snowboard> snowboardRepository,
            SqlRepository<Shoe> shoeRepository,
            SqlRepository<Helmet> helmetsRepository,
            WarehouseAppDbContext warehouseAppDbContext,
            IHelmetsProvider helmetsProvider)
        {
            _skiRepository = skiRepository;
            _snowboardRepository = snowboardRepository;
            _shoeRepository = shoeRepository;
            _helmetsRepository = helmetsRepository;
            _warehouseAppDbContext = warehouseAppDbContext;
            _helmetsProvider = helmetsProvider;
        }

        public void Communication()
        {
            _skiRepository = new SqlRepository<Ski>(_warehouseAppDbContext);
            _snowboardRepository = new SqlRepository<Snowboard>(_warehouseAppDbContext);
            _shoeRepository = new SqlRepository<Shoe>(_warehouseAppDbContext);

            ActivateEventListeners();

            AddSampleHelmetsToDb();

            ShowMenu();


            var quit = false;

            while (quit != true)
            {
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddEquipment();
                        break;
                    case "2":
                        RemoveEquipment();
                        break;
                    case "3":
                        WriteAllToConsole();
                        break;
                    case "4":
                        GetMinimumPriceOfAllHelmets();
                        break;
                    case "5":
                        OrderByName();
                        break;
                    case "6":
                        WhereColorIsRed();
                        break;
                    case "7":
                        GetUniqueHelmetColors();
                        break;
                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("wrong option");
                        break;
                }
            }

            return;

            void EquipmentRepositoryOnItemAdded(object? sender, Equipment e)
            {
                var equipment = ($"Data: {DateTime.Now}, Equipment added => {e.Type} from {sender?.GetType().Name}");
                Console.WriteLine(equipment);
                using var writer = File.AppendText("Warehouse.txt");
                writer.WriteLine(equipment);
            }

            void EquipmentRepositoryOnItemRemove(object? sender, Equipment e)
            {
                var equipment =
                    $"Date:  {DateTime.Now}, Equipment remove => {e.Type}  from {sender?.GetType().Name}";
                Console.WriteLine(equipment);
                using var writer = File.AppendText("Warehouse.txt");
                writer.WriteLine(equipment);
            }


            void AddEquipment()//ech kusi by zrobić ładniej bo sporo powtarzalnego kodu... 
            {
                Console.WriteLine("Please provide the name of the equipment: Ski, Snowboard, Shoe, Helmet");
                var addedItemName = Console.ReadLine();
                if (addedItemName.ToLower().Contains("ski"))
                {
                    var ski = new Ski
                    {
                        Type = addedItemName, Name = addedItemName, SkiFlex = "low", Color = "black", ListPrice = 1500
                    };
                    _skiRepository.Add(ski);
                    _skiRepository.Save();
                }
                else if (addedItemName.ToLower().Contains("snowboard"))
                {
                    var snowboard = new Snowboard
                    {
                        Type = addedItemName, Name = addedItemName, SnowboardFlex = "low", Color = "black",
                        ListPrice = 1500
                    };
                    _snowboardRepository.Add(snowboard);
                    _snowboardRepository.Save();
                }
                else if (addedItemName.ToLower().Contains("shoe"))
                {
                    var shoe = new Shoe()
                    {
                        Type = addedItemName, Name = addedItemName, FeetSize = 45, Color = "black", ListPrice = 1500
                    };
                    _shoeRepository.Add(shoe);
                    _shoeRepository.Save();
                }
                else if (addedItemName.ToLower().Contains("helmet"))
                {
                    var helmet = new Helmet()
                    {
                        Type = addedItemName, Name = addedItemName, Color = "Red", ListPrice = 50
                    };
                    _helmetsRepository.Add(helmet);
                    _helmetsRepository.Save();
                }
            }

            void WriteAllToConsole()//tutaj nie dałem rady się powstrzymać xD 
            {
                Show("SKIES",_skiRepository);
                Show("\nSNOWBOARDS",_snowboardRepository);
                Show("\nSHOES",_shoeRepository);
                Show("\nHELMETS",_helmetsRepository);
            }
            
            //więc stworzyłem tą metodę i jest ślicznie
            void Show(string equipmentName, IReadRepository<Equipment> from)
            {
                Console.WriteLine(equipmentName + ":");
                foreach (var eq in from.GetAll())
                {
                    Console.WriteLine(eq);
                }
            }

            void RemoveEquipment()// a tutaj to już bardzo świeżbi by zrobić ślicznie wydzielając powtarzalny kod do osobnych metod - to będzie zadanie dla Ciebie by 
            {                           //upiększyć kodzik - ale to później, w ramach korepetycji to ogarniemy 
                Console.WriteLine("Select equipment to remove: Ski, Snowboard, Shoe, Helmet");
                var eqName = Console.ReadLine();

                Console.WriteLine("Available equipment");
                WriteAllToConsole(); // bez dodania tego Użytkownik nie ogarnie co chce usnąć - możesz usunąć ale wtedy na pamięć obsługa
                    
                try
                {
                    if (eqName.ToLower().Contains("ski"))
                    {
                        Console.WriteLine("Enter the id of equipment you want to delete");
                        _skiRepository.Remove(_skiRepository.GetById(int.Parse(Console.ReadLine())));
                        _skiRepository.Save();
                    }
                    else if (eqName.ToLower().Contains("snowboard"))
                    {
                        Console.WriteLine("Enter the id of equipment you want to delete");
                        _snowboardRepository.Remove(_snowboardRepository.GetById(int.Parse(Console.ReadLine())));
                        _snowboardRepository.Save();
                    }
                    else if (eqName.ToLower().Contains("shoe"))
                    {
                        Console.WriteLine("Enter the id of equipment you want to delete");
                        _shoeRepository.Remove(_shoeRepository.GetById(int.Parse(Console.ReadLine())));
                        _shoeRepository.Save();
                    }
                    else if (eqName.ToLower().Contains("helmet"))
                    {
                        Console.WriteLine("Enter the id of equipment you want to delete");
                        _helmetsRepository.Remove(_helmetsRepository.GetById(int.Parse(Console.ReadLine())));
                        _helmetsRepository.Save();
                    }
                }
                catch
                {
                    Console.WriteLine("wrong option");
                }
            }

            void OrderByName()
            {
                Console.WriteLine("OrderByName");
                foreach (var helmet in _helmetsProvider.OrderByName())
                {
                    Console.WriteLine(helmet);
                }
            }

            void GetMinimumPriceOfAllHelmets()
            {
                Console.WriteLine("GetMinimumPriceOfAllHelmets");
                Console.WriteLine(_helmetsProvider.GetMinimumPriceOfAllHelmets());
            }

            void WhereColorIsRed()
            {
                Console.WriteLine("WhereColorIs red");
                foreach (var helmet in _helmetsProvider.WhereColorIs("red"))
                {
                    Console.WriteLine(helmet);
                }
            }

            void GetUniqueHelmetColors()
            {
                Console.WriteLine("GetUniqueHelmetColors");
                foreach (var helmet in _helmetsProvider.GetUniqueHelmetColors())
                {
                    Console.WriteLine(helmet);
                }
            }

            void ShowMenu() // ten znakczek '\n' to symbol nowej linii - naucz się go, zastosuj 
            {
                Console.WriteLine("----| Welcome to Warehouse Application |----");
                Console.WriteLine("     ----------------------------------     ");
                Console.WriteLine("Warehouse Application used to rent ski equipment");
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Select what you want to do below by selecting the appropriate action number   \n");
                Console.WriteLine("Press 1 if you want to return your equipment");
                Console.WriteLine("Press 2 if you want to rent equipment");
                Console.WriteLine("Press 3 if you want read the amount of equipment in the warehouse");
                Console.WriteLine("Press 4 if you want minimum price of all helmets");
                Console.WriteLine("Press 5 if you want by name");
                Console.WriteLine("Press 6 if you want color red");
                Console.WriteLine("Press 7 if you want unique helmet colors");
                Console.WriteLine("Press q if you want quit\n");
            }

            void ActivateEventListeners()
            {
                _skiRepository.ItemAdded += EquipmentRepositoryOnItemAdded;
                _skiRepository.ItemRemove += EquipmentRepositoryOnItemRemove;

                _snowboardRepository.ItemAdded += EquipmentRepositoryOnItemAdded;
                _snowboardRepository.ItemRemove += EquipmentRepositoryOnItemRemove;

                _shoeRepository.ItemAdded += EquipmentRepositoryOnItemAdded;
                _shoeRepository.ItemRemove += EquipmentRepositoryOnItemRemove;
            }

            void AddSampleHelmetsToDb()
            {
                _helmetsRepository.ItemAdded += EquipmentRepositoryOnItemAdded;
                _helmetsRepository.ItemRemove += EquipmentRepositoryOnItemRemove;
                _helmetsRepository.Save();
            }
        }
    }
}