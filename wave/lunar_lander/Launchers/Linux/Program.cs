using System;

namespace RLEnvs
{
    public class ProgramArguments
    {
        public enum InitMode {random, far, center};

        public bool IsServerMode;
        public int Port;
        public bool Is3D;
        internal bool RenderMode;
        public InitMode ResetMode;
        internal int Seed;
    }

    public static class Program
    {
        public static ProgramArguments Arguments { get; private set; }

        //[STAThread]
        static void Main(string[] args)
        {
            Arguments = new ProgramArguments();
            // ["server"|"keyboard"] [port] [renderMode] ["random", "far", "center"] [seed]

            // defaults
            Arguments.IsServerMode = true;
            Arguments.Port = 3000;
            Arguments.RenderMode = true;
            Arguments.ResetMode = ProgramArguments.InitMode.center;
            Arguments.Seed = (new System.Random()).Next();

            if (args.Length > 0)
            {
                if (args[0] != "server" && args[0] != "keyboard")
                {
                    throw new ArgumentException("The first argument must be 'server' or 'keyboard'.");
                }

                Arguments.IsServerMode = args[0] == "server";
            }

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out Arguments.Port))
                {
                    throw new ArgumentException("When the server mode is selected the second argument is the server port and must be an integer.");
                }
            }

            if (args.Length > 2)
            {
                Arguments.RenderMode = args[2] == "true" || args[2] == "True";
            }

            if (args.Length > 3)
            {
                if (args[3] != "random" && args[3] != "far" && args[3] != "center")
                {
                    throw new ArgumentException("The fourth argument must be 'random', 'far', or 'center'.");
                }
                switch (args[3])
                {
                    case "center":
                        Arguments.ResetMode = ProgramArguments.InitMode.center;
                        break;
                    case "random":
                        Arguments.ResetMode = ProgramArguments.InitMode.random;
                        break;
                    case "far":
                        Arguments.ResetMode = ProgramArguments.InitMode.far;
                        break;
                }
            }

            if (args.Length > 4)
            {
                if (!int.TryParse(args[4], out Arguments.Seed))
                {
                    throw new ArgumentException("Provided seed should be an integer.");
                }
            }

            // set to be always true
            Arguments.Is3D = true;

            using (App game = new App(Arguments.RenderMode, Arguments.Seed))
            {
                game.Run();
            }
        }
    }
}

