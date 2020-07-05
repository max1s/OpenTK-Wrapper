using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SevenSeasProject
{
    class SkyBoxShader : TranslatableShader
    {
        int colorLocation;

        public SkyBoxShader(String name)
            : base(name)
        {
        }

        public override void OnCompile()
        {
            int floatSize = Marshal.SizeOf(typeof(float));
            base.OnCompile();
            colorLocation = GL.GetUniformLocation(ProgramID, "color");

            AddAttribute(new Attribute(this, "in_vertex", VertexAttribPointerType.Float, false, 5 * floatSize, 0 * floatSize, 3));
            AddAttribute(new Attribute(this, "in_uv", VertexAttribPointerType.Float, false, 5 * floatSize, 3 * floatSize, 2));
            AddTexture("tex", TextureUnit.Texture1);

        }

        public override void OnBegin()
        {
            base.OnBegin();
            // GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            GL.Uniform4(colorLocation, Color4.White);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            // GL.Disable(EnableCap.CullFace);
        }
    }
}
