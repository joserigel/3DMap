using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace Engine {
    public class Window : GameWindow 
    {
        private float[] vertices = 
        {
            //Coordinates           //UV            //Normals
            -0.5f, -0.5f, 0.0f,     0.0f, 0.0f,     0.0f, 0.0f, 1.0f,
             0.5f, -0.5f, 0.0f,     1.0f, 0.0f,     0.0f, 0.0f, 1.0f,
            -0.5f,  0.5f, 0.0f,     0.0f, 1.0f,     0.0f, 0.0f, 1.0f,
             0.5f,  0.5f, 0.0f,     1.0f, 1.0f,     0.0f, 0.0f, 1.0f,
        };
        private uint[] indices = 
        {
            0, 3, 1,
            0, 2, 3
        };
        private Geometry sphere;
        private float angle;
        private int vao, vbo, ebo;
        private Shader shader;
        private Texture colorMap, bumpMap;
        private Camera camera;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) 
        {
            
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            CursorState = CursorState.Grabbed;

            //Generate Sphere
            sphere = Geometry.CreateSphere(5.0f, 64, 64);
            vertices = sphere.Vertices;
            indices = sphere.Indices;

            //Set Clear Color
            GL.ClearColor(0.1f, 0.15f, 0.2f, 1.0f);

            //Generate Element Buffer Object
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 
                sizeof(uint) * indices.Length, 
                indices, BufferUsageHint.StaticDraw);
            
            //Generate Vertex Buffer Object
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length,
                vertices, BufferUsageHint.StaticDraw);

            //Generate Vertex Array Object
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            //Set Texture
            colorMap = new Texture("images/color_2k.jpg"); 
            bumpMap = new Texture("images/bump_8k.jpg"); 
            
            //Instantiate Shader
            shader = new Shader("shaders/default.vert", "shaders/default.frag");
            GL.UseProgram(shader.Handler);

            //Instaniate Camera
            camera = new Camera(this.Size.X, this.Size.Y);
            Matrix4 projection = camera.CameraMatrix;
            Matrix4 view = camera.View;
            Matrix4 model = Matrix4.Identity;
            
            //Set Textures
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, colorMap.Handler);
            int location = GL.GetUniformLocation(shader.Handler, "colorMap"); 
            GL.Uniform1(location, 0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, bumpMap.Handler);
            location = GL.GetUniformLocation(shader.Handler, "bumpMap");
            GL.Uniform1(location, 1);
            
            //Set Uniforms
            location = GL.GetUniformLocation(shader.Handler, "projection");
            GL.UniformMatrix4(location, true, ref projection);
            location = GL.GetUniformLocation(shader.Handler, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(shader.Handler, "model");
            GL.UniformMatrix4(location, true, ref model);

            //Set Uniform Constants
            location = GL.GetUniformLocation(shader.Handler, "bumpIntensity");
            GL.Uniform1(location, 0.1f);

            //Set Light Direction
            angle = 45f;
            Vector3 lightDir = new Vector3(
                MathF.Cos(angle * MathF.PI / 180f),
                0f,
                MathF.Sin(angle * MathF.PI / 180f)
            );
            location = GL.GetUniformLocation(shader.Handler, "lightDir");
            GL.Uniform3(location, ref lightDir);
            
            //Enable Depth Test
            GL.Enable(EnableCap.DepthTest);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            Matrix4 projection = camera.CameraMatrix;
            Matrix4 view = camera.View;

            //Update View
            int location = GL.GetUniformLocation(shader.Handler, "projection");
            GL.UniformMatrix4(location, true, ref projection);
            location = GL.GetUniformLocation(shader.Handler, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(shader.Handler, "model");

            //Calculate Sun Direction
            angle += 15f * (float)args.Time;
            while (angle >= 360f) angle -= 360f;
            while (angle < 0) angle += 360f;
            Vector3 lightDir = new Vector3(
                MathF.Cos(angle * MathF.PI / 180f),
                0f,
                MathF.Sin(angle * MathF.PI / 180f)
            );
            location = GL.GetUniformLocation(shader.Handler, "lightDir");
            GL.Uniform3(location, ref lightDir);

            //Closes when escape key is pressed
            if (KeyboardState.IsKeyDown(Keys.Escape)) {
                Close();
                
            }
            camera.Update(KeyboardState, MouseState, (float)args.Time);
            
            
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.UseProgram(shader.Handler);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer,  ebo);
            GL.BindVertexArray(vao);
            
            
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            

            
            
            
                
            Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}