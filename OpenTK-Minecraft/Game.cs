using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_Minecraft;

public class Game : GameWindow
{
     float[] vertices =
    {
        0f, 0.5f, 0f, //top vertex
        -0.5f, -0.5f, 0f, //bottom left vertex
        0.5f, -0.5f, 0f, //bottom right vertex
    };
     
     //Render pipeline variables
      int VAO;
      int shaderProgram;
    
    int width, height; 
    
    public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        this.width = width;
        this.height = height;
        //center window
        CenterWindow(new Vector2i(width,height));
    }
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0,0,e.Width,e.Height);
        this.width = e.Width;
        this.height = e.Height;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        VAO = GL.GenVertexArray();

        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //unbind vbo
    }

    protected override void OnUnload()
    {
        base.OnUnload();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        
        GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        Context.SwapBuffers();
       
        base.OnRenderFrame(args);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }
}