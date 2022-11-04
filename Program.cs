using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unit04.Game.Casting;
using Unit04.Game.Directing;
using Unit04.Game.Services;


namespace Unit04
{
    /// <summary>
    /// The program's entry point.
    /// </summary>
    class Program
    {
        private static int FRAME_RATE = 12;
        private static int MAX_X = 1200;//Default 900
        private static int MAX_Y = 900;//Default 600
        private static int CELL_SIZE = 30;
        private static int FONT_SIZE = 33;
        private static int COLS = 60;//Default 60
        private static int ROWS = 40;//Default 40
        private static string CAPTION = "Greed";
        private static string DATA_PATH = "Data/messages.txt";
        private static Color WHITE = new Color(255, 255, 255);
        private static int DEFAULT_ARTIFACTS = 40;


        /// <summary>
        /// Starts the program using the given arguments.
        /// </summary>
        /// <param name="args">The given arguments.</param>
        static void Main(string[] args)
        {
            // create the cast
            Cast cast = new Cast();

            // create the banner
            Actor banner = new Actor();
            banner.SetText("");
            banner.SetFontSize(FONT_SIZE);
            banner.SetColor(WHITE);
            banner.SetPosition(new Point(CELL_SIZE, 0));
            cast.AddActor("banner", banner);

            // create the robot
            Actor robot = new Actor();
            robot.SetText("#");
            robot.SetFontSize(FONT_SIZE);
            robot.SetColor(WHITE);
            robot.SetPosition(new Point(MAX_X / 2, MAX_Y / 2));
            cast.AddActor("robot", robot);

            // load the messages
            List<string> messages = File.ReadAllLines(DATA_PATH).ToList<string>();

            // create the artifacts
            Random random = new Random();
            for (int i = 0; i < DEFAULT_ARTIFACTS; i++)
            {
                // CHAR = ■ (alt254)
                string text = "o";
                int indexx = random.Next(0, 3);
                if (indexx == 1)
                {
                    text = ((char)random.Next(42, 43)).ToString();
                }
                else
                {
                    text = ((char)random.Next(164, 165)).ToString();
                }

                string message = messages[i];

                int x = random.Next(1, COLS);
                int y = random.Next(1, ROWS);
                Point position = new Point(x, y);
                position = position.Scale(CELL_SIZE);

                int r = 0;
                int b = 0;
                int g = 0;

                while (r + g + b < 255)
                {
                    r = random.Next(0, 256);
                    g = random.Next(0, 256);
                    b = random.Next(0, 256);
                }

                int speed = random.Next(1, 4);
                speed = speed * 5;
                Color color = new Color(r, g, b);

                Artifact artifact = new Artifact();
                artifact.SetText(text);
                artifact.SetFontSize(FONT_SIZE);
                artifact.SetColor(color);
                artifact.SetPosition(position);
                artifact.SetMessage(message);
                artifact.SetSpeed(speed);
                cast.AddActor("artifacts", artifact);
            }

            // start the game
            KeyboardService keyboardService = new KeyboardService(CELL_SIZE);
            VideoService videoService
                = new VideoService(CAPTION, MAX_X, MAX_Y, CELL_SIZE, FRAME_RATE, false);
            Director director = new Director(keyboardService, videoService);
            director.StartGame(cast);

        }
    }
}