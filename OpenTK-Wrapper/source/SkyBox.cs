using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace SevenSeasProject
{
    class SkyBox
    {
        static VertexBufferObject myVBO;
        static Texture frontTexture;
        static Texture backTexture;
        static Texture bottomTexture;
        static Texture topTexture;
        static Texture leftTexture;
        static Texture rightTexture;
        const float size = 100f;
        static float[] faces = 
        {
            -size, -size, -size, 1, 1,
            -size, size, -size, 1, 0,
             size, size, -size, 0, 0,
            size, -size, -size, 0, 1,

            -size, -size, size, 1, 1,
            -size, size, size, 1, 0,
            size, size, size, 0, 0,
            size, -size, size, 0, 1,

   
            -size, -size, -size, 1, 1,
            -size, size, -size, 1, 0,
             -size, size, size, 0, 0,
            -size, -size, size, 0, 1,

            size, -size, -size, 1, 1,
            size, size, -size, 1, 0,
            size, size, size, 0, 0,
            size, -size, size, 0, 1,

       
            -size, size, -size, 1, 1,
            -size, size, size, 1, 0,
            size, size, size, 0, 0,
            size, size, -size, 0, 1,

        
            -size, -size, -size, 1, 1,
            -size, -size, size, 1, 0,
            size, -size, size, 0, 0,
            size, -size, -size, 0, 1,

        };

        public SkyBox()
        {
            myVBO = new VertexBufferObject(5);
            frontTexture = new Texture("fronttexture.png");
            frontTexture.LoadTexture();
            backTexture = new Texture("backtexture.png");
            backTexture.LoadTexture();
            rightTexture = new Texture("righttexture.png");
            rightTexture.LoadTexture();
            leftTexture = new Texture("lefttexture.png");
            leftTexture.LoadTexture();
            topTexture = new Texture("toptexture.png");
            topTexture.LoadTexture();
            bottomTexture = new Texture("bottomtexture.png");
            bottomTexture.LoadTexture();
            Generate();
        }

        public void Generate()
        {
            
            myVBO.SetData(faces);
        }

        public void Render(Shader s)
        {
            s.SetTexture("tex", frontTexture);
            myVBO.Render(s, OpenTK.Graphics.OpenGL.BeginMode.Quads, 0, 4);
            s.SetTexture("tex", backTexture);
            myVBO.Render(s, OpenTK.Graphics.OpenGL.BeginMode.Quads, 4, 4 );
            s.SetTexture("tex", rightTexture);
            myVBO.Render(s, OpenTK.Graphics.OpenGL.BeginMode.Quads, 8, 4);
            s.SetTexture("tex", leftTexture);
            myVBO.Render(s, OpenTK.Graphics.OpenGL.BeginMode.Quads, 12, 4);
            s.SetTexture("tex", topTexture);
            myVBO.Render(s, OpenTK.Graphics.OpenGL.BeginMode.Quads, 16, 4);
            s.SetTexture("tex", bottomTexture);
            myVBO.Render(s, OpenTK.Graphics.OpenGL.BeginMode.Quads, 20, 4);
            
        }

    }
}
