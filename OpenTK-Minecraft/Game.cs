using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Minecraft
{
    public class Game : GameWindow
    {
        float[] vertices =
        {
            0f, 0.5f, 0f, // top vertex
            -0.5f, -0.5f, 0f, // bottom left vertex
            0.5f, -0.5f, 0f // bottom right vertex
        };

        int VAO;
        int shaderProgram;
        int width, height;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            VAO = GL.GenVertexArray();
            int VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbind VBO
            GL.BindVertexArray(0); // unbind VAO

            // Create and compile shaders
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);
            CheckShaderCompileStatus(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);
            CheckShaderCompileStatus(fragmentShader);

            // Create shader program and link shaders
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            CheckProgramLinkStatus(shaderProgram);

            // Delete shaders as they are no longer needed after linking
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(VAO);
            GL.DeleteProgram(shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        public static string LoadShaderSource(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
                return string.Empty;
            }
        }

        void CheckShaderCompileStatus(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status != (int)All.True)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Shader compilation failed: {infoLog}");
            }
        }

        void CheckProgramLinkStatus(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status != (int)All.True)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Program linking failed: {infoLog}");
            }
        }
    }
}
