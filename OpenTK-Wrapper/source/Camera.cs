using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SevenSeasProject
{
    class Camera
    {


        public Matrix4 PerspectiveMatrix;
        private Matrix4 myViewMatrix;


        public int Width;
        public int Height;

        private Vector3 myPosition;
        private Vector3 myRotation;

        public Vector3 Forward
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

        public Vector3 Right
        {
            get
            {
                float cosYaw = (float)Math.Cos(Rotation.Y);
                float sinYaw = (float)Math.Sin(Rotation.Y);
                float cosPitch = (float)Math.Cos(Rotation.X);
                float sinPitch = (float)Math.Sin(Rotation.X);
                return -new Vector3(cosYaw, 0, sinYaw);
            }
        }
        public Vector3 Up
        {
            get
            {
                float cosYaw = (float)Math.Cos(Rotation.Y);
                float sinYaw = (float)Math.Sin(Rotation.Y);
                float cosPitch = (float)Math.Cos(Rotation.X);
                float sinPitch = (float)Math.Sin(Rotation.X);
                return new Vector3(0, 1, 0);
            }
        }
        private bool myMatrixChange = true;

        public Camera(int width, int height, Vector3 pos, Vector3 rot)
        {
            Width = width;
            Height = height;
            PerspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, (float)width / height, 1 / 64f, 300f);
            Position = pos;
            Rotation = rot;
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
        public Matrix4 ViewMatrix
        {
            get
            {
                if (myMatrixChange)
                    UpdateMatrix();
                return myViewMatrix;
            }
        }

        private void UpdateMatrix()
        {
            myRotation.X = Math.Min(MathHelper.PiOver2, Math.Max(-MathHelper.PiOver2, Rotation.X));
            myViewMatrix = Matrix4.CreateRotationX(Rotation.X);
            myViewMatrix = Matrix4.Mult(Matrix4.CreateRotationY(Rotation.Y), myViewMatrix);
            myViewMatrix = Matrix4.Mult(Matrix4.CreateTranslation(myPosition), myViewMatrix);
            myMatrixChange = false;

        }
    }
}
