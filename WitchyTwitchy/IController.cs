using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using System;
using System.Threading.Tasks;

namespace WitchyTwitchy
{
    internal interface IController
    {
  private static string _name;
  private static string _version;
  private static InputSimulator sim = new InputSimulator();

        TwitchBot TwitchBot { get; }

        public Task ParseInput();
        public static void OnMessage(object sender, TwitchChatMessage twitchChatMessage)
        {
        }

        public async Task GameInput(VirtualKeyCode key, TimeSpan span, int ms)
        {
            await Task.Run(() => sim.Keyboard.KeyDown(key).Sleep(ms));
            await Task.Delay(span);
            await Task.Run(() => sim.Keyboard.KeyUp(key));
        }
    }
}
