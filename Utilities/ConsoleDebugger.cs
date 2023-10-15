using ConsoleLib.Console;

namespace CavesOfQuickMenu.Utilities
{
    public class ConsoleDebugger
    {
        private const int LENGTH_LIMIT = 50;

        private readonly TextConsole textConsole;
        private readonly ScreenBuffer buffer, bufferInit;

        public ConsoleDebugger(TextConsole textConsole, ScreenBuffer buffer)
        {
            this.textConsole = textConsole;
            this.buffer = buffer;
            bufferInit = buffer;
        }

        public void DrawMessage(string message)
        {
            buffer.Copy(bufferInit);
            int limit = message.Length < LENGTH_LIMIT ? message.Length : LENGTH_LIMIT;
            for (int x = 0; x < limit; x++)
            {
                buffer.SetCharAt(x, 0, message[x]);
            }
            textConsole.DrawBuffer(buffer);
        }
    }
}
