﻿using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
	public class OpenTKApp : GameWindow
	{
		static int screenID;
        //static int mapID;
        static Game game;
		static bool terminated = false;
		protected override void OnLoad( EventArgs e )
		{
			// called upon app init
            ////////////////verandering part 2
			//GL.ClearColor( Color.Black );
			//GL.Enable( EnableCap.Texture2D );
			//GL.Disable( EnableCap.DepthTest );
			GL.Hint( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );
			ClientSize = new Size( 640, 400 );
			game = new Game();
			game.screen = new Surface( Width, Height );
			Sprite.target = game.screen;
			screenID = game.screen.GenTexture();
            //test voor map deel 2
            //game.map = new Surface(Width, Height);
            //Sprite.target = game.map;
            //mapID = game.map.GenTexture();
            game.Init();
		}
		protected override void OnUnload( EventArgs e )
		{
			// called upon app close
			GL.DeleteTextures( 1, ref screenID );
            //GL.DeleteTextures(1, ref mapID);
            Environment.Exit( 0 ); // bypass wait for key on CTRL-F5
		}
		protected override void OnResize( EventArgs e )
		{
			// called upon window resize
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( -1.0, 1.0, -1.0, 1.0, 0.0, 4.0 );
		}
		protected override void OnUpdateFrame( FrameEventArgs e )
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();
		}
		protected override void OnRenderFrame( FrameEventArgs e )
		{
            // called once per frame; render
            ////part2///toevoeging voor game.tick
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Color3(1.0f, 1.0f, 1.0f);
            game.Tick();
			if (terminated) 
			{
				Exit();
				return;
			}
			// convert Game.screen to OpenGL texture
			GL.BindTexture( TextureTarget.Texture2D, screenID );
			GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
						   game.screen.width, game.screen.height, 0, 
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
						   PixelType.UnsignedByte, game.screen.pixels 
						 );
            //test voor map deel 2
            //GL.BindTexture(TextureTarget.Texture2D, mapID);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            //   game.map.width, game.map.height, 0,
            //   OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
            //   PixelType.UnsignedByte, game.map.pixels
            // );
            // clear window contents
            GL.Clear( ClearBufferMask.ColorBufferBit);
            // setup camera
            GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
            // draw screen filling quad
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
            GL.End();
            // tell OpenTK we're done rendering
            /////////part2// prepare for generic OpenGL rendering
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Texture2D);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            game.RenderGL();
            SwapBuffers();
		}
		public static void Main( string[] args ) 
		{ 
			// entry point
			using (OpenTKApp app = new OpenTKApp()) { app.Run( 30.0, 0.0 ); }
		}
	}
}