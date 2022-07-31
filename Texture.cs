using OpenTK;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;


namespace Engine 
{    
    class Texture 
    {
        private static int count = 0;
        private int handler;
        private int unit;

        public int Handler 
        {
            get => handler;
        }
        public int Unit {
            get => unit;
        }
        public Texture(string path) 
        {
            //Generate and Bind Texture, set unit
            handler = GL.GenTexture();
            unit = count;
            count++;

            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            GL.BindTexture(TextureTarget.Texture2D, handler);
            
            //Set out of bounds behavior
            GL.TexParameter(TextureTarget.Texture2D, 
                TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);

            //Set scaling behavior
            GL.TexParameter(TextureTarget.Texture2D, 
                TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            //Get Image Bytes
            byte[] pixels;
            int width, height;
            using (FileStream stream = File.OpenRead(path)) {
                StbImage.stbi_set_flip_vertically_on_load(1);
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                pixels = image.Data;
                width = image.Width;
                height = image.Height;
            }
            
            //Set Image Data
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, width, height, 0, 
                PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            
        }
    }
}