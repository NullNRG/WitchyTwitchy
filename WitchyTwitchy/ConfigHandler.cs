using System;
using System.IO;

namespace WitchyTwitchy
{
    public class ConfigHandler
    {
        private const string configFile = @"\config.txt";
        private const string oAuthFile = @"\DO_NOT_SHARE.txt";

        public static string ConfigDir { get; set; } = $@"{Environment.CurrentDirectory}\Config";

        public ConfigHandler()
        {
            Start();
        }

        public void Start()
        {
            CheckConfigDirectory();
            CheckConfigFile();
            CheckOAuthFile();
            LoadConfigs();
        }

        private static void CheckConfigFile()
        {
            if (!File.Exists($"{ConfigDir}{configFile}"))
            {
                using StreamWriter sW = new StreamWriter(new FileStream($"{ConfigDir}{configFile}", FileMode.Create, FileAccess.Write, FileShare.Read));
                sW.Write($"Name:{Environment.NewLine}Channel:");
            }
        }

        private static void CheckOAuthFile()
        {
            if (!File.Exists($"{ConfigDir}{oAuthFile}"))
            {
                using StreamWriter sW = new StreamWriter(new FileStream($"{ConfigDir}{oAuthFile}", FileMode.Create, FileAccess.Write, FileShare.Read));
                sW.Write("OAuth:");
            }
        }

        private static void CheckConfigDirectory()
        {
            if (!Directory.Exists(ConfigDir))
            {
                Directory.CreateDirectory(ConfigDir);
            }
        }

        public static void LoadConfigs()
        {
            LoadConfigFile();

            LoadOAuthFile();
        }

        private static void LoadOAuthFile()
        {
            using StreamReader sR = new StreamReader($"{ConfigDir}{oAuthFile}");
            string[] lines = File.ReadAllLines($"{ConfigDir}{oAuthFile}");
            if (lines == null)
            {
                LoadOAuthFile();
            }
            foreach (string line in lines)
            {
                string[] tempLine = line.Split(':');

                switch (tempLine[0])
                {
                    case "OAuth":
                        if (tempLine.Length == 3)
                        {
                            Program.password = $"{tempLine[1]}:{tempLine[2]}";
                        }
                        else
                        {
                            Console.WriteLine($"---Loading oauth from {oAuthFile} FAILED. " +
                                "Please make sure you have your oauth set correctly - OAuth:oauth:xxxxxxxxxx");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void LoadConfigFile()
        {
            using StreamReader sR = new StreamReader($"{ConfigDir}{configFile}");
            string[] lines = File.ReadAllLines($"{ConfigDir}{configFile}");
            if (lines == null)
            {
                LoadConfigFile();
            }
            foreach (string line in lines)
            {
                string[] tempLine = line.Split(':');
                switch (tempLine[0])
                {
                    case "Name":
                        Program.botUserName = tempLine[1];
                        break;
                    case "Channel":
                        Program.channelName = tempLine[1];
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
