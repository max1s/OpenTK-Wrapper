using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace SevenSeasProject
{
    class ObjectFile
    {
        Vector3[] verts;
        Vector3[] normals;
        Vector2[] uvs;
        public VertexBufferObject vbo;
        ObjectGroup[] objGroups;
        Face[] faces;

        enum VERTEX_DATA_TYPE { POSITION = 0, UV = 1, NORMAL = 2 }

        struct Face
        {
            public Face(int[][] ind)
            {
                Indices = ind;
                
            }
            public int[][] Indices;
        }

        

        private ObjectFile()
        {
            vbo = new VertexBufferObject(8);
        }

        public class ObjectGroup
        {
            public String objectName;
            public int startPosition;
            public int endPosition;

            public ObjectGroup(String name, int start, int end)
            {
                objectName = name;
                startPosition = start;
                endPosition = end;
            }
        }
        
        public static ObjectFile ObjectFromFile(String fileName)
        {
            String[] lines;
            String currentLine;
            lines = File.ReadAllLines(fileName + ".obj");
            ObjectFile file = new ObjectFile();
            int vertexCount = 0;
            int facesCount = 0;
            int objectCount = 0;
            int uvCount = 0;
            int normalCount = 0;
            for (int i = 0; i < lines.Count(); i++)
            { 
                currentLine = lines[i];
                if(currentLine.StartsWith("v ")){vertexCount++; continue;}
                if(currentLine.StartsWith("vn ")){normalCount++;continue;}
                if(currentLine.StartsWith("o ")){objectCount++;continue;}
                if(currentLine.StartsWith("vt ")){uvCount++;continue;}
                if(currentLine.StartsWith("f ")){facesCount++;continue;}
            }

            file.verts = new Vector3[vertexCount];
            file.normals = new Vector3[normalCount];
            file.uvs = new Vector2[uvCount];
            file.objGroups = new ObjectGroup[objectCount];
            file.faces = new Face[facesCount];

            bool start = true;
            int j = 0, k = 0, l = 0, m = 0, n = 0;

            for (int i = 0; i < lines.Count(); i++)
            {
                currentLine = lines[i];
                if (currentLine.StartsWith("v ")) 
                {
                    String[] components = currentLine.Split(' '); 
                    file.verts[j++] = new Vector3(Single.Parse(components[1]), Single.Parse(components[2]), Single.Parse(components[3]));
                    continue;
                }
                if (currentLine.StartsWith("vn "))
                {
                    String[] components = currentLine.Split(' ');
                    file.normals[k++] = new Vector3(Single.Parse(components[1]), Single.Parse(components[2]), Single.Parse(components[3]));
                    continue;
                }
                if (currentLine.StartsWith("o "))
                { 
                    String[] components = currentLine.Split(' ');
                   
                    if(!start)
                    {
                        file.objGroups[l++].endPosition = n * 3;
                    }
                    start = false;
                    file.objGroups[l] = new ObjectGroup(components[1], n * 3, 0);
                    continue;
                }
                if (currentLine.StartsWith("vt ")) 
                {
                    String[] components = currentLine.Split(' ');
                    file.uvs[m++] = new Vector2(Single.Parse(components[2]), Single.Parse(components[1]));
                    continue;
                }
                if (currentLine.StartsWith("f ")) 
                {
                    int[][] components = currentLine.Split(' ').Where((y, x) => x > 0).Select(y => y.Split('/').Select(z => Int32.Parse("0" + z)).ToArray()).ToArray();
                    file.faces[n++] = new Face(components);
                    continue;
                }
            }
            file.objGroups[l++].endPosition = n * 3;
            file.FillVBO();
            return file;

        }

        public void FillVBO()
        {
            List<float> data = new List<float>();
            foreach (Face f in faces)
            {

                for (int i = 0; i < 3; ++i)
                {
                    data.Add(verts[f.Indices[i][(int)VERTEX_DATA_TYPE.POSITION] - 1]);
                    data.Add(normals[f.Indices[i][(int)VERTEX_DATA_TYPE.NORMAL] - 1]);
                    int uvIndex = f.Indices[i][(int)VERTEX_DATA_TYPE.UV] - 1;
                    if (uvIndex >= 0)
                        data.Add(uvs[uvIndex]);
                    else
                        data.Add(Vector2.Zero);
                }
            }
            vbo.SetData(data.ToArray());

        }

        public void Render(TranslatableShader shader, params ObjectGroup[] args)
        {
            if (args.Length == 0)
            {
                args = objGroups;
            }

            foreach(ObjectGroup objGroup in args)
            {
                vbo.Render(shader, BeginMode.Triangles, objGroup.startPosition, objGroup.endPosition - objGroup.startPosition);
            }
        }

        public ObjectGroup FindGroupByName(String s)
        {
            return objGroups.First(x => x.objectName.StartsWith(s));
        }

    }
}
