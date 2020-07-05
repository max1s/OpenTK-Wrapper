using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace SevenSeasProject
{
    class CameraController
    {
        public Camera myCamera;
        GameWindow myWindow;
        public bool capMouse = true;
        public bool wireFrame = false;

        public CameraController(Camera c, GameWindow window )
        {
            myCamera = c;
            myWindow = window;
            System.Windows.Forms.Cursor.Hide();
            window.Mouse.Move += (sender, e) =>
            {
                if (!capMouse)
                    return;

                System.Drawing.Point center = new System.Drawing.Point((myWindow.Bounds.Left + myWindow.Bounds.Width / 2), (myWindow.Bounds.Top + myWindow.Bounds.Height / 2));
                if (System.Windows.Forms.Cursor.Position == center)
                    return;

                myCamera.Rotation += new OpenTK.Vector3(e.YDelta, e.XDelta, 0f) / 512f;
                System.Windows.Forms.Cursor.Position = center;
            };

            window.Keyboard.KeyDown += (sender, e) =>
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        if (capMouse)
                            System.Windows.Forms.Cursor.Show();
                        else
                            System.Windows.Forms.Cursor.Hide();
                        capMouse = !capMouse;
                        break;

                }

            };

        }

        public void OnUpdate()
        {
            if (myWindow.Keyboard[Key.W])
                myCamera.Position += myCamera.Forward / 6f;

            if (myWindow.Keyboard[Key.A])
                myCamera.Position -= myCamera.Right / 6f;

            if (myWindow.Keyboard[Key.D])
                myCamera.Position += myCamera.Right / 6f;

            if (myWindow.Keyboard[Key.S])
                myCamera.Position -= myCamera.Forward/ 6f;

            if (myWindow.Keyboard[Key.G])
                if (wireFrame)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    wireFrame = !wireFrame;
                }
                else
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    wireFrame = !wireFrame;
                }
        
        }

    }
}
