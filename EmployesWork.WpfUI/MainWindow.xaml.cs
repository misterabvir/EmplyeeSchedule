using System;
using System.Threading.Tasks;
using System.Windows;


namespace EmployesWork.WpfUI
{

    public partial class MainWindow : Window
    {
        private PathesKeeper pk;
        private string TxtPath => txtPath.Text; 

        public MainWindow()
        {
            InitializeComponent();
            SetNames();
            StartInit();
        }

        private void SetNames()
        {
            this.Title = CN.MAIN_TITLE;
            btnStart.Content = CN.MAIN_BTN_START;
            SetCheckBoxName();
            txtCount.Text = CN.MAIN_PATH_LABEL;
            btnAdd.Content = CN.MAIN_BTN_ADD;
            btnRemove.Content = CN.MAIN_BTN_REMOVE;
        }

        private void StartInit()
        {
            pk = new PathesKeeper();

            if (pk.ReadPathes())
            {
                LoadPathes();
            }
            dateelem.SelectedDate = DateTime.Now;
        }

        private void LoadPathes()
        {
            if (cmbPathes.Items.Count != 0)
            {
                cmbPathes.Items.Clear();
            }
            foreach (string path in pk)
            {
                cmbPathes.Items.Add(path);
            }
            cmbPathes.SelectedIndex = 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (pk != null)
            {
                pk.SavePathes();
            }
            base.OnClosed(e);
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            var creator =  new CreatorRaport( checkAdmin.IsChecked == true, 
                dateelem.SelectedDate.GetValueOrDefault(), 
                cmbPathes.SelectedValue.ToString());
            btnStart.IsEnabled = false;            
            creator.Start(btnStart);            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (pk.RemoveString(TxtPath))
            {
                MessageBox.Show($"{CN.PATH_PATHWORD}\n{ TxtPath }\n{CN.PATH_REMOVED}");
                LoadPathes();
            }
            else
            {
                MessageBox.Show($"{CN.PATH_PATHWORD}{ TxtPath }\n{CN.PATH_NOT_REMOVED}");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(TxtPath);
            if (pk.AddString(TxtPath))
            {
                MessageBox.Show($"{CN.PATH_PATHWORD}\n{TxtPath}\n{CN.PATH_ADDED}");
                LoadPathes();
            }
            else
            {
                MessageBox.Show($"{CN.PATH_PATHWORD}{TxtPath}\n{CN.PATH_NOT_ADDED}");
            }
        }

        private void cmbPathes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbPathes.SelectedValue != null)
                txtPath.Text = cmbPathes.SelectedValue.ToString();
        }

        private void checkAdmin_Checked(object sender, RoutedEventArgs e)
        {
            SetCheckBoxName();
        }

        private void SetCheckBoxName()
        {
            checkAdmin.Content = checkAdmin.IsChecked == true ? CN.MAIN_IS_FULL : CN.MAIN_IS_NOT_FULL;
        }
    }
}
