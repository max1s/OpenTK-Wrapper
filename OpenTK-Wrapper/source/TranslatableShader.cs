using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenSeasProject
{
    class TranslatableShader : Shader
    {

        public Matrix4 ModelMatrix;

        int viewLocation;
        int perspectiveLocation;
        int modelLocation;
        int myViewVectorLocation;

        public Camera theCamera;

        public TranslatableShader(String name) : base(name)
        {

        }

       public class NoCameraException : Exception
       {
           public NoCameraException(String s) : base(s)
           {
           }
       }

       public override void OnCompile()
       {
           perspectiveLocation = GL.GetUniformLocation(ProgramID, "perspective");
           viewLocation = GL.GetUniformLocation(ProgramID, "view");
           modelLocation = GL.GetUniformLocation(ProgramID, "model");
           myViewVectorLocation = GL.GetUniformLocation(ProgramID, "viewNormal");
       }

       public override void OnBegin()
       {
            if (theCamera == null)
               throw new NoCameraException(GetType().FullName + " requires a camera");
            GL.UniformMatrix4(perspectiveLocation, false, ref theCamera.PerspectiveMatrix);
            Matrix4 viewMatrix = theCamera.ViewMatrix;
            GL.UniformMatrix4(viewLocation, false, ref viewMatrix);
            GL.UniformMatrix4(modelLocation, false, ref ModelMatrix);
            GL.Uniform3(myViewVectorLocation, theCamera.Forward);
           
       }
       public void setModelMatrix(Matrix4 modelMatrix)
       {
           ModelMatrix = modelMatrix;
           //GL.UniformMatrix4(modelLocation, false, ref ModelMatrix);
           CheckErrors();
       }
    }
}
