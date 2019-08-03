using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace TwitchAdmin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {

           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
 

        }
    }
}


