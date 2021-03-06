using System;
using BroadEngine.Core;
using BroadEngineTester.Activities;

namespace BroadEngineTester
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game game = new Game("Content.txt"))
            {
                game.Run<ColorScreenActivity>();
            }
        }
    }
#endif
}

