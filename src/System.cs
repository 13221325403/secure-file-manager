using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace SecureFileManager
{
    public enum FileMode { Open, Create };
    public enum WindowMode { Insert, Update };

    /// <summary>
    /// static class for sqlite db connection and query execution [TODO]
    /// </summary>
    static class DBConnector
    {
        static public string File = "";
        static private string _Password = "";
        static public bool isPasswordSet = false;

        public static SQLiteConnection conn;


        /// <summary>
        /// create or open a sqlite database
        /// </summary>
        /// <param name="mode">file open mode</param>
        /// <param name="pfile">file path</param>
        /// <param name="ppassword">db password. if null or empty password will be removed</param>
        /// <returns></returns>
        public static bool Open(FileMode mode, string pfile, string ppassword)
        {
            File = pfile;
            _Password = ppassword;
            if (_Password != "")
                isPasswordSet = true;


            //CREAZIONE FILE: se indicato nel parametro 'mode' viene creato un nuovo file sqlite
            if (mode == FileMode.Create)
            {
                 _initializeDDL();
            }

            
            //APERTURA FILE
            conn = (_Password.Length > 0)
                ? new SQLiteConnection("Data Source=" + File + ";Version=3;Password=" + _Password + ";")
                : new SQLiteConnection("Data Source=" + File + ";Version=3;");


            //TEST CONNESSIONE: esecuzione query generica per verificare la funzionalità
            try
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand("PRAGMA schema_version;", conn))
                {
                    var ret = command.ExecuteScalar();
                }
            }
            catch (SQLiteException ex)
            {
                string msg = "Error";
                if (ex.ErrorCode == 26)
                    msg = "Invalid password";

                MessageBox.Show(msg, "Error " + ex.ErrorCode, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                conn.Close();
            }

            return true;
        }


        /// <summary>
        /// edits current db password
        /// </summary>
        /// <param name="new_pw">new password value. If is null or empty password will be removed</param>
        public static void ChangePassword(string new_pw)
        {
            //edit passoword
            conn.Open();
            conn.ChangePassword(new_pw);
            conn.Close();

            //reopen connection
            Open(FileMode.Open, File, new_pw);
        }


        /// <summary>
        /// return an array byte from string
        /// </summary>
        static byte[] _GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            bytes = System.Text.Encoding.Default.GetBytes(str);
            return bytes;
        }


        /// <summary>
        /// create db schema
        /// </summary>
        static bool _initializeDDL()
        {
            try
            {
                //db file creation
                SQLiteConnection.CreateFile(File);
                SQLiteConnection ctmp = new SQLiteConnection("Data Source=" + File + ";Version=3;");
                if (_Password.Length > 0)
                    ctmp.SetPassword(_GetBytes(_Password));

                //ddl
                ctmp.Open();
                string q =
                    @"BEGIN TRANSACTION;
                    CREATE TABLE IF NOT EXISTS `files` (
                        `pk_file`	INTEGER PRIMARY KEY AUTOINCREMENT,
                        `name`	TEXT NOT NULL,
                        `ext`	TEXT NOT NULL,
                        `length`	INTEGER NOT NULL,
                        `file`	BLOB,
                        `description`	TEXT,
                        `creation_date`	TEXT NOT NULL,
                        `last_edit_date`	TEXT NOT NULL,
                        `fk_directory`	INTEGER
                    );
                    CREATE TABLE IF NOT EXISTS `directories` (
                        `pk_directory`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `name`	TEXT NOT NULL,
                        `description`	TEXT,
                        `fk_parent_directory`	INTEGER,
                        `color`	TEXT
                    );
                    INSERT INTO directories ('name', 'description') VALUES('new directory', NULL);
                    COMMIT;";

                //command execution
                SQLiteCommand cmd = new SQLiteCommand(q, ctmp);
                cmd.ExecuteNonQuery();
                ctmp.Close();

                //alert
                MessageBox.Show(File + " successfully created", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error occurred during initialization: "+ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        /// <summary>
        /// Execute a specified query and return a datatable
        /// </summary>
        public static DataTable Select(string q)
        {
            DataTable dt = new DataTable();
            SQLiteCommand comm = null;
            try
            {
                conn.Open();
                comm = new SQLiteCommand(q, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(comm);
                da.Fill(dt);
            }
            catch(SQLiteException e)
            {
                MessageBox.Show("Error: " + e.Message, e.ErrorCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                comm.Dispose();
                conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// execute a specified query and returns affected rows
        /// </summary>
        public static int Query(string q)
        {
            SQLiteCommand cmd = null;
            int affected_rows = 0;
            try
            {
                conn.Open();
                cmd = new SQLiteCommand(q, conn);
                affected_rows = Convert.ToInt16(cmd.ExecuteNonQuery());
            }
            catch(SQLiteException e)
            {
                MessageBox.Show("Error: " + e.Message, e.ErrorCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return affected_rows;
        }


        /// <summary>
        /// execute a specified command and returns affected rows
        /// </summary>
        public static int Query(SQLiteCommand comm)
        {
            SQLiteCommand cmd = null;
            int affected_rows = 0;
            try
            {
                conn.Open();
                affected_rows = Convert.ToInt16(comm.ExecuteNonQuery());
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Error: " + e.Message, e.ErrorCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                comm.Dispose();
                conn.Close();
            }
            return affected_rows;
        }

    }


    public static class DBSet
    {
        public static object SetInt(int? value)
        {
            //se ha un valore assegnato ed è maggiore di -1
            if (value.HasValue && value > -1)
                return value;
            else
                return DBNull.Value;
        }
        public static object SetDouble(double? value)
        {
            if (value.HasValue && value > 0)
                return value;
            else
                return DBNull.Value;
        }
        public static object SetString(string value)
        {
            if (value != null && value.Length > 0)
                return value;
            else
                return DBNull.Value;
        }
        public static object SetDateTime(DateTime? value)
        {
            if (value.HasValue && value > DateTime.MinValue)
                return value;
            else
                return DBNull.Value;
        }
        public static object SetBoolean(bool? value)
        {
            if (value.HasValue)
                return value;
            else
                return DBNull.Value;
        }
        public static object SetTimeSpan(TimeSpan? value)
        {
            if (value.HasValue)
                return value;
            else
                return DBNull.Value;
        }
        public static object SetBytes(byte[] value)
        {
            if (value != null)
                return value;
            else
                return DBNull.Value;
        }
    }


    public static class DBGet
    {
        public static int? GetInt(object value)
        {
            if (value != DBNull.Value)
                return Convert.ToInt16(value);
            else
                return null;
        }
        public static double? GetDouble(object value)
        {
            if (value != DBNull.Value)
                return Convert.ToDouble(value);
            else
                return null;
        }
        public static string GetString(object value)
        {
            if (value != DBNull.Value)
                return value.ToString();
            else
                return null;
        }
        public static DateTime? GetDateTime(object value)
        {
            if (value != DBNull.Value)
                return Convert.ToDateTime(value);
            else
                return null;
        }
        public static bool? GetBoolean(object value)
        {
            if (value != DBNull.Value)
                return Convert.ToBoolean(value);
            else
                return null;
        }
        public static TimeSpan? GetTimeSpan(object value)
        {
            if (value != DBNull.Value)
                return TimeSpan.Parse(value.ToString());
            else
                return null;
        }
        public static byte[] GetBytes(object value)
        {
            if (value != DBNull.Value)
                return (byte[])value;
            else
                return null;
        }
    }


    public static class Common
    {
        public static List<string> ImgExt = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        public static List<string> VidExt = new List<string> { ".MOV", ".MP4", ".MKV", ".AVI", ".3GP" };
        public static List<string> AudExt = new List<string> { ".MP3", ".FLAC", ".WMA" };
        public static List<string> DocExt = new List<string> { ".PDF", ".DOC", ".DOCX", ".XLS", ".ODP", ".ODT" };

        public static void SetSession(string last_file)
        {
            System.IO.File.WriteAllText("session", last_file);
        }

        public static string GetSession()
        {
            if (System.IO.File.Exists("session"))
                return System.IO.File.ReadAllLines("session")[0];
            else
                return "";
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }
    }
}