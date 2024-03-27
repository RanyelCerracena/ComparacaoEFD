using comparacao_pis_cofins;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Data;
using System.Data.Odbc;

namespace SQLAnywhere
{
    public class Conexao
    {
        public static string ConString = $"DSN=DOMINIO;Uid=NEOSOLUTIONS;Pwd=NEOSOLUTIONS";

        public static void CriarConexaoODBC()
        {
            if (!Program.IsUserAdministrator()) {
                Program.RunWithAdminPrivileges(new string[] { "/createodbc" });
                return;
            }

            using (RegistryKey rootKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                RegistryKey regKey = rootKey.OpenSubKey("SOFTWARE\\ODBC\\ODBC.INI\\DOMINIO", true);

                if (regKey == null)
                {
                    regKey = rootKey.CreateSubKey("SOFTWARE\\ODBC\\ODBC.INI\\DOMINIO");
                }

                regKey.SetValue("CommLinks", $"TCPIP{{IP=192.168.40.200;ServerPort=2638}}");
                regKey.SetValue("DatabaseName", "Contabil");
                regKey.SetValue("Description", "DOMINIO");
                regKey.SetValue("Driver", @"C:\Program Files\SQL Anywhere 16\Bin32\dbodbc16.dll");
                regKey.SetValue("Encryption", "NONE");
                regKey.SetValue("Integrated", "NO");
                regKey.SetValue("ServerName", "srvcontabil");

                try
                {
                    regKey.Close();
                }
                catch { }

                RegistryKey odbcKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources");
                odbcKey.SetValue("DOMINIO", "SQL Anywhere 16", RegistryValueKind.String);
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBCINST.INI").SetValue("ODBCINST_INI_PATH", @"C:\Windows\System32\odbcinst.ini");
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBCINST.INI").SetValue("ODBC_DRIVER_REGISTER", "1");
            }
        }

        public static bool TestarConexao()
        {
            try {
                OdbcConnection con = new OdbcConnection(ConString);
                con.Open();

                return true;
            } catch {
                CriarConexaoODBC();
                try {
                    OdbcConnection con = new OdbcConnection(ConString);
                    con.Open();

                    return true;
                } catch {
                    return false;
                }
            }
        }

        public static List<T> ObterDados<T>(string query)
        {
            List<T> objetos = new List<T>();
            try
            {
                bool modelo = true;
                try
                {
                    var instancia = Activator.CreateInstance(typeof(T), false);
                    instancia = null;
                }
                catch
                {
                    modelo = false;
                }

                OdbcConnection con = new OdbcConnection(ConString);

                if (modelo)
                {
                    using (DataTable dt = new DataTable())
                    {
                        using (con)
                        {
                            OdbcDataAdapter adapter = new OdbcDataAdapter(query, con);
                            con.Open();
                            adapter.Fill(dt);

                            var resposta = JsonConvert.SerializeObject(dt);
                            objetos = JsonConvert.DeserializeObject<List<T>>(resposta);
                        }
                    }
                }
                else
                {
                    using (con)
                    {
                        OdbcCommand cmd = new OdbcCommand(query, con);
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            objetos.Add((T)Convert.ChangeType(dr.GetValue(0), typeof(T)));
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return objetos;
        }
        public static T ObterDado<T>(string query)
        {
            return ObterDados<T>(query).FirstOrDefault();
        }

        public static T ObterDadoUnitario<T>(string query)
        {
            T objeto = default(T);
            try
            {
                OdbcConnection con = new OdbcConnection(ConString);
                using (con)
                {
                    con.Open();
                    OdbcCommand cmd = new OdbcCommand(query, con);
                    try
                    {
                        objeto = (T)Convert.ChangeType(cmd.ExecuteScalar(), typeof(T));
                    }
                    catch { }
                }
            }
            catch { }
            return objeto;
        }
    }
}
