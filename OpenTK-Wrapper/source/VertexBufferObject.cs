using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Runtime.InteropServices;

namespace SevenSeasProject
{
    class VertexBufferObject
    {
        int myVBO_ID;
        int VBO_ID
        {
            get
            {
                if (myVBO_ID == 0)
                    GL.GenBuffers(1, out myVBO_ID);
                return myVBO_ID;
            }


        }

        int lengthOfData;
        readonly int Stride;

        public VertexBufferObject(int stride)
        {
            Stride = stride;
        }

        public void SetData<T>(T[] verts) where T : struct
        {
            int floatSize = Marshal.SizeOf(typeof(float));
            int unitSize = Marshal.SizeOf(typeof(T));
            lengthOfData = verts.Length* (unitSize/floatSize)/Stride;
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, new IntPtr(verts.Length * unitSize), verts, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            CheckForError();
        }

        public void Render(Shader s, BeginMode bm)
        {
            BeginMode beginMode = bm;
            CheckForError();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            s.Begin(false, beginMode, true);
            CheckForError();
            GL.DrawArrays(beginMode, 0, lengthOfData);
            CheckForError();
            s.End(false);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            CheckForError();
        }

        public void Render(Shader s, BeginMode bm, int begin, int end)
        {
            BeginMode beginMode = bm;
            CheckForError();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            s.Begin(false, beginMode, true);
            CheckForError();
            GL.DrawArrays(beginMode, begin, end);
            CheckForError();
            s.End(false);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            CheckForError();
        }

        private void CheckForError()
        {
            ErrorCode error = GL.GetError();

            //if (error != ErrorCode.NoError)
              //  throw new Exception("OpenGL hates your guts: " + error.ToString());
        }

    }
}
