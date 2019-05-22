using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2
{
    [DisplayName("Транспорт")]
    [Serializable]
    public class Vehicle
    {
        public string ModelName { get; set;}
        public float Weight { get; set; }
        public string Material { get; set; }
        public int Durability { get; set; }
       

        public Vehicle()
        {
            ModelName = "undefined";
            Weight = 0;
            Material = "test";
            Durability = 0;
           
        }

        public override string ToString()
        {
            return ModelName;
            //return base.ToString();
        }
    }

    [DisplayName("Топливо")]
    [Serializable]
    public class Fuel
    {
        public float Octane { get; set; }
        public string name;

        public Fuel()
        {
            name = "undefined";
            Octane = 0;
        }

        public override string ToString()
        {
            return name;
        }
    }

    [DisplayName("Бензин")]
    [Serializable]
    public class Gasoline : Fuel
    {
        public string Name { get; set; }
        public int OctaneNum { get; set; }
        public int Price { get; set; }

        public Gasoline()
        {
            Name = "undefined";
            OctaneNum = 0;
            Price = 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [DisplayName("Дизель")]
    [Serializable]
    public class Diesel : Fuel
    {
        public float Octanenum { get; set; }
        public int Price { get; set; }

        public Diesel()
        {
            Octanenum = 0;
            Price = 0;
        }

        public override string ToString()
        {
            return Octanenum.ToString();
        }
    }

    [DisplayName("Механические ТС")]
    [Serializable]
    public class MotorVehicle : Vehicle
    {
        public int power { get; set; }
        [Description("Aggregation")]
        public Fuel fuel { get; set; }
        public int tankSize { get; set; }
        public int Maxspeed { get; set; }
        public int acceleration { get; set; }

        public MotorVehicle()
        {
            power = 0;
            tankSize = 0;
            Maxspeed = 0;
            acceleration = 0;
        }
    }

    public enum PurposeType { None = 0, Passenger = 1, Cargo, Commercial, Public };

    [DisplayName("ТС приводимое мускульной силой")]
    [Serializable]
    public class MechanicVehicle : Vehicle
    {
        public int weight { get; set; }
        public int length { get; set; }
        public PurposeType Purposetype { get; set; }

        public MechanicVehicle()
        {
            weight = 0;
            length = 0;
            Purposetype = PurposeType.None;
        }
    }

    [DisplayName("Велосипед")]
    [Serializable]
    public class Bicycle : MechanicVehicle
    {
        public int wheelsize { get; set; }
        public int frameStiffness { get; set; }
        public int Gears { get; set; }

        public Bicycle()
        {
            wheelsize = 0;
            frameStiffness = 0;
            Gears = 0;
        }
    }

    [DisplayName("Легковые автомобили")]
    [Serializable]
    public class LightVehicle : MotorVehicle
    {
        public int RangeReserve { get; set; }
        public bool IsAutomatic { get; set; }
        public float EngineCapasity { get; set; }
        public int Old { get; set; }

        public LightVehicle()
        {
            RangeReserve = 0;
            IsAutomatic = false;
            EngineCapasity = 0;
            Old = 0;
        }

    }





    [DisplayName("Пассажирный автомобиль")]
    [Serializable]
    public class PassengerCar : LightVehicle
    {       
        public bool OnTheRun { get; set; }
        [Description("Aggregation")]
       
        public PassengerCar()
        {                     
            OnTheRun = false;
        }
    }

    public enum EngineType { Carburator = 0, Injector = 1, Turbo, Atmo };

    [DisplayName("Тяжеловесные ТС")]
    [Serializable]
    public class HeavyVehicle : MotorVehicle
    {
        public EngineType enginetype { get; set; }

        public HeavyVehicle()
        {
            enginetype = 0;
        }
    }

    [DisplayName("Грузовик")]
    [Serializable]
    public class Truck : HeavyVehicle
    {
        public string Name { get; set; }
        public int Length { get; set; }

        public Truck()
        {
            Name = " ";
            Length = 0;
        }
    }

    [DisplayName("Автобус")]
    [Serializable]
    public class Bus : HeavyVehicle
    {
        public string Class { get; set; }
        [Description("Class")]
       
        public string GearBox { get; set; }

        public Bus()
        {
           
            Class = " ";
            GearBox = " ";
        }
    }

}

