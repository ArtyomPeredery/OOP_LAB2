using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP2
{
    public interface IInitializator
    {
        void ItemsInit(List<Object> items);
        void SaveControlsToItems(Object item, List<Object> items, Form form);
        Form CreateForm(Object item, List<Object> items);
        void ListRedraw(Object sender, List<Object> items);
        void DeleteItem(Object item, List<Object> items);
    }

    public class Initializator: IInitializator
    {
        public void ItemsInit(List<Object> items)
        {
            items.AddRange(new List<object>
            {
                new PassengerCar()
                {ModelName = "BMW",Weight = 5,OnTheRun = true,RangeReserve = 30,EngineCapasity = 2.9f , Old = 1, power = 500 , Maxspeed = 120 },
                new Bus()
                {ModelName = "Mercedes", Maxspeed = 120 , power = 300 , Weight = 1232.3f ,  enginetype = EngineType.Injector },
                new Truck()
                {ModelName = "Scania", enginetype = EngineType.Turbo , Weight = 10000 , power = 480 , Material = "steel" , Length = 50 , Maxspeed = 2 , tankSize = 10},
                new Bicycle()
                {ModelName = "АИСТ", Material = "aluminium" , Weight = 0.5f, Gears = 20 , Purposetype = PurposeType.Public, weight = 10, wheelsize = 1 , length = 1 },                              
                new Diesel()
                { Octanenum = 7.62f, Price = 1, Octane = 0.05f },
                new Gasoline()
                {Name = "АИ-98" , Price = 4, Octane = 9.81f , OctaneNum = 98}
            }
            );
        }

        private void TrySetValue(PropertyInfo propertyInfo, object item, object value)
        {
            var buf = propertyInfo.GetValue(item);
            try
            {
                propertyInfo.SetValue(item, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            catch
            {
                propertyInfo.SetValue(item, buf);
                MessageBox.Show(propertyInfo.Name + ": Incorrect field value");
            }
        }

        public void SaveControlsToItems(Object item, List<Object> items, Form form)
        {
            if ((form == null) || (item == null) || (items == null))
                return;

            PropertyInfo[] fields = item.GetType().GetProperties();
            //из текста в значение
            foreach (var control in form.Controls.OfType<TextBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    PropertyInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    TrySetValue(fi, item, control.Text);
                }
            }

            //из текста в значение
            foreach (var control in form.Controls.OfType<CheckBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    PropertyInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    TrySetValue(fi, item, control.Checked);
                }
            }

            //Из выпадающих списков в enum
            foreach (var control in form.Controls.OfType<ComboBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    PropertyInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(item);

                    if (control.SelectedIndex == -1)
                        continue;

                    if (fi.PropertyType.IsEnum)
                    {
                        try
                        {
                            fi.SetValue(item, control.SelectedIndex);
                        }
                        catch
                        {
                            fi.SetValue(item, buf);
                            MessageBox.Show(fi.Name + ": Incorrect field value");
                        }
                    }
                    else
                    {
                        List<object> suitableItems = items.Where(sitem => ((sitem.GetType() == fi.PropertyType) || (sitem.GetType().BaseType == fi.PropertyType))).ToList();
                        try
                        {
                            fi.SetValue(item, suitableItems[control.SelectedIndex]);
                        }
                        catch
                        {
                            fi.SetValue(item, buf);
                            MessageBox.Show(fi.Name + ": Incorrect field value");
                        }
                    }
                }
            }
        }

        private Label CreateLabel(string name, Point point)
        {
            Label label = new Label
            {
                Location = point,
                Text = name,
                Width = name.Length * 7
            };
            return label;
        }

        private CheckBox CreateCheckBox(string name, Point point, bool value)
        {

            CheckBox checkBox = new CheckBox
            {
                Name = name,
                Text = "Yes",
                Location = point,
                Checked = value
            };
            return checkBox;
        }

        private TextBox CreateTextBox(string name,Point point, int width,string value)
        {
            TextBox textBox = new TextBox
            {
                Name = name,
                Location = point,
                Width = width,
                Text = value
            };
            return textBox;
        }

        private ComboBox CreateComboBox(string name, Point point, int width, string[] values, int currentValue)
        {
            ComboBox combobox = new ComboBox();
            combobox.Name = name;
            combobox.SelectionStart = 0;
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.Location = point;
            combobox.Width = width;
            combobox.Items.AddRange(values);
            combobox.SelectedIndex = currentValue;
            return combobox;
        }

        private ComboBox CreateComboBox(string name, Point point, int width,Type itemType ,Object currentItem , List<Object> allItems)
        {

            ComboBox combobox = new ComboBox();
            combobox.Name = name;
            combobox.SelectionStart = 0;
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.Location = point;
            combobox.Width = width;

            //список объектов удовлетворяющих типу поля
            List<object> suitableItems = allItems.Where(s_item => ((s_item.GetType() == itemType) || (s_item.GetType().BaseType == itemType))).ToList();

            for (int j = 0; j < suitableItems.Count; j++)
            {
                combobox.Items.Add(suitableItems[j].ToString());
            }

            int index = -1;
            if (currentItem != null)
            {
                for (int j = 0; j < suitableItems.Count; j++)
                {
                    if (currentItem.Equals(suitableItems[j]))
                    {
                        index = j;
                        break;
                    }
                }
                combobox.SelectedIndex = index;
            }
            combobox.SelectedIndex = index;
            return combobox;
        }

        public Form CreateForm(Object item, List<Object> items)
        {
            //список всех полей объекта
            PropertyInfo[] fields = item.GetType().GetProperties();
            const int controlHeight = 25;
            const int formWidth = 350;


            //создание формы для редактирования полей
            Form form = new Form
            {
                Text = item.GetType().ToString(),
                Size = new Size(formWidth, 60 + controlHeight * (fields.Length + 2)),
                StartPosition = FormStartPosition.CenterScreen
            };

            for (int i = 0; i < fields.Length; i++)
            {

                Label label = CreateLabel(string.Concat(fields[i].PropertyType.Name, " ", fields[i].Name),
                                            new Point(15, controlHeight * (i + 1)));
                form.Controls.Add(label);

                Type fieldType = fields[i].PropertyType;
                //Создание для типов значений текстовых полей ввода и их заполнение
                if (((fieldType.IsPrimitive) && (!fieldType.IsEnum))  ||  (fieldType == typeof(string)) )
                {
                    if (fieldType == typeof(bool))
                    {
                        CheckBox checkBox = CreateCheckBox(fields[i].Name,
                                                            new Point(15 + label.Width,
                                                            controlHeight * (i + 1)),
                                                            (bool)fields[i].GetValue(item));
                        form.Controls.Add(checkBox);
                    }
                    else
                    {
                        TextBox text = CreateTextBox(fields[i].Name, 
                                                       new Point(15 + label.Width, controlHeight * (i + 1)),
                                                       form.Width - (label.Location.X + label.Width + 30),
                                                       fields[i].GetValue(item).ToString());
                        form.Controls.Add(text);
                    }

                }//Создание выпадающих списков для перечислимых типов
                else if (fields[i].PropertyType.IsEnum)
                {
                    ComboBox combobox = CreateComboBox(fields[i].Name,
                                                        new Point(15 + label.Width, controlHeight * (i + 1)),
                                                        form.Width - (label.Location.X + label.Width + 30),
                                                        fields[i].PropertyType.GetEnumNames(),
                                                        (int)(fields[i].GetValue(item)));
                    form.Controls.Add(combobox);

                }//Создание выпадающих списков для вложенных членов
                else if ((fields[i].PropertyType.IsClass))
                {
                    ComboBox combobox = CreateComboBox(fields[i].Name,
                                                       new Point(15 + label.Width, controlHeight * (i + 1)),
                                                       form.Width - (label.Location.X + label.Width + 30),
                                                       fields[i].PropertyType,
                                                       fields[i].GetValue(item),
                                                       items);
                    form.Controls.Add(combobox);
                }
            }

            //кнопка сохранения
            Button btn = new Button
            {
                Text = "Save",
                Location = new Point(form.Width / 2 - (form.Width / 8), (fields.Length + 1) * controlHeight),
                Width = form.Width / 4,
                DialogResult = DialogResult.OK,
            };

            EventHandler eventForSave = (object sender, EventArgs e) =>
            {
                Button button = (Button)sender;
                Form eventForm = button.FindForm();

                if ((item != null) && (eventForm != null))
                    SaveControlsToItems(item, items, eventForm);
            };

            btn.Click += eventForSave;
            form.Controls.Add(btn);

            return form;
        }

        public void ListRedraw(Object sender, List<Object> items )
        {
            ListView listView = (ListView)sender;
            listView.Clear();
            listView.Columns.Add("Type",145);
            listView.Columns.Add("Name", 145);

            for (int i = 0; i < items.Count; i++)
            {
                Type itemType = items[i].GetType();
                object name = items[i].ToString();

                string typeString = itemType.Name; 
                if (itemType.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                {
                    typeString = displayNameAttribute.DisplayName;
                }

                var listItem = new ListViewItem(new string[]{ typeString , name.ToString() });
                listView.Items.Add(listItem);
            }
        }

        public void DeleteItem(Object item, List<Object> items)
        {
            //список объектов из общего списка
            //у которых среди полей
            //есть поле с типом удаляемого объекта или его родительским типом
            var ownerList = items.Where(itm => (itm.GetType()
            .GetProperties()
            .Where(fld => ((fld.PropertyType == item.GetType() || fld.PropertyType.BaseType == item.GetType())))).ToList().Count > 0).ToList();

            foreach (var owner in ownerList)
            {
                foreach (var fld in owner.GetType().GetProperties().Where(fld => (fld.PropertyType == item.GetType())).ToList())
                {
                    if ((fld.GetValue(owner) != null) && (fld.GetValue(owner).Equals(item)))
                    {
                        fld.SetValue(owner, null);
                    }
                }
            }
            items.Remove(item);
        }
    }
}
