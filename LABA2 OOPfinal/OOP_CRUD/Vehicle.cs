using System.ComponentModel;

namespace OOP2
{
    [DisplayName("Транспорт")]
    public class Vehicle
    {
        public string modelName;
        public float weight;
        public string material;
        public int durability;
       

        public Vehicle()
        {
            modelName = "undefined";
            weight = 0;
            material = "test";
            durability = 0;
            
        }

        public override string ToString()
        {
            return modelName;
            //return base.ToString();
        }
    }

    [DisplayName("Топливо")]
    public class Fuel
    {
        public string name;
        public float octane;

        public Fuel()
        {
            name = "undefined";
            octane = 0;
        }

        public override string ToString()
        {
            return name;
        }
    }

    [DisplayName("Бензин")]
    public class Gasoline : Fuel
    {
        public int octaneNum;
        public int Price;

        public Gasoline()
        {
            octaneNum = 92;
            Price = 2;
        }
    }

    [DisplayName("Дизель")]
    public class Diesel : Fuel
    {
        public float density;
        public int Price;

        public Diesel()
        {
            density = 0;
            Price = 0;
        }

        public override string ToString()
        {
            return density.ToString();
        }
    }

    [DisplayName("Механические ТС")]
    public class MotorVehicle : Vehicle
    {
        public int power;
        [Description("Aggregation")]
        public Fuel fuel = null;
        public int tanksize;
        public int maxspeed;
        public float acceleration;

        public MotorVehicle()
        {
            power = 0;
            tanksize = 0;
            maxspeed = 0;
            acceleration = 0;
        }
    }

    public enum PurposeType {None =  0, Passenger = 1, Cargo, Commercial, Public};

    [DisplayName("ТС приводимое мускульной силой")]
    public class MechanicVehicle : Vehicle
    {
        public int Weight;
        public int length;
        public PurposeType purposeType;

        public MechanicVehicle()
        {
            Weight = 0;
            length = 0;
            purposeType = PurposeType.None;
        }
    }

    [DisplayName("Лесопедик")]
    public class Bicycle : MechanicVehicle
    {
        public int wheelsize;
        public int framestiffness;
        public int countofgears;

        public Bicycle()
        {
            wheelsize = 0;
            framestiffness = 0;
            countofgears = 0;
        }
    }

    [DisplayName("Легковые автомобили")]
    public class LightVehicle : MotorVehicle
    {
        public int RangeReserve;
        public bool isAutomatic;
        public float enginecapasity;
        public int yearsOld;

        public LightVehicle()
        {
            RangeReserve = 0;
            isAutomatic = false;
            enginecapasity = 0;
            yearsOld = 0;
        }

    }

   /* public enum AimType { None = 0, Laser, Collimator, Optic, Holographic };

    [DisplayName("Прицел")]
    public class Gunsight
    {
        public AimType aimType;
        public int zoom;

        public Gunsight()
        {
            aimType = AimType.None;
            zoom = 0;
        }

        public override string ToString()
        {
            return aimType.ToString() + " Gunsight";
        }
    }
    */
    [DisplayName("Пассажирский автомобиль")]
    public class PassengerCar : LightVehicle
    {

     
        public bool OnTheRun;
        [Description("Aggregation")]
      //  public Gunsight gunsight = null;

        public PassengerCar()
        {
            OnTheRun = true;
        }
    }

    public enum EngineType{Carburator = 0,Injector = 1, Turbo, Atmo};

    [DisplayName("Тяжеловесные ТС")]
    public class HeavyVehicle : MotorVehicle
    {
        public EngineType enginetype;

        public HeavyVehicle()
        {
            enginetype = 0;
        }
    }

    [DisplayName("Грузовик")]
    public class Truck : HeavyVehicle
    {
        public string name;
        public int Lenght;

        public Truck()
        {
            name = " ";
            Lenght = 0;
        }
    }

    [DisplayName("Автобус")]
    public class Bus : HeavyVehicle
    {
        public int countofpassenger;
        [Description("Aggregation")]
      //  public Gunsight gunsight = null;
        public string triggerType;

        public Bus()
        {
            countofpassenger = 1;
            triggerType = " ";
        }
    }

}

