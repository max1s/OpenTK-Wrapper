using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace SevenSeasProject
{
    class Ship
    {
        static ObjectFile obj;
        Vector4 color;
        private GameWindow myGameWindow;
        public float time = 1.0f;

        private Matrix4 myTransformation;
        private Vector3 myPosition;
        private Vector3 myRotation;

        Texture woodTexture;
        Texture coverTexture;
        Texture sailTexture;

        static ObjectFile.ObjectGroup sail;
        static ObjectFile.ObjectGroup cover;
        static ObjectFile.ObjectGroup wheel;
        static ObjectFile.ObjectGroup boat;


        private bool myMatrixChange = true;

        public Ship(GameWindow gameWindow)
        {
            if (obj == null)
            {
                obj = ObjectFile.ObjectFromFile("boatFinal");
                sail = obj.FindGroupByName("Sail");
                cover = obj.FindGroupByName("Cover");
                wheel = obj.FindGroupByName("Wheel");
                boat = obj.FindGroupByName("Boat");
            }
            myGameWindow = gameWindow;
            woodTexture = new Texture("woodtexture.jpg");
            coverTexture = new Texture("covertexture.jpg");
            sailTexture = new Texture("sailtexture.jpg");

            woodTexture.LoadTexture();
            coverTexture.LoadTexture();
            sailTexture.LoadTexture();
        }

        public Vector3 Position
        {
            get
            {
                return myPosition;
            }
            set
            {
                myPosition = value;
                myMatrixChange = true;
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return myRotation;
            }
            set
            {
                myRotation = value;
                myMatrixChange = true;
            }

        }

        public Vector3 Right
        {
            get
            {
                float cosYaw = (float)Math.Cos(-Rotation.Y);
                float sinYaw = (float)Math.Sin(-Rotation.Y);
                float cosPitch = (float)Math.Cos(Rotation.X);
                float sinPitch = (float)Math.Sin(Rotation.X);
                return new Vector3(sinYaw, sinPitch, cosYaw * cosPitch);
            }
        }

        public Vector3 Forward
        {
            get
            {
                float cosYaw = (float)Math.Cos(Rotation.Y);
                float sinYaw = (float)Math.Sin(Rotation.Y);
                float cosPitch = (float)Math.Cos(Rotation.X);
                float sinPitch = (float)Math.Sin(Rotation.X);
                return new Vector3(cosYaw,0f , -sinYaw);
            }
        }
       
        public Matrix4 Transformation
        {
            get
            {
                if (myMatrixChange)
                    UpdateMatrix();
                return myTransformation;
            }
        }

        private void UpdateMatrix()
        {
            myTransformation = Matrix4.Scale(0.25f);
            myTransformation = Matrix4.Mult(Matrix4.CreateTranslation(myPosition), myTransformation);
            myTransformation = Matrix4.Mult(Matrix4.CreateRotationY(Rotation.Y), myTransformation);
            myTransformation = Matrix4.Mult(Matrix4.CreateRotationX(Rotation.X), myTransformation);
            myTransformation = Matrix4.Mult(Matrix4.CreateRotationZ(Rotation.Z), myTransformation);
            myMatrixChange = false;

        }

        public void Render(TranslatableShader s)
        {
            s.setModelMatrix(Transformation);
            s.SetTexture("tex", sailTexture);
            obj.Render(s, sail);
            s.SetTexture("tex", woodTexture);
            obj.Render(s, boat);
            s.SetTexture("tex", coverTexture);
            obj.Render(s, cover);
            s.SetTexture("tex", coverTexture);
            float wheelRotation = time * 2.0f;
            s.setModelMatrix(Matrix4.Mult(Matrix4.CreateRotationX(wheelRotation), Matrix4.Mult(Matrix4.CreateTranslation(0f, 3.60631f, 0f), myTransformation)));
            obj.Render(s,  wheel);
        }

        public void OnUpdate()
        {
            this.Position += new Vector3(0f, (float)Math.Sin(time)/55f, 0f);
            this.Rotation += new Vector3((float)Math.Sin(time*2.5f) / 500f, 0f, 0f);
            if (myGameWindow.Keyboard[Key.Up])
                myPosition += this.Forward/20f;
            if (myGameWindow.Keyboard[Key.Left])
                myRotation += new Vector3(0f, 0.8f, 0f)/250f;
            if (myGameWindow.Keyboard[Key.Right])
                myRotation += new Vector3(0f, -0.8f, 0f)/250f;

        }
    }

}
