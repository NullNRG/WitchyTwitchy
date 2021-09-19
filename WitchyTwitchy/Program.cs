using AsyncAwaitBestPractices;
using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using System;
using System.Threading.Tasks;

namespace WitchyTwitchy
{

    // IRC Login Requirements: NICK and PASS. USER is not necessary
    // Twitch will require the OAuth token for the password from https://twitchapps.com/tmi/
    // Use a Twitch account that isnt your main account

    // This bot will be written using async, and the Main() method 'void' return needs to be changed to 
    // async Task

    // If you want to use your OAuth password as an enviroment variabe, you can retrieve it using 
    // Environment.GetEnvironmentVariable(“TWITCH_OAUTH”)
    internal static class Program
    {

        #region OAuth
        public static string password;
        #endregion
        public static string botUserName;
        public static string channelName;
        private static TwitchBot twitchBot;
        private static readonly InputSimulator sim = new InputSimulator();

        private static async Task Main(string[] args)
        {
            ConfigHandler cfgHandler = new ConfigHandler();
            // Enviroment variable alternative: Environment.GetEnvironmentVariable(“TWITCH_OAUTH”)
            twitchBot = new TwitchBot(botUserName, password);
            twitchBot.Start().SafeFireAndForget();
            //We could .SafeFireAndForget() these two calls if we want to
            await twitchBot.JoinChannel(channelName.ToLower());
            //await twitchBot.SendMessage(channelName, "Hi");
            //await twitchBot.GatherUsers(channelName);
            twitchBot.OnMessage += OnMessage;
            #region Lambda Event Handler
/*            twitchBot.OnMessage += (sender, twitchChatMessage) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{twitchChatMessage.Sender}");
                Console.ResetColor();
                Console.Write($": ");
                Console.WriteLine($"{twitchChatMessage.Message}");
                string msg = twitchChatMessage.Message;

                /// Listen for commands
                /// NIDHOGG 2  
                ///
                /// PLAYER 1 CONTROLLS

                //  Left
                if (msg.Contains("l") || msg.Contains("i"))
                {
                    await PushButtons(VirtualKeyCode.VK_A, new TimeSpan(5000), 1500);
                }

                //  Right
                if (msg.Contains("r") || msg.Contains("e"))
                {
                    await PushButtons(VirtualKeyCode.VK_D, new TimeSpan(5000), 1500);
                }

                // UP
                if (msg.Contains("t") || msg.Contains("b"))
                {
                    await PushButtons(VirtualKeyCode.VK_W, new TimeSpan(5000), 1000);
                }

                // Crouch 
                if (msg.Contains("s"))
                {
                    await PushButtons(VirtualKeyCode.VK_S, new TimeSpan(100), 100);
                }

                // Attack
                if (msg.Contains("f") || msg.Contains("a"))
                {
                    await PushButtons(VirtualKeyCode.VK_F, new TimeSpan(5000), 100);
                }

                // Jump
                if (msg.Contains("g") || msg.Contains("o"))
                {
                    await PushButtons(VirtualKeyCode.VK_G, new TimeSpan(5000), 100);
                }
            };*/


            
            #endregion
            await Task.Delay(-1);
        }

        private static void OnMessage(object sender, TwitchChatMessage twitchChatMessage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{twitchChatMessage.Sender}");
            Console.ResetColor();
            Console.Write($": ");
            Console.WriteLine($"{twitchChatMessage.Message}");

            //Listen for !hey command
            if (twitchChatMessage.Message.Equals("hi".ToLower()))
            {
                Task task = twitchBot.SendMessage(twitchChatMessage.Channel, $"Hey there {twitchChatMessage.Sender}");
            }

            string msg = twitchChatMessage.Message;

            /// Listen for commands
            /// NIDHOGG 2  
            ///
            /// PLAYER 1 CONTROLLS

            //  Left
            if (msg.Contains("l") || msg.Contains("i"))
            {
                Task task = PushButtons(VirtualKeyCode.VK_A, new TimeSpan(5000), 1500);
            }

            //  Right
            if (msg.Contains("r") || msg.Contains("e"))
            {
                Task task =  PushButtons(VirtualKeyCode.VK_D, new TimeSpan(5000), 1500);
            }

            // UP
            if (msg.Contains("t") || msg.Contains("b"))
            {
                Task task =  PushButtons(VirtualKeyCode.VK_W, new TimeSpan(5000), 1000);
            }

            // Crouch 
            if (msg.Contains("s"))
            {
                Task task =  PushButtons(VirtualKeyCode.VK_S, new TimeSpan(100), 100);
            }

            // Attack
            if (msg.Contains("f") || msg.Contains("a"))
            {
                Task task = PushButtons(VirtualKeyCode.VK_F, new TimeSpan(5000), 100);
            }

            // Jump
            if (msg.Contains("g") || msg.Contains("o"))
            {
                Task task = PushButtons(VirtualKeyCode.VK_G, new TimeSpan(5000), 100);
            }
        }

        public static async Task PushButtons(VirtualKeyCode key, TimeSpan span, int ms)
        {
            await Task.Run(() => sim.Keyboard.KeyDown(key).Sleep(ms));
            await Task.Delay(span);
            await Task.Run(() => sim.Keyboard.KeyUp(key));
        }
    }
}
