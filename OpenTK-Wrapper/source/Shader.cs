using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SevenSeasProject
{
   

    public class Shader
    {
        String myFragmentShader;
        String myVertexShader;

        private int myProgramID;
        private int myVertexID;
        private int myFragmentID;

        private List<Attribute> myAttributeList = new List<Attribute>();
        private List<int> myBoundAttributes = new List<int>();

        private BeginMode myBeginMode;

        public Dictionary<String, TextureUniformInfo> currentTextures = new Dictionary<String, TextureUniformInfo>();

       public int ProgramID
        {
            get
            {
                if (myProgramID == 0)
                    myProgramID = GL.CreateProgram();
                return myProgramID;
            }

        }

        public int VertexID
        {
            get
            {
                if (myVertexID == 0)
                    myVertexID = GL.CreateShader(ShaderType.VertexShader);
                return myVertexID;
            }

        }

        public int FragmentID
        {
            get
            {
                if (myFragmentID == 0)
                    myFragmentID = GL.CreateShader(ShaderType.FragmentShader);
                return myFragmentID;
            }

        }

        public Shader(String name)
        {
            myFragmentShader = File.ReadAllText(name + ".frag.glsl");
            myVertexShader = File.ReadAllText(name + ".vert.glsl");
        }

        public void AddAttribute(Attribute anAttribute)
        {
            Debug.WriteLine("addAttrib: {0}, {1}", GetType().FullName, anAttribute.myAttributeName);
            myAttributeList.Add(anAttribute);
        }

       /// <summary>
       /// Here is where the compiling starts..
       /// </summary>
       /// <returns></returns>
        public bool Compile()
        {
            System.Diagnostics.Debug.WriteLine("compiling " + GetType().FullName);
            GL.ShaderSource(VertexID, myVertexShader);
            GL.ShaderSource(FragmentID, myFragmentShader);
            GL.CompileShader(VertexID);
            GL.CompileShader(FragmentID);
            
            String s = s = GL.GetShaderInfoLog(myVertexID);
            String t = GL.GetShaderInfoLog(myFragmentID);
            if (s.Length > 0 || t.Length > 0)
            {
                System.Diagnostics.Debug.WriteLine(s);
                System.Diagnostics.Debug.WriteLine(t);
                return false;
            }
           
            GL.AttachShader(ProgramID, VertexID);
            GL.AttachShader(ProgramID, FragmentID);
            GL.BindFragDataLocation(ProgramID, 0, "out_colour");
            GL.LinkProgram(ProgramID);
            String p;
            if ((p = GL.GetProgramInfoLog(ProgramID)).Length > 0)
            {
                System.Diagnostics.Debug.WriteLine(p);
                throw new Exception(p);

            }
            
            GL.UseProgram(ProgramID);
            OnCompile();
            foreach (Attribute attrib in myAttributeList)
            {
                myBoundAttributes.Add(GL.GetAttribLocation(ProgramID, attrib.myAttributeName));
            }

            GL.UseProgram(0);
            CheckErrors();
            return true;
        }

        public void Begin(bool immediateMode, BeginMode begin, bool depthtesting)
        {
            myBeginMode = begin;

            int floatSize = Marshal.SizeOf(typeof(float));


            GL.UseProgram(ProgramID);

            OnBegin();
            CheckErrors();

            GL.Enable(EnableCap.DepthTest);
            if (immediateMode)
                GL.Begin(myBeginMode);
            else
            {
                foreach (Attribute a in myAttributeList)
                {
                    GL.VertexAttribPointer(myBoundAttributes[myAttributeList.IndexOf(a)], a.mySize,
                        a.myPointerType, a.amINormal, a.myStride, a.myOffset);
                    CheckErrors();
                }

                foreach (int ba in myBoundAttributes)
                {
                    GL.EnableVertexAttribArray(ba);
                }
            }
        }

        public virtual void Render(float[] coords)
        {
            int i = 0;
            while (i < coords.Length)
            {
                foreach( Attribute a in myAttributeList)
                {
                    switch (a.mySize) 
                    {
                        case 1:
                            GL.VertexAttrib1(myBoundAttributes[myAttributeList.IndexOf(a)], coords[i++]);
                            break;
                        case 2:
                            GL.VertexAttrib2(myBoundAttributes[myAttributeList.IndexOf(a)], coords[i++], coords[i++]);
                            break;
                        case 3:
                            GL.VertexAttrib3(myBoundAttributes[myAttributeList.IndexOf(a)], coords[i++], coords[i++], coords[i++]);
                            break;
                        case 4:
                            GL.VertexAttrib4(myBoundAttributes[myAttributeList.IndexOf(a)], coords[i++], coords[i++], coords[i++], coords[i++]);
                            break;

                    }
                }
                
            }
        }

        public void End(bool immediateMode)
        {
            if (immediateMode)
                GL.End();
            OnEnd();
            foreach (int ba in myBoundAttributes)
            {
                GL.DisableVertexAttribArray(ba);
            }
            GL.Disable(EnableCap.DepthTest);
            GL.UseProgram(0);
        }

        public virtual void OnBegin()
        {
        }

        public virtual void OnCompile()
        {
        }

        public virtual void OnEnd()
        {
        }

        public void CheckErrors()
        {
            ErrorCode ec = GL.GetError();
            if (ec != ErrorCode.NoError)
            {
                //throw new Exception(ec.ToString());
            }
        }

        public void AddTexture(String uniformName, TextureUnit texUnit)
        {
            currentTextures.Add(uniformName, new TextureUniformInfo(this, uniformName, texUnit, GL.GetUniformLocation(ProgramID, uniformName), null));
        }

        public void SetTexture(String uniformName, Texture texture)
        {
            currentTextures[uniformName].SetCurrentTexture(texture);
        }
    }

    public class Attribute
    {
        public Shader myShader { get; private set; }
        public String myAttributeName { get; private set; }
        public VertexAttribPointerType myPointerType { get; private set; }
        public bool amINormal { get; private set; } //probably not
        public int myStride { get; private set; }
        public int myOffset { get; private set; }
        public int mySize { get; private set; }

        public Attribute(Shader shader, String attributeName, VertexAttribPointerType pointerType,
            bool isNormal, int stride, int offset, int size)
        {
            myShader = shader;
            myAttributeName = attributeName;
            myPointerType = pointerType;
            amINormal = isNormal;
            myStride = stride;
            myOffset = offset;
            mySize = size;
        }
    }
    public class TextureUniformInfo
    {
        public Shader myShader { get; private set; }
        public String myUniformName { get; private set; }
        public TextureUnit myTextureUnit { get; private set; }
        public int myUniformLocation { get; private set; }
        public Texture myCurrentTexture { get; private set; }

        public TextureUniformInfo(Shader shader, String uniformName, TextureUnit texUnit, int uniformLocation, Texture currentTexture)
        {
            myShader = shader;
            myUniformName = uniformName;
            myTextureUnit = texUnit;
            myUniformLocation = uniformLocation;
            myCurrentTexture = currentTexture;
            int texUnitValue = (int)texUnit - (int)TextureUnit.Texture0;
            GL.Uniform1(uniformLocation, texUnitValue);
        }


        public void SetCurrentTexture(Texture texture)
        {
            myCurrentTexture = texture;
            GL.ActiveTexture(myTextureUnit);
            texture.BindTexture();
        }

    }
}
