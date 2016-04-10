using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using WsnTools;

namespace DrioSync {

	//ジェネリクスとかでやってみるか？intとかStringとかにかぎらない。可変長ジェネリクスはめんどくさそうなので、必要そうならインターフェースでやるといいかも
	public class SQLDataBase {

		public string dbFile;
		public SQLDataBase(string dbFile) {
			this.dbFile = dbFile;
			if (!File.Exists(dbFile)) {
				string sql = "create table files(id INTEGER PRIMARY KEY AUTOINCREMENT,isFile INTEGER, linkDirectory TEXT, originalDirectory TEXT)";
				SendSQLCommand(sql);
			}
		}

		public struct FilesTable {
			public int isFile;
			public string linkDirectory, originalDirectory;

			public FilesTable(bool isFile, string linkDirectory, string originalDirectory) {
				this.isFile = isFile ? 1 : 0;
				this.linkDirectory = linkDirectory;
				this.originalDirectory = originalDirectory;
			}
		}
		public List<string> GetColumnFromDB(string column, string table, string key = "", string value = "") {
			if (key != "" && value != "") {
				key = " WHERE " + key;
				value = " = '" + value + "'";//valueを''でくくるやつなんとかしたい
			}
			List<string> tList = new List<string>();
			using (var conn = new SQLiteConnection("Data Source=" + dbFile)) {
				conn.Open();
				using (SQLiteCommand command = conn.CreateCommand()) {
					command.CommandText = "SELECT " + column + " FROM " + table + key + value;
					using (SQLiteDataReader reader = command.ExecuteReader()) {
						while (reader.Read()) {
							tList.Add(reader[column].ToString());
						}
					}
				}
				conn.Close();
			}
			return tList;
		}



		public void SendSQLCommand(string sql) {
			using (var connection = new SQLiteConnection("DATA Source=" + dbFile)) {
				connection.Open();
				using (SQLiteCommand command = connection.CreateCommand()) {
					command.CommandText = sql;
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
		}

		public void InsertToDB(FilesTable filesTable) {
			InsertToDB(new List<FilesTable>() { filesTable });
		}

		public void DeleteFromDB(string table, string key, string value) {
			//valueを''でくくるやつなんとかしたい
			SendSQLCommand("DELETE FROM " + table + " where " + key + " = '" + value + "'");
		}

		public void InsertToDB(IEnumerable<FilesTable> fList) {
			using (var connection = new SQLiteConnection("Data Source=" + dbFile)) {
				connection.Open();
				using (SQLiteTransaction sqlt = connection.BeginTransaction()) {
					using (SQLiteCommand command = connection.CreateCommand()) {
						foreach (FilesTable f in fList) {
							command.CommandText = @"insert into files (isFile, linkDirectory, originalDirectory) values(@isFile,@linkDirectory,@originalDirectory)";
							command.Parameters.Add("isFile", DbType.Int16);
							command.Parameters.Add("linkDirectory", DbType.String);
							command.Parameters.Add("originalDirectory", DbType.String);
							command.Parameters["isFile"].Value = f.isFile;
							command.Parameters["linkDirectory"].Value = f.linkDirectory;
							command.Parameters["originalDirectory"].Value = f.originalDirectory;
							command.ExecuteNonQuery();
						}
					}
					sqlt.Commit();
				}
				connection.Close();
			}
		}
	}

	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		///
		public static string computerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
		public static string homeDir = Environment.GetEnvironmentVariable("USERPROFILE");
		public static string BackupDir = Path.Combine(homeDir, "Dropbox", "BackupLinks", computerName);

		[STAThread]
		static void Main() {

			string[] args = Environment.GetCommandLineArgs();
			List<string> argList = args.ToList();
			argList.RemoveAt(0);

			if (argList.Count == 0) {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			else {
				foreach (string arg in argList) {
					linkToBackup(arg);
				}
			}
		}


		public static void linkToBackup(string file) {
			Directory.CreateDirectory(Path.Combine(BackupDir, StringOperator.GetRootChar(file), StringOperator.GetDirectoryNameExceptRoot(file)));
			FileOperator.MkLinkOption option;
			bool isFile;
			if (File.Exists(file)) {
				option = FileOperator.MkLinkOption.HardLink;
				isFile = true;
			}
			else if (Directory.Exists(file)) {
				option = FileOperator.MkLinkOption.Junction;
				isFile = false;
			}
			else {
				Console.WriteLine("ファーｗｗｗ");
				throw new FileNotFoundException();
			}
			string linkDirectory = Path.Combine(BackupDir, StringOperator.GetRootChar(file), StringOperator.GetFileNameExceptRoot(file));
			string originalDirectory = file;
			bool noerror = FileOperator.MkLink(linkDirectory, originalDirectory, option).CheckNoError();

			//いちいちトランザクション生成して遅いらしいけど、暫定。
			if (noerror) {
				(new SQLDataBase("sync.db")).InsertToDB(new SQLDataBase.FilesTable(isFile, linkDirectory, originalDirectory));
			}
		}
	}
}