using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace ApagarBackup
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            Log("Inicio");
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timer = new Timer(new TimerCallback(estaFucionando), null, 5000, 720 * 60000);
        }

        private void estaFucionando(object state)
        {
            try
            {
                var caminho = @"D:\BACKUP";
                var pasta = new DirectoryInfo(caminho);

                foreach (var item in pasta.GetFiles())
                {
                    var dataCriacao = item.LastWriteTime;

                    if (dataCriacao <= DateTime.Now.AddDays(-5))
                    {
                        Log(item.Name, "arquivo apagado");
                        item.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Errou", ex.Message);
            }
        }

        protected override void OnStop()
        {
            Log("Parou");
        }

        public static void Log(string type, string error = "")
        {
            string folderLog = @"C:\LogApagarBackup\";

            if (!Directory.Exists(folderLog))
            {
                Directory.CreateDirectory(folderLog);
            }

            var file = folderLog + "Log" + DateTime.Now.Month.ToString("D2") + ".txt";

            if (!File.Exists(file))
            {
                FileInfo file_ = new FileInfo(file);
            }

            using (StreamWriter escritor = new StreamWriter(file, true))
            {
                escritor.WriteLine("{0}: {1} {2}", type, DateTime.Now.ToString(), error);
            }
        }
    }
}
