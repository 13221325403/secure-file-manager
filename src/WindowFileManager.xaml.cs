using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SecureFileManager.Controller;
using SecureFileManager.Model;
using System.Windows.Media;
using System.Data;

namespace SecureFileManager
{
    /// <summary>
    /// files and directory manager
    /// </summary>
    public partial class WindowFileManager : Window
    {
        string appDataTemp = IO.Path.GetTempPath() + @"fsTmp\";

        List<Directory> directories;
        DataTable files;
        File _selectedFile = null;
        Directory _selectedDirectory = null;

        public WindowFileManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// load directories list
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = DBConnector.File;
            clear_file_preview();

            //impostazione menuitem
            if (DBConnector.isPasswordSet == false)
                mi_rimuovi_password.IsEnabled = false;
            else
                mi_rimuovi_password.IsEnabled = true;

            //caricamento elenco cartelle
            load_directories();
        }

        /// <summary>
        /// check temp folder
        /// </summary>
        void check_temp_folder()
        {
            if (!IO.Directory.Exists(appDataTemp))
                IO.Directory.CreateDirectory(appDataTemp);
        }

        /// <summary>
        /// load directories to listbox
        /// </summary>
        void load_directories()
        {
            _selectedFile = null;
            _selectedDirectory = null;
            clear_file_preview();

            //get directories list
            directories = DirectoryController.GetDirectories();

            //bind directories to combobox
            cmb_fk_cartella.ItemsSource = null;
            cmb_fk_cartella.ItemsSource = directories;
            cmb_fk_cartella.DisplayMemberPath = "name";

            //add special folder
            if(mi_file_no_dir.IsChecked == true)
            {
                directories.Add(new Directory()
                {
                    pk = -1,
                    name = "files without directory",
                    description = ""
                });
            }

            //populate listbox with custom stackpanel
            lbxDir.Items.Clear();

            SolidColorBrush lblBrush;
            StackPanel sp;
            string header1;
            string header2;
            foreach (Directory c in directories)
            {
                header1 = c.name;
                header2 = (string.IsNullOrEmpty(c.description)) ? "no description" : c.description;
                sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                lblBrush = (string.IsNullOrEmpty(c.color))
                    ? Brushes.Black
                    : (SolidColorBrush)(new BrushConverter().ConvertFrom(c.color));
                sp.Children.Add(new Label() { Content = header1, FontWeight = FontWeights.Bold, Padding = new Thickness(0), Foreground = lblBrush });
                sp.Children.Add(new Label() { Content = header2, Padding = new Thickness(0), Margin = new Thickness(0, 0, 0, 3) });
                lbxDir.Items.Add(sp);
            }
        }

        /// <summary>
        /// load files to datagrid
        /// </summary>
        void load_files()
        {
            if (_selectedDirectory is null)
                return;

            clear_file_preview();
            _selectedFile = null;
            files = FileController.GetDirectoryFilesDT(_selectedDirectory.pk);
            dgFiles.ItemsSource = null;
            dgFiles.ItemsSource = files.DefaultView;
        }

        /// <summary>
        /// write selected file to temp folder for preview
        /// </summary>
        string write_temp_file(File f)
        {
            string tmp_file = appDataTemp + f.pk + f.ext;
            if(!IO.File.Exists(tmp_file))
                IO.File.WriteAllBytes(tmp_file, f.file);
            return tmp_file; 
        }

        /// <summary>
        /// show file info (grid)
        /// </summary>
        void display_file_preview(File f)
        {
            check_temp_folder();

            gridSelectedFile.IsEnabled = true;
            bool added = false;

            //info
            grid_preview.Children.Clear();
            txt_nome.Text = f.name;
            txt_ext.Text = f.ext;
            txt_dimensioni.Text = f.length / 1000 + " kb";
            txt_data_creazione.Text = f.creation_date.Value.ToString("yyyy/MM/dd hh:mm:ss");
            txt_data_ultima_modifica.Text = f.last_edit_date.Value.ToString("yyyy/MM/dd hh:mm:ss");
            txt_dettagli.Text = f.description;
            if (f.fk_directory is null)
            {
                cmb_fk_cartella.SelectedIndex = cmb_fk_cartella.Items.Count - 1;
            }  
            else
            {
                foreach (Directory c in cmb_fk_cartella.Items)
                    if (c.pk == f.fk_directory)
                        cmb_fk_cartella.SelectedItem = c;
            }
           


            //preparazione anteprima
            if (f.file == null)
                return;

            //immagine
            if (Common.ImgExt.Contains(f.ext.ToUpper()))
            {
                BitmapImage b = new BitmapImage();
                IO.MemoryStream ms = new IO.MemoryStream(f.file);
                ms.Position = 0;
                b.BeginInit();
                b.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.UriSource = null;
                b.StreamSource = ms;
                b.EndInit();
                grid_preview.Children.Add(new Image() { Source = b});
                added = true;
            }

            //video
            if (Common.VidExt.Contains(f.ext.ToUpper()))
            {
                //smadonnare ed istanziare un player wpf
                grid_preview.Children.Add(
                    new Label()
                    {
                        Content = "video preview not available",
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    });
                added = true;
            }

            //audio
            if (Common.AudExt.Contains(f.ext.ToUpper()))
            {
                grid_preview.Children.Add(new Label() {
                    Content = "audio preview not available",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
                added = true;
            }

            //documento
            if (Common.DocExt.Contains(f.ext.ToUpper()))
            {
                grid_preview.Children.Add(new Label()
                {
                    Content = "document preview not available",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
                added = true;
            }

            //se non è nei tipi supportati tenta disperatamente il browser
            if (!added)
            {
                string file_path = write_temp_file(f);
                TextBox txt = new TextBox();
                txt.IsReadOnly = true;
                txt.BorderThickness = new Thickness(1);
                txt.Padding = new Thickness(5);
                txt.Text = IO.File.ReadAllText(file_path);
                grid_preview.Children.Add(txt);
            }
        }

        /// <summary>
        /// clear current file info (grid)
        /// </summary>
        void clear_file_preview()
        {
            gridSelectedFile.IsEnabled = false;
            grid_preview.Children.Clear();
            grid_preview.Children.Add(new Label() {
                Content = "no file selected",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            });
            foreach (object o in gridSelectedFile.Children)
            {
                if (o is TextBox)
                    (o as TextBox).Text = "";
                if (o is ComboBox)
                    (o as ComboBox).SelectedIndex = 1;
            }     
        }

        /// <summary>
        /// add to db selected files
        /// </summary>
        /// <param name="files">list of files path</param>
        void insert_files(string[] files)
        {
            if (files.Length > 0 && MessageBox.Show(files.Length + " files found, proceed?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                File tmp;
                IO.FileInfo info;
                int count = 0;
                foreach (string file in files)
                {
                    if(IO.Path.GetFullPath(file) == IO.Path.GetFullPath(DBConnector.File))
                    {
                        MessageBox.Show("Cannot add current database as file", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        tmp = new File();
                        info = new IO.FileInfo(file);
                        tmp.name = IO.Path.GetFileNameWithoutExtension(file);
                        tmp.ext = IO.Path.GetExtension(file);
                        tmp.length = info.Length;
                        tmp.file = IO.File.ReadAllBytes(file);
                        tmp.creation_date = info.CreationTime;
                        tmp.last_edit_date = DateTime.Now;
                        tmp.fk_directory = _selectedDirectory.pk;
                        FileController.insert(tmp);
                        count++;
                    }       
                }
                MessageBox.Show(count + " files added", "alert", MessageBoxButton.OK, MessageBoxImage.Information);
                load_files();
            }          
        }

        /// <summary>
        /// open file dialog and load files
        /// </summary>
        private void add_file(object sender, RoutedEventArgs e)
        {
            if (_selectedDirectory is null)
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
                insert_files(ofd.FileNames);
        }

        /// <summary>
        /// delete selected files from datagrid
        /// </summary>
        private void delete_file(object sender, RoutedEventArgs e)
        {
            if (dgFiles.SelectedIndex == -1)
                return;

            List<File> ToDelete = new List<File>();
            File tmp;
            foreach (DataRowView dr in dgFiles.SelectedItems)
            {
                tmp = FileController.select(Convert.ToInt16(dr["pk_file"]));
                ToDelete.Add(tmp);
            }

            if (MessageBox.Show("delete " + ToDelete.Count + " files permanently?", "warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                foreach (File f in ToDelete)
                    FileController.delete(f.pk);
                load_files();
            }
        }

        /// <summary>
        /// write back selected files
        /// </summary>
        private void export_file(object sender, RoutedEventArgs e)
        {
            if (dgFiles.SelectedIndex == -1)
                return;

            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.ShowNewFolderButton = true;
                if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string directoryDest = fbd.SelectedPath;
                    int count = 0;

                    File f;
                    foreach (DataRowView dr in dgFiles.SelectedItems)
                    {
                        f = FileController.select(Convert.ToInt16(dr["pk_file"]));
                        if (f.file != null)
                        {
                            try
                            {
                                IO.File.WriteAllBytes(directoryDest + "\\" + f.name + f.ext, f.file);
                                count++;
                            }
                            catch { }
                        }
                    }
                        
                    MessageBox.Show(count + " files exported", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }  
            }
        }

        /// <summary>
        /// mostra i dettagli del file ed un anteprima
        /// </summary>
        private void dgFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFiles.SelectedIndex == -1)
                return;

            txbFilesCount.Text = string.Format("{0} of {1} files selected", dgFiles.SelectedItems.Count, dgFiles.Items.Count);
            int pk = Convert.ToInt16(files.Rows[dgFiles.SelectedIndex]["pk_file"]);
            _selectedFile = FileController.select(pk);
            display_file_preview(_selectedFile);
        }

        /// <summary>
        /// delete selected directory
        /// </summary>
        private void delete_directory(object sender, RoutedEventArgs e)
        {
            if (_selectedDirectory is null || _selectedDirectory.pk < 0)
                return;

            if (MessageBox.Show("delete directory?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                if (DirectoryController.Delete(_selectedDirectory.pk) > 0)
                    load_directories();
        }

        /// <summary>
        /// write back all directory's files
        /// </summary>
        private void export_directory(object sender, RoutedEventArgs e)
        {
            if (_selectedDirectory is null)
                return;

            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string directoryDest = fbd.SelectedPath;
                    int count = 0;
                    int err = 0;
                    foreach (File f in FileController.GetDirectoryFiles(_selectedDirectory.pk))
                        if (f.file != null)
                        {
                            try
                            {
                                IO.File.WriteAllBytes(directoryDest + "\\" + f.name + f.ext, f.file);
                                count++;
                            }
                            catch { err++; }
                        }
                    MessageBox.Show(
                        string.Format("'{0}' exported:\n - {1} files successfully processed \n - {2} errors occurred", _selectedDirectory.name, count, err),
                        "alert", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information
                    );
                }
            }
        }

        /// <summary>
        /// carica i file della cartella selezionata
        /// </summary>
        private void lbxDir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxDir.SelectedIndex == -1)
                return;

            _selectedDirectory = directories[lbxDir.SelectedIndex];
            load_files();
            txbFilesCount.Text = string.Format("{0} of {1} files selected", dgFiles.SelectedItems.Count, dgFiles.Items.Count);
        }

        /// <summary>
        /// close current window and show windowOpen
        /// </summary>
        private void open_database(object sender, RoutedEventArgs e)
        {
            new WindowOpen().Show();
            this.Close();
        }

        /// <summary>
        /// open inputbox for file filtering
        /// </summary>
        private void search_file(object sender, RoutedEventArgs e)
        {
            if (dgFiles.Items.Count == 0)
                return;

            string pattern = Interaction.InputBox("insert a pattern", "search into files");
            if (pattern != "")
            {
                //files.AsEnumerable().Where(f =>
                //    f["name"].ToString().ToUpper().Contains(pattern.ToUpper()) ||
                //    f["ext"].ToString().ToUpper().Contains(pattern.ToUpper()) ||
                //    f["description"].ToString().ToUpper().Contains(pattern.ToUpper())
                //);
                dgFiles.ItemsSource = null;
                files.DefaultView.RowFilter = string.Format("name LIKE '%{0}%' OR ext LIKE '%{0}%' OR description LIKE '%{0}%'", pattern);
                dgFiles.ItemsSource = files.DefaultView;
            }
        }

        /// <summary>
        /// update selected file
        /// </summary>
        private void save_file(object sender, RoutedEventArgs e)
        {
            if (dgFiles.SelectedIndex == -1 || _selectedFile is null)
                return;

            _selectedFile.name = txt_nome.Text;
            _selectedFile.description = txt_dettagli.Text;
            _selectedFile.fk_directory = ((Directory)cmb_fk_cartella.SelectedItem).pk;

            if (FileController.update(_selectedFile) > 0)
            {
                MessageBox.Show("file successfully edited", "",MessageBoxButton.OK,MessageBoxImage.Information);
                load_files();
            }
        }

        /// <summary>
        /// open selected file
        /// </summary>
        private void open_file(object sender, RoutedEventArgs e)
        {
            if (dgFiles.SelectedIndex == -1 || _selectedFile is null)
                return;

            System.Diagnostics.Process.Start(write_temp_file(_selectedFile));
        }

        /// <summary>
        /// delete temp folder
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IO.Directory.Exists(appDataTemp))
                IO.Directory.Delete(appDataTemp, true);
        }

        /// <summary>
        /// remove protection password
        /// </summary>
        private void remove_password(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("remove passoword?","confirm",MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DBConnector.ChangePassword("");
                mi_rimuovi_password.IsEnabled = false;
                MessageBox.Show("Password successfully removed", "alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }     
        }

        /// <summary>
        /// edit current password
        /// </summary>
        private void set_password(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("edit database password?", "confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string new_pw = Interaction.InputBox("type new password", "security");
                if (new_pw != "")
                {
                    DBConnector.ChangePassword(new_pw);
                    MessageBox.Show("password successfully updated", "alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
                
        }

        /// <summary>
        /// show/hide orphans file directory on listbox
        /// </summary>
        private void mostra_file_senza_cartella(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            load_directories();
        }

        /// <summary>
        /// open WindowDirectory on update mode
        /// </summary>
        private void edit_directory(object sender, RoutedEventArgs e)
        {
            if (_selectedDirectory is null || _selectedDirectory.pk < 0)
                return;

            WindowDirectory de = new WindowDirectory(_selectedDirectory.pk);
            if(de.ShowDialog() == true)
            {
                load_directories();
            }
        }

        /// <summary>
        /// open WindowDirectory on insert mode
        /// </summary>
        private void add_directory(object sender, RoutedEventArgs e)
        {
            WindowDirectory de = new WindowDirectory();
            if (de.ShowDialog() == true)
                load_directories();
        }

        private void dgFiles_Drop(object sender, DragEventArgs e)
        {
            if (_selectedDirectory is null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                insert_files(files);
            }
        }

        private void lbxDir_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            edit_directory(sender, e);
        }


        /*
        
       basic tutorial for recursive tree directory structure

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //find all fathers, all directories without any parent (no fk_parent)
            tree = CartellaController.GetChild(-1);

            foreach (Directory f in tree)
            {
                loadChild(f);
            }

            MessageBox.Show("fine");
        }

        private void loadChild(Directory c)
        {
            var figli = CartellaController.GetChild(c.pk_cartella);
            if (figli.Count == 0)
                return;

            c.childs.AddRange(figli);
            foreach (Cartella cf in c.childs)
            {
                caricaFigli(cf);
            }
        }
        */
    }
}