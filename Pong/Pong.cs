using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using nulloader;

namespace Pong
{
    public class Pong : Plugin, IPluginIcon
    {
        float player1 = 0;
        float player2 = 0;

        int score1 = 0;
        int score2 = 0;

        float BallX = 0;
        float BallY = 0;

        float BallDirX = 0;
        float BallDirY = 0;

        TabPage tabpage;
        Random rand = new Random();
        PongControl controller;

        public Pong()
        {
            // usage: pong(x,y,time)
            RegisterFunction("pong", 3, pong);
            OperateOnControl(CreateEditorTab("Pong"), _ =>
            {
                tabpage = _ as TabPage;
                controller = new PongControl { Parent = tabpage, Dock = DockStyle.Fill };

                controller.Player1Up += (s, e) => player1 += 4;
                controller.Player1Down += (s, e) => player1 -= 4;

                controller.Player2Up += (s, e) => player2 += 4;
                controller.Player2Down += (s, e) => player2 -= 4;
            });
        }

        void Pong_Tick()
        {
            BallX += BallDirX;
            BallY += BallDirY;

            if (BallY > 10)
            {
                BallDirY *= -1.05f;
                BallY = 9.5f;
            }
            if (BallY < -10)
            {
                BallDirY *= -1.05f;
                BallY = -9.5f;
            }

            if (BallX < -9)
            {
                if (player1 + 4 < BallY || player1 - 4 > BallY)
                {
                    controller.SetScore(score1, score2++);
                    Begin();
                }
                else
                    BallDirX *= -1.05f;
            }

            if (BallX > 9)
            {
                if (player2 + 4 < BallY || player2 - 4 > BallY)
                {
                    controller.SetScore(score1++, score2);
                    Begin();
                }
                else
                    BallDirX *= -1.05f;
            }
        }

        void Begin()
        {
            BallDirX = 0;
            while (BallDirX == 0)
                BallDirX = rand.Next(-2, 4) / 4f;

            BallDirY = 0;
            while (BallDirY == 0)
                BallDirY = rand.Next(-2, 4) / 4f;
            BallX = 0;
            BallY = 0;
            running = true;
        }

        bool running = false;
        float last_time;

        public float pong(float[] args)
        {
            float x = args[0];
            float y = args[1];

            if (running && last_time < Math.Floor(args[2] * 16))
            {
                last_time = (float)Math.Floor(args[2] * 16);
                Pong_Tick();
            }

            if (args[2] /* time */ > 0 && !running)
                Begin();
            if (args[2] == 0)
                running = false;

            if (Math.Sqrt(Math.Pow(x - BallX, 2) + Math.Pow(y - BallY, 2)) < 0.4)
                return 1;

            if (x > -9.5 && x < -9 && y < player1 + 4 && y > player1 - 4)
                return 1;

            if (x < 9.5 && x > 9 && y < player2 + 4 && y > player2 - 4)
                return 1;

            return 0;
        }

        public Image GetIcon()
        {
            return Properties.Resources.joystick;
        }
    }
}
