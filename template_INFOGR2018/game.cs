using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template {

    class Game
    {
        // member variables
        public Surface screen;
        int MiddenY;
        int MiddenX;
        float Timer;
        float x1, x2, x3, x4;
        float y1, y2, y3, y4;
        public Surface map;
        float[,] h;
        float[] vertexData;
        int VBO;
        int programID;
        int vsID;
        int fsID;
        int attribute_vpos;
        int attribute_vcol;
        int uniform_mview;
        int vbo_pos;
        int vbo_col;
        // initialize
        public void Init()
        {
            //geeft je het midden van het scherm
            MiddenX = screen.width / 2;
            MiddenY = screen.height / 2;
            // we willen de cubes in het midden, dus je moet de helft van de 4kant eraf trekken.
            MiddenX -= 256 / 2;
            MiddenY -= 256 / 2;
            // deze is voor de lopendevierkant
            Timer = 0.0f;
            x1 = -1; x2 = 1; x3 = 1; x4 = -1f;
            y1 = 1; y2 = 1; y3 = -1; y4 = -1f;
            //deze is voor de heightmap
            map = new Surface("C:/Users/NeanderG/Desktop/zeergeheimmapje/GRAPHICS/template_INFOGR2018/assets/heightmap.png");
            h = new float[128, 128];
            for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
                    h[x, y] = ((float)(map.pixels[x + y * 128] & 255)) / 256;

            vertexData = new float[127 * 127 * 2 * 3 * 3];
            float offset = 0.01f;
            float k = 5.0f;
            int v_teller = 0;
            //dit is sneller hoisten heet het?
            // ja man die array vullen kan ook hier, als het goed is
            for (int j = 0; j < 127; j++) for (int i = 0; i < 127; i++)
                {
                    //driehoek 1
                    float x = (((float)i / 128) - 0.5f) + offset;
                    vertexData[v_teller] = x;
                    v_teller++;
                    float y = (((float)j / 128) - 0.5f) + offset;
                    vertexData[v_teller] = y;
                    v_teller++;
                    float z = (((float)h[i, j]) / k) * -1;
                    vertexData[v_teller] = z;
                    v_teller++;
                    //driehoek 2
                    x = (((float)i / 128) - 0.5f);
                    vertexData[v_teller] = x;
                    v_teller++;
                    y = (((float)j / 128) - 0.5f) + offset;
                    vertexData[v_teller] = y;
                    v_teller++;
                    z = (((float)h[i, j]) / k) * -1;
                    vertexData[v_teller] = z;
                    v_teller++;
                    //driehoek 3
                    x = (((float)i / 128) - 0.5f) + offset;
                    vertexData[v_teller] = x;
                    v_teller++;
                    y = (((float)j / 128) - 0.5f);
                    vertexData[v_teller] = y;
                    v_teller++;
                    z = (((float)h[i, j]) / k) * -1;
                    vertexData[v_teller] = z;
                    v_teller++;
                    //driehoek 4
                    x = (((float)i / 128) - 0.5f);
                    vertexData[v_teller] = x;
                    v_teller++;
                    y = (((float)j / 128) - 0.5f);
                    vertexData[v_teller] = y;
                    v_teller++;
                    z = (((float)h[i, j]) / k) * -1;
                    vertexData[v_teller] = z;
                    v_teller++;
                    //driehoek 5
                    x = (((float)i / 128) - 0.5f) + offset;
                    vertexData[v_teller] = x;
                    v_teller++;
                    y = (((float)j / 128) - 0.5f);
                    vertexData[v_teller] = y;
                    v_teller++;
                    z = 0;
                    vertexData[v_teller] = z;
                    v_teller++;
                    //driehoek 6
                    x = (((float)i / 128) - 0.5f);
                    vertexData[v_teller] = x;
                    v_teller++;
                    y = (((float)j / 128) - 0.5f) + offset;
                    vertexData[v_teller] = y;
                    v_teller++;
                    z = 0;
                    vertexData[v_teller] = z;
                    v_teller++;
                }

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<float>(BufferTarget.ArrayBuffer,(IntPtr)(vertexData.Length * 4),vertexData,BufferUsageHint.StaticDraw);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 12, 0);
            
            //toegeveogd voor de shader
            programID = GL.CreateProgram();
            LoadShader("../../shaders/vs.glsl",
             ShaderType.VertexShader, programID, out vsID);
            LoadShader("../../shaders/fs.glsl",
             ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);            // acces to input varibles used in vertex shader            attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(programID, "vColor");
            uniform_mview = GL.GetUniformLocation(programID, "M");            vbo_pos = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_pos);
            GL.BufferData<float>(BufferTarget.ArrayBuffer,
             (IntPtr)(vertexData.Length * 4),
            vertexData, BufferUsageHint.StaticDraw
             );
            GL.VertexAttribPointer(attribute_vpos, 3,
             VertexAttribPointerType.Float,
            false, 0, 0
             );            vbo_col = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_col);
            GL.BufferData<float>(BufferTarget.ArrayBuffer,
             (IntPtr)(vertexData.Length * 4),
            vertexData, BufferUsageHint.StaticDraw
             );
            GL.VertexAttribPointer(attribute_vcol, 3,
             VertexAttribPointerType.Float,
            false, 0, 0
             );
        }

        //deze is ook toegevoegd om de shaders in te laden
        void LoadShader(string name, ShaderType type, int program, out int ID)
        {
            ID = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(name))
                GL.ShaderSource(ID, sr.ReadToEnd());
            GL.CompileShader(ID);
            GL.AttachShader(program, ID);
            Console.WriteLine(GL.GetShaderInfoLog(ID));
        }

        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffffff);
            screen.Line(2, 20, 160, 20, 0xff0000);
            Vierkant();
            DraaiendVierkant();

            Matrix4 M = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), Timer);
            M *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), 1.9f);
            M *= Matrix4.CreateTranslation(0, 0, -1);
            M *= Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
            GL.UseProgram(programID);
            GL.UniformMatrix4(uniform_mview, false, ref M);
        }
        // XOXO - lekker stukje code hoor - XOXO

        int TX(float x)
        {
            x *= (screen.width / 4);
            x += (screen.width / 2);
            return (int)x;
        }

        int TY(float y)
        {
            y *= (screen.height / 4);
            y += (screen.height / 2);
            return (int)y;
        }

        public void Vierkant()
        {
            for (int x = 1; x < 256; x++)
            {
                for (int y = 1; y < 256; y++)
                {
                    //tekenen van de vierkant. je hebt middenx en y nodig om de cubes te centreren
                    int location = (x + MiddenX) + (y + MiddenY) * screen.width;
                    int pixel = screen.pixels[location];
                    screen.pixels[location] = CreateColor(x, y, 255); //x * 256; // wow dan wordt hij groen man
                }
            }

            int CreateColor(int red, int green, int blue)
            {
                return (red << 16) + (green << 8) + blue;
            }
        }

        public void DraaiendVierkant()
        {
            Timer += 0.01f;

            float rx1 = (float)(x1 * Math.Cos(Timer) - y1 * Math.Sin(Timer));
            float ry1 = (float)(x1 * Math.Sin(Timer) + y1 * Math.Cos(Timer));

            float rx2 = (float)(x2 * Math.Cos(Timer) - y2 * Math.Sin(Timer));
            float ry2 = (float)(x2 * Math.Sin(Timer) + y2 * Math.Cos(Timer));

            float rx3 = (float)(x3 * Math.Cos(Timer) - y3 * Math.Sin(Timer));
            float ry3 = (float)(x3 * Math.Sin(Timer) + y3 * Math.Cos(Timer));

            float rx4 = (float)(x4 * Math.Cos(Timer) - y4 * Math.Sin(Timer));
            float ry4 = (float)(x4 * Math.Sin(Timer) + y4 * Math.Cos(Timer));

            //dus wat je doet is eigenlijk alleen de coordinatien van de lijnen verplaatsen naar de goede plek in de game window
            screen.Line(TX(rx1), TY(ry1), TX(rx2), TY(ry2), 0xffffff);
            screen.Line(TX(rx2), TY(ry2), TX(rx3), TY(ry3), 0xffffff);
            screen.Line(TX(rx3), TY(ry3), TX(rx4), TY(ry4), 0xffffff);
            screen.Line(TX(rx4), TY(ry4), TX(rx1), TY(ry1), 0xffffff);
        }

        //begin deel 2

        public void RenderGL()
        {
            float PI = (float)Math.PI;
            var M = Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
            GL.LoadMatrix(ref M);
            GL.Translate(0, 0, -1);
            GL.Rotate(110, 1, 0, 0);
            GL.Rotate(Timer * 180 / PI, 0, 0, 1);

            //GL.Color3(0.5f, 0.0f, 0.5f);
            GL.BindBuffer( BufferTarget.ArrayBuffer, VBO );
            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);
            GL.DrawArrays( PrimitiveType.Triangles, 0, 127 * 127 * 2 * 3 );

            //////////we gaan deze sneller maken
            //for (int j = 0; j < 128; j++) for (int i = 0; i < 128; i++)
            //    {
            //        //float z = h[i, j]; // x = i & y =j
            //        float z = ((float)h[i, j]);
            //        float x = ((float)i / 128) - 0.5f;
            //        float y = ((float)j / 128) - 0.5f;
            //        //Console.WriteLine(i);
            //        GL.Color3(z, 0.0f, 0.5f);
            //        //GL.Begin(PrimitiveType.Quads);
            //        //GL.Vertex3(x + offset, y + offset, -z / k);
            //        //GL.Vertex3(x, y + offset, -z / k);
            //        //GL.Vertex3(x, y, -z / k);
            //        //GL.Vertex3(x + offset, y, -z / k);
            //        //GL.End();

            //        ////maak die matherfakking 3hoek
            //        //GL.Begin(PrimitiveType.Triangles);
            //        //GL.Vertex3(x + offset, y + offset, -z / k);
            //        //GL.Vertex3(x, y + offset, -z / k);
            //        //GL.Vertex3(x + offset, y, -z / k);
            //        //GL.End();
            //        ////dupliceer die bitch
            //        //GL.Begin(PrimitiveType.Triangles);
            //        //GL.Vertex3(x, y, -z / k);
            //        //GL.Vertex3(x + offset, y, 0);
            //        //GL.Vertex3(x, y + offset, 0);
            //        //GL.End();

            //    }
        }
    }

} // namespace Template