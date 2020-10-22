using System;
using System.Diagnostics;
using System.IO.Compression;
using System.ServiceProcess;
using System.Threading;

namespace CommandanteUpdater
{
    class Program
    {
        // args:
        // 0: Service Name
        // 1: zip file
        // 2: target directory
        // 3: PID to wait for stopping
        static void Main(string[] args)
        {
            if (args.Length != 4 || !int.TryParse(args[3], out int Pid)) return;
            Process process = Process.GetProcessById(Pid);
            ServiceController service = new ServiceController(args[0]);
            try
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            ZipFile.ExtractToDirectory(args[1], args[2], true);
            try
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }
    }
}
