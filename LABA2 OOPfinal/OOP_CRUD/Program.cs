using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<Type> types = new List<Type>(){
                typeof(PassengerCar),
                typeof(Bus),
                typeof(Truck),
                typeof(Bicycle),                
                typeof(Diesel),
                typeof(Gasoline),
            }; 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CRUDForm(types, new CRUDHelper()));
        }
    }
}
