using System.Windows;
using System.Windows.Controls;
using SecureFileManager.Controller;
using SecureFileManager.Model;

namespace SecureFileManager
{
    /// <summary>
    /// Create or edit a directory
    /// </summary>
    public partial class WindowDirectory : Window
    {
        /*
         * documentation link:
         * convert control Brush to string: https://social.msdn.microsoft.com/Forums/vstudio/en-US/47261255-2e5b-4d03-b692-9ded0cb9774f/convert-brush-to-string?forum=wpf
         * creating brush from string:      https://stackoverflow.com/questions/10062376/creating-solidcolorbrush-from-hex-color-value
         */
        
        WindowMode mode;
        Directory _dir = new Directory();

        /// <summary>
        /// insert a new directory
        /// </summary>
        public WindowDirectory()
        {
            InitializeComponent();
            mode = WindowMode.Insert;
            btnRemove.IsEnabled = false;
            this.Title = "New directory";
        }

        /// <summary>
        /// edit an existing directory
        /// </summary>
        /// <param name="directory_pk"></param>
        public WindowDirectory(int directory_pk)
        {
            InitializeComponent();
            mode = WindowMode.Update;
            _dir.pk = directory_pk;
            this.Title = "Edit directory";
        }

        /// <summary>
        /// write changes to database (insert or update)
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //assign form data to _dir object
            _dir.name = txtDirName.Text;
            _dir.description = txtDirDescr.Text;
            _dir.color = (cmbLabel.SelectedIndex > -1)
                ? ((ComboBoxItem)cmbLabel.SelectedItem).Background.ToString()
                : "";
            
            //edit data
            if (DirectoryController.InsertUpdate(_dir) > 0)
            {
                MessageBox.Show("Directory sucessfully edited", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// delete the current directory (only on update mode)
        /// </summary>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Permanently delete directory?","", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (DirectoryController.Delete(_dir.pk) > 0)
                {
                    MessageBox.Show("Directory successfully deleted", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                }
            }
        }

        /// <summary>
        /// if update mode load directory object from db
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(mode == WindowMode.Update)
            {
                _dir = DirectoryController.Select(_dir.pk);
                if(_dir != null)
                {
                    txtDirName.Text = _dir.name;
                    txtDirDescr.Text = _dir.description;

                    foreach (ComboBoxItem ci in cmbLabel.Items)
                        if (ci.Background.ToString() == _dir.color)
                            cmbLabel.SelectedItem = ci;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
        }
    }
}

/*
//ComboBoxItem cmbi = (ComboBoxItem)cmbLabel.SelectedItem;
//convert from string color
//var el = (Color)ColorConverter.ConvertFromString("Red");
MessageBox.Show(hex);
//cmbLabel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffaacc"));
 */
