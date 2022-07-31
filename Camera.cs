using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Engine
{
    class Camera
    {
        private static Vector3 up = Vector3.UnitY;
        private static Vector3 right = Vector3.UnitX;
        private float fov = 65, speed = 1.5f, sens = 0.1f;
        private float lookX, lookY;
        private Vector3 position, front;
        private Matrix4 cameraMatrix, view;
        public Matrix4 CameraMatrix {
            get => cameraMatrix;
        }
        public Matrix4 View{
            get => view;
        }
        public Camera(int width, int height) 
        {
            cameraMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(fov),
                (float)width/(float)height, 0.1f, 100f);
            position = new Vector3(0f, 0f, 3f);

            lookX = 0;
            lookY = 0;  

            front = new Vector3(0f, 0f, -1f);
            
            view = Matrix4.LookAt(position, position + front, up);
        }
        public void Update(KeyboardState keyboardState, MouseState mouseState, float deltaTime) 
        {
            if (keyboardState.IsKeyDown(Keys.W))
                position += front * speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.S))
                position -= front * speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D))
                position += right * speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.A))
                position -= right * speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.Space))
                position += up * speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.LeftControl))
                position -= up * speed * deltaTime;

            lookX += mouseState.Delta.X * sens * deltaTime;
            lookY += mouseState.Delta.Y * sens * deltaTime;

            lookY = Math.Clamp(lookY, -90f, 90f);

            Vector4 lookDir = -Vector4.UnitZ 
                * Matrix4.CreateFromAxisAngle(Vector3.UnitX, -lookY)
                * Matrix4.CreateFromAxisAngle(up, -lookX);
            front = new Vector3(
                lookDir.X,
                lookDir.Y,
                lookDir.Z
            );

            right = Vector3.Cross(front, up);
            

            view = Matrix4.LookAt(position, position + front, up);
                
        }
    }
}