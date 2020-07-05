using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace SevenSeasProject
{
    class Sea
    {
        static VertexBufferObject myVBO;
        public Vector3[] seaMesh;
        Texture seaTexture;

        public Sea()
        {
            myVBO = new VertexBufferObject(3);
            CreateSeaMesh(100, 500);
            seaTexture = new Texture("seatexture.jpg");
            seaTexture.LoadTexture();
        }

        public void CreateSeaMesh(float size, int noOfSquares)
        {
            seaMesh = new Vector3[noOfSquares * noOfSquares * 4];
            int ks = 0;
            float stride = size / noOfSquares;
            while (ks < seaMesh.Length)
            {
                for (int i = 0; i < noOfSquares; ++i )
                {
                    for (int j = 0; j < noOfSquares; ++j )
                    {
                        seaMesh[ks++] = new Vector3(j, 0, i)*stride;
                        seaMesh[ks++] = new Vector3(j + 1, 0, i)*stride;
                        seaMesh[ks++] = new Vector3(j + 1, 0, i + 1)*stride;
                        seaMesh[ks++] = new Vector3(j, 0, i + 1)*stride;
                  
                    }
                }
            }

        }
        
        public void Populate()
        {
            myVBO.SetData(seaMesh);
        }

        public void Render(SeaShader s)
        {
            //s.Render(theSea);
            s.SetTexture("waves", seaTexture);
            myVBO.Render(s, BeginMode.Quads);
        }
    }
}
