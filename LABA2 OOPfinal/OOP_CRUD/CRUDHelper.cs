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
    public interface ICRUDHelper
    {
        void ItemsInit(List<Object> items);
        void SaveControlsToItems(Object item, List<Object> items, Form form);
        Form CreateForm(Object item, List<Object> items);
        void ListRedraw(Object sender, List<Object> items);
        void DeleteItem(Object item, List<Object> items);
    }

    public class CRUDHelper: ICRUDHelper
    {
        public void ItemsInit(List<Object> items)
        {
            items.AddRange(new List<object>
            {
                new PassengerCar()
                {modelName = "Мерседес",weight = 2750,OnTheRun = true,RangeReserve = 700,enginecapasity = 3.8f , yearsOld = 1, power = 500 , maxspeed = 270 },
                new Bus()
                {modelName = "Маз", maxspeed = 90 , power = 250 , weight = 4.3f ,  enginetype = EngineType.Atmo },
                new Truck()
                {modelName = "Скания", enginetype = EngineType.Turbo , weight = 11000 , power =600 , material = "сталь" , Lenght = 24 , maxspeed = 140 , tanksize = 200},
                new Bicycle()
                {modelName = "Аист", material = "алюминий" , weight = 13.5f, countofgears = 21 , purposeType = PurposeType.Passenger, Weight = 13, wheelsize = 27 , length = 2 },
               
                new Diesel()
                { density = 34.21f, Price = 2, octane = 92.05f },
                new Gasoline()
                {name = "Евро 5 АИ-102" , Price = 4, octane = 102.02f , octaneNum = 102}
            }
            );
        }

        public void SaveControlsToItems(Object item, List<Object> items, Form form)
        {
            if ((form == null) || (item == null) || (items == null))
                return;

            FieldInfo[] fields = item.GetType().GetFields();
            //из текста в значение
            foreach (var control in form.Controls.OfType<TextBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    FieldInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(item);
                    try
                    {
                        fi.SetValue(item, Convert.ChangeType(control.Text, fi.FieldType));
                    }
                    catch
                    {
                        fi.SetValue(item, buf);
                        MessageBox.Show(fi.Name + ": Неверный формат значения поля");
                    }
                }
            }

            //из текста в значение
            foreach (var control in form.Controls.OfType<CheckBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    FieldInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(item);
                    try
                    {
                        fi.SetValue(item, control.Checked);
                    }
                    catch
                    {
                        fi.SetValue(item, buf);
                        MessageBox.Show(fi.Name + ": Неверный формат значения поля");
                    }
                }
            }

            //Из выпадающих списков в enum
            foreach (var control in form.Controls.OfType<ComboBox>().ToList())
            {
                if (fields.ToList().Where(field => field.Name == control.Name).Count() != 0)
                {
                    FieldInfo fi = fields.ToList().Where(field => field.Name == control.Name).First();
                    var buf = fi.GetValue(item);

                    if (control.SelectedIndex == -1)
                        continue;

                    if (fi.FieldType.IsEnum)
                    {
                        try
                        {
                            fi.SetValue(item, control.SelectedIndex);
                        }
                        catch
                        {
                            fi.SetValue(item, buf);
                            MessageBox.Show(fi.Name + ": Неверный формат значения поля");
                        }
                    }
                    else
                    {
                        List<object> suitableItems = items.Where(sitem => ((sitem.GetType() == fi.FieldType) || (sitem.GetType().BaseType == fi.FieldType))).ToList();
                        try
                        {
                            fi.SetValue(item, suitableItems[control.SelectedIndex]);
                        }
                        catch
                        {
                            fi.SetValue(item, buf);
                            MessageBox.Show(fi.Name + ": Неверный формат значения поля");
                        }
                    }
                }
            }
        }

        public Form CreateForm(Object item, List<Object> items)
        {
            //список всех полей объекта
            FieldInfo[] fields = item.GetType().GetFields();
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
                //надпись содержащая тип и имя поля
                Label label = new Label
                {
                    Location = new Point(15, controlHeight * (i + 1)),
                    Text = string.Concat(fields[i].FieldType.Name, " ", fields[i].Name),
                    Width = string.Concat(fields[i].FieldType.Name, " ", fields[i].Name).Length * 7,           
                };

                form.Controls.Add(label);

                Type fieldType = fields[i].FieldType;
                //Создание для типов значений текстовых полей ввода и их заполнение
                if (((fieldType.IsPrimitive) && (!fieldType.IsEnum))  ||  (fieldType == typeof(string)) )
                {
                    if (fieldType == typeof(bool))
                    {
                        CheckBox radioButton = new CheckBox
                        {
                            Name = fields[i].Name,
                            Text = "Yes",
                            Location = new Point(15 + label.Width, controlHeight * (i + 1)),
                            Checked = (bool)fields[i].GetValue(item),
                        };
                        form.Controls.Add(radioButton);
                    }
                    else
                    {
                        TextBox text = new TextBox
                        {
                            Name = fields[i].Name,
                            Location = new Point(15 + label.Width, controlHeight * (i + 1)),
                            Width = form.Width - (label.Location.X + label.Width + 30),
                            Text = fields[i].GetValue(item).ToString()
                        };
                        form.Controls.Add(text);
                    }

                }//Создание выпадающих списков для перечислимых типов
                else if (fields[i].FieldType.IsEnum)
                {
                    ComboBox combobox = new ComboBox
                    {
                        Name = fields[i].Name,
                        SelectionStart = 0,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(15 + label.Width, controlHeight * (i + 1)),
                        Width = form.Width - (label.Location.X + label.Width + 30)
                    };
                    combobox.Items.AddRange(fields[i].FieldType.GetEnumNames());
                    combobox.SelectedIndex = (int)(fields[i].GetValue(item));
                    form.Controls.Add(combobox);

                }//Создание выпадающих списков для вложенных членов
                else if ((fields[i].FieldType.IsClass))
                {
                    ComboBox combobox = new ComboBox
                    {
                        Name = fields[i].Name,
                        SelectionStart = 0,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(15 + label.Width, controlHeight * (i + 1)),
                        Width = form.Width - (label.Location.X + label.Width + 30)
                    };

                    //список объектов удовлетворяющих типу поля
                    List<object> suitableItems = items.Where(s_item => ((s_item.GetType() == fields[i].FieldType) || (s_item.GetType().BaseType == fields[i].FieldType))).ToList();

                    for (int j = 0; j < suitableItems.Count; j++)
                    {
                        combobox.Items.Add(suitableItems[j].ToString());
                    }

                    var buf = fields[i].GetValue(item);
                    int index = -1;

                    if (buf != null)
                    {
                        for (int j = 0; j < suitableItems.Count; j++)
                        {
                            if (buf.Equals(suitableItems[j]))
                            {
                                index = j; break;
                            }
                        }
                        combobox.SelectedIndex = index;
                    }

                    form.Controls.Add(combobox);
                }
            }

            //кнопка сохранения
            Button btn = new Button
            {
                Name = "btnSave",
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
            .GetFields()
            .Where(fld => ((fld.FieldType == item.GetType() || fld.FieldType.BaseType == item.GetType())))).ToList().Count > 0).ToList();

            foreach (var owner in ownerList)
            {
                foreach (var fld in owner.GetType().GetFields().Where(fld => (fld.FieldType == item.GetType())).ToList())
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
