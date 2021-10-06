using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WitchyTwitchy
{
    public partial class TwitchBot
    {
        private const string ip = "irc.chat.twitch.tv";
        private const int port = 6697;

        private readonly string nick;
        private readonly string password;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private readonly TaskCompletionSource<int> connected = new TaskCompletionSource<int>();

        public event EventHandler<TwitchChatMessage> OnMessage = delegate { };
        public delegate void TwitchChatEventHandler(object sender, TwitchChatMessage e);

        public TwitchBot(string nick, string password)
        {
            this.nick = nick;
            this.password = password;
        }

        public async Task Start()
        {
            InputSimulator sim = new InputSimulator();
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port).ConfigureAwait(false);
            SslStream sslStream = new SslStream(tcpClient.GetStream(),
                                      false,
                                      ValidateServerCertificate,
                                      null
            );
            await sslStream.AuthenticateAsClientAsync(ip).ConfigureAwait(false);
            streamReader = new StreamReader(sslStream);
            streamWriter = new StreamWriter(sslStream) { NewLine = Environment.NewLine, AutoFlush = true };

            await streamWriter.WriteLineAsync($"PASS {password}").ConfigureAwait(false);
            await streamWriter.WriteLineAsync($"NICK {nick}").ConfigureAwait(false);
            connected.SetResult(0);

            while (!sim.InputDeviceState.IsKeyDown(VirtualKeyCode.DELETE))
            {
                string line = await streamReader.ReadLineAsync().ConfigureAwait(false);
                //Console.WriteLine(line);

                try
                {
                    if (line != null)
                    {
                        string[] split = line.Split(" ");
                        await CheckPing(line, split).ConfigureAwait(false);
                        MessageHandler(line, split);
                    }
                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                }
                //string[] split = line.Split(" ");
                //PING :tmi.twitch.tv
                //Respond with PONG :tmi.twitch.tv
            }
            Environment.Exit(0);
        }

        private void MessageHandler(string line, string[] split)
        {
            if (split.Length > 2 && split[1] == "PRIVMSG")
            {
                //:mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv 
                // ^^^^^^^^
                //Grab this name here
                int exclamationPointPosition = split[0].IndexOf("!");
                string username = split[0][1..exclamationPointPosition];
                //Skip the first character, the first colon, then find the next colon
                int secondColonPosition = line.IndexOf(':', 1);//the 1 here is what skips the first character
                string message = line[(secondColonPosition + 1)..];//Everything past the second colon
                string channel = split[2].TrimStart('#');
                CheckUser(username);
                MessageEvent(username, message, channel);
            }
        }

        private void CheckUser(string username)
        {
            if (username != null)
            {
                if (!User._users.ContainsKey(username))
                {
                    CreateNewUser(username);
                }
            }
            if (User._users.Count>=1)
            {
                if(User._users.ContainsKey(username))
                {
                    User user = User._users[username];

                    user.MessageCount++;

                    //Console.WriteLine($"{username} has sent {user.MessageCount} messages in this server");
                }
            }
            
        }

        private void CreateNewUser(string username)
        {

            User user = new User(username, UserPermissions.Standard);
        }

        private void MessageEvent(string username, string message, string channel)
        {
            OnMessage?.Invoke(this, new TwitchChatMessage
            {
                Message = message,
                Sender = username,
                Channel = channel
            });
        }

        private async Task CheckPing(string line, string[] split)
        {
            if (line.StartsWith("PING"))
            {
                Console.WriteLine("PONG");
                await streamWriter.WriteLineAsync($"PONG {split[1]}").ConfigureAwait(false);
            }
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        public async Task SendMessage(string channel, string message)
        {
            await connected.Task.ConfigureAwait(false);
            await streamWriter.WriteLineAsync($"PRIVMSG #{channel} :{message}").ConfigureAwait(false);
        }

        public async Task JoinChannel(string channel)
        {
            await connected.Task.ConfigureAwait(false);
            await streamWriter.WriteLineAsync($"JOIN #{channel}").ConfigureAwait(false);
        }
    }
}
