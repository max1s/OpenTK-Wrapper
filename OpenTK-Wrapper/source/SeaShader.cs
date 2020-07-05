using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SevenSeasProject
{
    class SeaShader : TranslatableShader
    {

        public int timeLocation;
        public float time = 0.0f;

        public SeaShader(String s)
            : base(s)
        {
           
        }

        public override void OnCompile()
        {
            int floatSize = Marshal.SizeOf(typeof(float));
            base.OnCompile();
            
            AddAttribute(new Attribute(this, "in_vertex", VertexAttribPointerType.Float, false, 3 * floatSize, 0 * floatSize, 3));
            AddTexture("waves", TextureUnit.Texture0);
            timeLocation = GL.GetUniformLocation(ProgramID, "time");

        }

        public override void OnBegin()
        {
            base.OnBegin();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Uniform1(timeLocation, time);
            
            
        }

        public override void OnEnd()
        {
            base.OnEnd();
            GL.Disable(EnableCap.Blend);
        }
    }
}
