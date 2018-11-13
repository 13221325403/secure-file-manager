using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SecureFileManager.Model;
using System.Data;

namespace SecureFileManager.Controller
{
    /// <summary>
    /// CRUD for table 'files'
    /// </summary>
    static class FileController
    {
        public static List<File> GetDirectoryFiles(int pk)
        {
            string q = (pk > 0)
                ? "select * from files where fk_directory = " + pk
                : "select * from files where fk_directory is null";

            List<File> files = new List<File>();

            DBConnector.conn.Open();
            SQLiteCommand command = new SQLiteCommand(q, DBConnector.conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                files.Add(_getFile(reader));
            }
            command.Dispose();
            DBConnector.conn.Close();
            return files;
        }

        public static DataTable GetDirectoryFilesDT(int pk)
        {
            string q = (pk > 0)
                ? "select * from files where fk_directory = " + pk
                : "select * from files where fk_directory is null";

            DataTable dt = new DataTable();
            DBConnector.conn.Open();
            SQLiteCommand command = new SQLiteCommand(q, DBConnector.conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);
            da.Fill(dt);
            command.Dispose();
            DBConnector.conn.Close();
            return dt;
        }

        public static File select(int pk)
        {
            string q = "select * from files where pk_file = " + pk;
            File f = null;

            DBConnector.conn.Open();
            SQLiteCommand command = new SQLiteCommand(q, DBConnector.conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                f = _getFile(reader);
            }
            command.Dispose();
            DBConnector.conn.Close();
            return f;
        }

        public static int insert(File f)
        {
            string q = "insert into files (name, ext, length, file, description, creation_date, last_edit_date, fk_directory) values (@name, @ext, @len, @file, @descr, @cd, @led, @fk_dir);";

            SQLiteCommand cmd = new SQLiteCommand(q, DBConnector.conn);
            cmd.Parameters.AddWithValue("@name", DBSet.SetString(f.name));
            cmd.Parameters.AddWithValue("@ext", DBSet.SetString(f.ext));
            cmd.Parameters.AddWithValue("@len", f.length);
            cmd.Parameters.AddWithValue("@file", DBSet.SetBytes(f.file));
            cmd.Parameters.AddWithValue("@descr", f.description);
            cmd.Parameters.AddWithValue("@cd", DBSet.SetDateTime(f.creation_date));
            cmd.Parameters.AddWithValue("@led", DBSet.SetDateTime(f.last_edit_date));
            cmd.Parameters.AddWithValue("@fk_dir", DBSet.SetInt(f.fk_directory));

            return DBConnector.Query(cmd);
        }

        public static int update(File f)
        {
            string q = "update files set name=@name, description=@descr, last_edit_date=@led, fk_directory=@fk_dir where pk_file=@pk;";

            SQLiteCommand cmd = new SQLiteCommand(q, DBConnector.conn);
            cmd.Parameters.AddWithValue("@name", DBSet.SetString(f.name));
            cmd.Parameters.AddWithValue("@descr", f.description);
            cmd.Parameters.AddWithValue("@led", DBSet.SetDateTime(f.last_edit_date));
            cmd.Parameters.AddWithValue("@fk_dir", DBSet.SetInt(f.fk_directory));
            cmd.Parameters.AddWithValue("@pk", DBSet.SetInt(f.pk));

            return DBConnector.Query(cmd);
        }

        public static int delete(int pk)
        {
            string q = "delete from files where pk_file = "+pk;
            return DBConnector.Query(q);
        }

        static File _getFile(SQLiteDataReader reader)
        {
            File f = new File();
            f.pk = Convert.ToInt16(reader["pk_file"]);
            f.name = Convert.ToString(reader["name"]);
            f.ext = Convert.ToString(reader["ext"]);
            f.length = Convert.ToInt32(reader["length"]);
            f.file = (byte[])reader["file"];
            f.description = Convert.ToString(reader["description"]);
            f.creation_date = DBGet.GetDateTime(reader["creation_date"]);
            f.last_edit_date = DBGet.GetDateTime(reader["last_edit_date"]);
            f.fk_directory = DBGet.GetInt(reader["fk_directory"]);
            return f;
        }

        static File _getFile(DataRow row)
        {
            File f = new File();
            f.pk = Convert.ToInt16(row["pk_file"]);
            f.name = Convert.ToString(row["name"]);
            f.ext = Convert.ToString(row["ext"]);
            f.length = Convert.ToInt32(row["length"]);
            f.file = (byte[])row["file"];
            f.description = Convert.ToString(row["description"]);
            f.creation_date = DBGet.GetDateTime(row["creation_date"]);
            f.last_edit_date = DBGet.GetDateTime(row["last_edit_date"]);
            f.fk_directory = DBGet.GetInt(row["fk_directory"]);
            return f;
        }
    }


    /// <summary>
    /// CRUD for table 'directories'
    /// </summary>
    static class DirectoryController
    {
        public static Directory Select(int directory_pk)
        {
            string q = "select * from directories where pk_directory = " + directory_pk;
            Directory dir = null;

            DBConnector.conn.Open();
            SQLiteCommand command = new SQLiteCommand(q, DBConnector.conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dir = _getDirectory(reader);
            }
            command.Dispose();
            DBConnector.conn.Close();
            return dir;
        }

        public static List<Directory> GetDirectories()
        {
            string q = "select *, (select count(*) from files where fk_directory = directories.pk_directory) as 'files_count' from directories;";
            List<Directory> cartelle = new List<Directory>();

            DBConnector.conn.Open();
            SQLiteCommand command = new SQLiteCommand(q, DBConnector.conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cartelle.Add(_getDirectory(reader));
            }
            command.Dispose();
            DBConnector.conn.Close();
            return cartelle;
        }      

        public static int Delete(int pk_cartella)
        {
            string q =string.Format("delete from directories where pk_directory = {0}; update files set fk_directory = NULL where fk_directory = {0};", pk_cartella);
            return DBConnector.Query(q);
        }

        public static int InsertUpdate(Directory c)
        {
            string q = (c.pk > 0)
                ? "update directories set name=@n, description=@d, color=@c where pk_directory=@pk;"
                : "insert into directories (name, description, color) values (@n, @d, @c);";

            SQLiteCommand cmd = new SQLiteCommand(q, DBConnector.conn);
            cmd.Parameters.AddWithValue("@n", c.name);
            cmd.Parameters.AddWithValue("@d", c.description);
            cmd.Parameters.AddWithValue("@c", c.color);
            if (c.pk > 0)
                cmd.Parameters.AddWithValue("@pk", c.pk);

            return DBConnector.Query(cmd);
        }

        static Directory _getDirectory(SQLiteDataReader reader)
        {
            Directory d = new Directory();
            d.pk = Convert.ToInt16(reader["pk_directory"]);
            d.name = Convert.ToString(reader["name"]);
            d.description = Convert.ToString(reader["description"]);
            d.fk_parent_directory = DBGet.GetInt(reader["fk_parent_directory"]);
            d.color = Convert.ToString(reader["color"]);
            try { d.files_count = Convert.ToInt16(reader["files_count"]); } catch { }
            return d;
        }
    }

}


/*
public static List<Directory> GetChild(int fk_padre)
{
    string q = (fk_padre > 0)
        ? string.Format("select * from cartelle where cartella_superiore = {0} order by nome", fk_padre)
        : "select * from cartelle where cartella_superiore is null order by nome;";

    List<Directory> cartelle = new List<Directory>();

    DBConnector.conn.Open();
    SQLiteCommand command = new SQLiteCommand(q, DBConnector.conn);
    SQLiteDataReader reader = command.ExecuteReader();
    while (reader.Read())
    {
        cartelle.Add(_getDirectory(reader));
    }
    DBConnector.conn.Close();
    return cartelle;
}
 */
