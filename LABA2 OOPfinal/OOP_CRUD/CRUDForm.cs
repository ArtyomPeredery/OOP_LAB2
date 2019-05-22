using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Plugin;

namespace OOP2
{
    public partial class CRUDForm : Form
    {
        private List<ISerializer> _serializers = new List<ISerializer>()
        {
            new BinarySerializer(),
            new JSONSerializer(),
            new TextSerializer()
        };

        public IInitializator _CRUDAssistant = null;
        public Form _editForm = null;
        public List<object> _itemList = new List<object>();
        public List<object> _activeItemList = new List<object>();
        public List<Type> _itemCreator = new List<Type>();
        private string OpenDialogfilter;
        private string SaveDialogfilter;
        private List<IPlugin> Plugins;
        
        public CRUDForm(List<Type> availibleTypes, IInitializator CRUDHelper)
        {
            InitializeComponent();
            _CRUDAssistant = CRUDHelper;
            //CRUDAssistant.ItemsInit(itemList);
            _activeItemList = _itemList;
            _itemCreator = availibleTypes;
        }

        private void CRUDForm_Load(object sender, EventArgs e)
        {
            OOP_CRUD.Plugin plugins = new OOP_CRUD.Plugin();
            plugins.LoadPlugins(".\\Plugins", ref Plugins);
            foreach (IPlugin plugin in Plugins)
            {
                comboBoxPlugins.Items.Add(plugin.PluginName);
            }
            SaveDialogfilter = CreatePluginFilter(Plugins);


            foreach (var item in _itemCreator)
            {
                string typeString = item.Name; 

                if (item.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxTypes.Items.Add(typeString);
            }

            comboBoxTypes.SelectedIndex = 0;
            comboBoxTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            itemsListView.View = View.Details;

            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);

            foreach (var item in _serializers)
            {
                string typeString = item.GetType().Name;

                if (item.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxChooseSerializer.Items.Add(typeString);
            }

            if(comboBoxChooseSerializer.Items.Count != 0)
                comboBoxChooseSerializer.SelectedIndex = 0;

            comboBoxChooseSerializer.DropDownStyle = ComboBoxStyle.DropDownList;           
        }

        private static string CreatePluginFilter(List<IPlugin> Serializators)
        {

            string result = null;
            foreach (IPlugin item in Serializators)
            {
                result += item.PluginExtentionName + "|";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }


        private void CreateItemEditForm(Object item, List<Object> itemList)
        {
            if (item != null)
            {
                _editForm = _CRUDAssistant.CreateForm(item, itemList);
                _editForm.ShowDialog();
                _editForm.Dispose();
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            ConstructorInfo itemConstructor = _itemCreator[comboBoxTypes.SelectedIndex].GetConstructor(new Type[] { });
            object newitem = itemConstructor.Invoke(new object[] { });
            _itemList.Add(newitem);

            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);

            CreateItemEditForm(newitem, _itemList);

            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item != null)
            {
                _CRUDAssistant.DeleteItem(item, _itemList);
                _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
                _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            object item = GetFocusItem();
            if (item != null)
            {
                CreateItemEditForm(item, _itemList);
                _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
            }
        }

        public object GetFocusItem()
        {
            object item = null;
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);

            if (itemsListView.SelectedIndices.Count != 0)
                item = _activeItemList[itemsListView.SelectedIndices[0]];

            return item;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
            _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
        }

        public  List<object> GetActiveList(List<object> items, bool specificClass, Type classType)
        {
            return (specificClass && (classType != null)) 
                ?  items.Where(item => (item.GetType() == classType)).ToList() 
                :  items;
        }

        private string ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\Artem\Desktop\LAB3OOP";
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }

        private ISerializer ChooseSerializer(string ext)
        {
            foreach (var serializer in _serializers)
            {
                if (ext == serializer.FileExtension)
                    return serializer;
            }
            return null;
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog();
            saveFileDialog.ShowDialog();
            saveFileDialog.Filter = SaveDialogfilter;
            
            ISerializer serializer = _serializers[comboBoxChooseSerializer.SelectedIndex]; 
            if (saveFileDialog.FileName.Length != 0)
            {                            
                if (serializer.FileExtension != Path.GetExtension(saveFileDialog.FileName))
                {
                    saveFileDialog.FileName += serializer.FileExtension;
                    serializer.Serialize(_itemList, saveFileDialog.FileName);
                    _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
                    _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
                }                
                Plugins[comboBoxPlugins.SelectedIndex].SaveFile(saveFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("Файл не выбрал ,алё!");
                return;
            }                      
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            openFileDialog.Filter = SaveDialogfilter;

            ISerializer serializer;
            if (openFileDialog.FileName.Length != 0)
            {
                string ext = Path.GetExtension(openFileDialog.FileName);
                string temp = openFileDialog.FileName.Replace(ext, "");
                int index = Plugins.FindIndex(x => x.PluginExtention == ext);
                Plugins[index].OpenFile(openFileDialog.FileName, temp);
                ext = Path.GetExtension(temp);
                int serialIndex = _serializers.FindIndex(x => x.FileExtension == ext);
                serializer = _serializers[serialIndex];               
                if (serializer == null)
                { 
                    MessageBox.Show("кривоватый путь к файлу");
                    return;
                }
                _itemList = (List<Object>)serializer.Deserialize(temp);
                _activeItemList = GetActiveList(_itemList, checkBoxFilter.Checked, _itemCreator[comboBoxTypes.SelectedIndex]);
                _CRUDAssistant.ListRedraw(itemsListView, _activeItemList);
            }
            else
            {
                MessageBox.Show("выбери файл!");
                return;
            }          
        }

        private void comboBoxChooseSerializer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
