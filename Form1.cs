using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace TwitchAdmin
{
    public partial class Form1 : Form
    {
        // Bot settings
        private static string _botName = "fiercedeityninja";
        private static string _broadcasterName = "itsmehamham";
        //private static string _twitchOAuth = "oauth:50bpq88z8hpv4spvhykn0j3i5gbo5w"; // fiercedeityninja
        //private static string _twitchOAuth = "oauth:b8q75mlelgf3lqu47ptp7saw5totzx"; //fiercedeitybot
        private static string _twitchOAuth = "oauth:1upqxdzd3wwhy2ev0h6vuytdprrdk7"; //0x420b
        IrcClient irc = new IrcClient("irc.chat.twitch.tv", 6667,
                _botName, _twitchOAuth, _broadcasterName);

        public Form1()
        { // Initialize and connect to Twitch chat

            PingSender ping = new PingSender(irc);
            ping.Start();
            Thread T = new Thread(() => loop(irc));
            T.Start();

            InitializeComponent();
            //loop(irc);


        }

delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.AppendText(text + "\r\n");
            }
        }

        private void SetText2(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText2);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox1.Items.Add(text);
                listBox1.Hide();
                listBox1.Show();
            }
        }


        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                textBox1.AppendText(_botName + " -> " + this.textBox2.Text + "\r\n");
                irc.SendPublicChatMessage(this.textBox2.Text+"\r\n");
                this.textBox2.Text = null;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IrcClient irc = new IrcClient("irc.chat.twitch.tv", 6667,
                _botName, _twitchOAuth, _broadcasterName);

            
        }

        private void loop(IrcClient irc)
        {
            while (true)
            {
                string message = irc.ReadMessage();
                Console.WriteLine(message); // Print raw irc messages

                
                if (message == null)
                    break;
                if (message.Contains("PRIVMSG"))
                {
                    int intIndexParseSign = message.IndexOf('!');
                    string userName = message.Substring(1, intIndexParseSign - 1); // parse username from specific section (without quotes)
                                                                                   // Format: ":[user]!"
                                                                                   // Get user's message
                    intIndexParseSign = message.IndexOf(" :");
                    message = message.Substring(intIndexParseSign + 2);

                    Console.WriteLine(message); // Print parsed irc message (debugging only)
                    SetText(userName + " -> " + message);

                    if (!listBox1.Items.Contains(userName))
                    {
                        SetText2(userName);
                    }

                    

                    // Broadcaster commands
                    if (userName.Equals(_broadcasterName))
                    {
                        if (message.Equals("!exitbot"))
                        {
                            irc.SendPublicChatMessage("Bye! Have a beautiful time!");
                            Environment.Exit(0); // Stop the program
                        }
                    }

                    // General commands anyone can use
                    if (message.Equals("!hello"))
                    {
                        irc.SendPublicChatMessage("Hello World!");
                    }

                }
            }
            throw new NotImplementedException();
            Thread.Sleep(100);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            irc.SendPublicChatMessage("Hello, @" + listBox1.GetItemText(listBox1.SelectedItem));
            SetText(_botName + " - > " + "Hello, @" + listBox1.GetItemText(listBox1.SelectedItem));
            this.ActiveControl = textBox2;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SetText("BANNED: " + listBox1.GetItemText(listBox1.SelectedItem));
            irc.SendPublicChatMessage("/ban " + listBox1.GetItemText(listBox1.SelectedItem).ToString());
            this.ActiveControl = textBox2;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SetText("UNBANNED: " + listBox1.GetItemText(listBox1.SelectedItem));
            irc.SendPublicChatMessage("/unban " + listBox1.GetItemText(listBox1.SelectedItem).ToString());
            this.ActiveControl = textBox2;
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            SetText("TIMED OUT: " + listBox1.GetItemText(listBox1.SelectedItem));
            irc.SendPublicChatMessage("/timeout " + listBox1.GetItemText(listBox1.SelectedItem).ToString());
            this.ActiveControl = textBox2;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = null;
            this.ActiveControl = textBox2;
        }


    }
}
