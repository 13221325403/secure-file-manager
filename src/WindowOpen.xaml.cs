using Microsoft.Win32;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SecureFileManager
{
    /// <summary>
    /// create or open a db file
    /// </summary>
    public partial class WindowOpen : Window
    {
        NumberFormatInfo nfi = new CultureInfo("it-IT", false).NumberFormat;

        public WindowOpen()
        {
            InitializeComponent();
        }

        void checkFile()
        {
            if (rdbOpen.IsChecked == true)
            {
                if (System.IO.File.Exists(txtFilePath.Text))
                {
                    nfi.NumberGroupSeparator = ".";
                    System.IO.FileInfo fi = new System.IO.FileInfo(txtFilePath.Text);
                    lblLength.Content = (fi.Length / 1024).ToString("N0", CultureInfo.GetCultureInfo("it")) + " KB";
                    lblCreationDate.Content = fi.CreationTime.ToString("yyyy/MM/dd hh:mm:ss");
                    lblLastEdit.Content = fi.LastAccessTime.ToString("yyyy/MM/dd hh:mm:ss");
                    lblError.Content = "";
                    btnOpen.IsEnabled = true;
                    btnDeleteFile.IsEnabled = true;
                }
                else
                {
                    lblLength.Content = "---";
                    lblCreationDate.Content = "---";
                    lblLastEdit.Content = "---";
                    lblError.Content = "specified file doesn't exists";
                    btnOpen.IsEnabled = false;
                    btnDeleteFile.IsEnabled = false;
                }
            }

            //handling creazione
            if (rdbCreate.IsChecked == true)
            {
                lblLength.Content = "---";
                lblCreationDate.Content = "---";
                lblLastEdit.Content = "---";

                if (System.IO.File.Exists(txtFilePath.Text))
                {
                    lblError.Content = "specified file already exists";
                    btnOpen.IsEnabled = false;
                    btnDeleteFile.IsEnabled = false;
                }
                else
                {
                    lblError.Content = "";
                    btnOpen.IsEnabled = true;
                    btnDeleteFile.IsEnabled = true;
                }

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFilePath.Text = Common.GetSession();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == "")
                return;

            bool res = false;
            if (rdbOpen.IsChecked == true)
            {
                res = DBConnector.Open(FileMode.Open, txtFilePath.Text, pwbPasswd.Password);
            }
            if (rdbCreate.IsChecked == true)
            {
                res = DBConnector.Open(FileMode.Create, txtFilePath.Text, pwbPasswd.Password);
            }
            if (res)
            {
                Common.SetSession(txtFilePath.Text);
                new WindowFileManager().Show();
                this.Close();
            }
        }

        private void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
            FileDialog fd;
            if (rdbOpen.IsChecked == true)
                fd = new OpenFileDialog();
            else
                fd = new SaveFileDialog();

            fd.Filter = "database sqlite (*.db;*.sqlite;*.sqlite3)|*.db;*.sqlite;*.sqlite3";
            if (fd.ShowDialog() == true)
            {
                txtFilePath.Text = fd.FileName;
            }
        }

        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            checkFile();
            
        }

        private void switchRdbMode_Click(object sender, RoutedEventArgs e)
        {
            checkFile();
        }

        private void btnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("delete selected database?", "warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    System.IO.File.Delete(txtFilePath.Text);
                    MessageBox.Show("File deleted", "alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    checkFile();
                }

            }
        }
    }
}
