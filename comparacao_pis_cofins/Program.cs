using System.Diagnostics;
using System.Security.Principal;

namespace comparacao_pis_cofins
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Contains("/createodbc"))
                {
                    if (!IsUserAdministrator())
                    {
                        MessageBox.Show("O programa deve ser executado como administrador para executar a fun��o desejada!", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Environment.Exit(0);
                    }

                    SQLAnywhere.Conexao.CriarConexaoODBC();
                    Environment.Exit(0);
                } 
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static bool IsUserAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RunWithAdminPrivileges(string[] args)
        {
            // Obter o caminho do execut�vel do programa atual
            string executablePath = Process.GetCurrentProcess().MainModule.FileName;

            // Criar uma nova inst�ncia do processo com privil�gios de administrador
            ProcessStartInfo startInfo = new ProcessStartInfo(executablePath);
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas"; // Solicitar privil�gios de administrador

            // Passar os argumentos que informam quais m�todos devem ser executados
            startInfo.Arguments = string.Join(" ", args);

            try
            {
                // Iniciar o processo com privil�gios de administrador
                var p = Process.Start(startInfo);
                p.WaitForExit();
            }
            catch (Exception ex) { }
        }
    }
}