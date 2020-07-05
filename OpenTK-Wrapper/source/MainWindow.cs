
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK.Graphics;


namespace SevenSeasProject
{
  
    public class MainWindow : GameWindow
    {
      
        ModelShader boatShader;
        Ship ship;
        Stopwatch stopWatch;
        static int floatSize = Marshal.SizeOf(typeof(float));
        private CameraController cameraController;
        SeaShader seaShader;
        Sea sea;
        SkyBox skybox;
        SkyBoxShader skyBoxShader;

        public MainWindow()
            : base(1024, 640,new GraphicsMode(new ColorFormat(24),16,0,2))
        {
            boatShader  = new ModelShader("ModelShader");
            seaShader = new SeaShader("SeaShader");
            skyBoxShader = new SkyBoxShader("SkyBoxShader");
            boatShader.theCamera = seaShader.theCamera = skyBoxShader.theCamera = new Camera(Width, Height, new Vector3(-17.06843f, -6.339379f, -28.01191f), new Vector3(0.2402344f, -11.31787f, 0f));// new Vector3(-39.28024f, -32.6281f, -43.21751f), new Vector3(1.0644f,-0.6015f,0f));
            cameraController = new CameraController(boatShader.theCamera, this);
            

          
            stopWatch = new Stopwatch();
            stopWatch.Start();
            VSync = VSyncMode.On;

            Font font = new Font(FontFamily.GenericSansSerif, 12.0f);

            

        }

    


        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Exit();

            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
        }

       

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);
            boatShader.Compile();
            seaShader.Compile();
            skyBoxShader.Compile();
            ship = new Ship(this);
            skybox = new SkyBox();
            ship.Position += new Vector3(100f, 8f, 100f);
            sea = new Sea();
            sea.Populate();

        }

    


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }



        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Title = cameraController.myCamera.Rotation.ToString() + " " + cameraController.myCamera.Position.ToString();
            //Matrix4 transform = Matrix4.Mult(Matrix4.CreateTranslation(new Vector3(0, 0, -10)), Matrix4.CreateRotationY(GetTime()));
            //boatShader.ModelMatrix = transform;
            //ship.Position += new Vector3(1f, 0f, 0f) / 600f;
            //ship.Rotation += new Vector3(1f, 2f, 3f) / 6000f;
            cameraController.OnUpdate();
            ship.OnUpdate();
            //ship.Position += ship.Forward/60f;
            //ship.Rotation += new Vector3(0, ship.Forward.X, 0);
            seaShader.time = GetTime();
            ship.time = GetTime();
        }

    

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
           
            ship.Render(boatShader);
            //seaShader.Begin(true, BeginMode.Triangles, true);
            sea.Render(seaShader);
            //skyBoxShader.Begin(false, BeginMode.Quads, true);

            //skyBoxShader.setModelMatrix(Matrix4.Identity);
            skybox.Render(skyBoxShader);
            //skyBoxShader.End(true);
            this.SwapBuffers();
        }

        public float GetTime()
        {
            return (float)stopWatch.Elapsed.TotalSeconds;
        }



     
        [STAThread]
        public static void Main()
        {
            using (MainWindow example = new MainWindow())
            {
                      example.Run();
            }
        }


    }
}
