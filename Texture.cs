using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


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
        public Texture() 
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
            
            //Load and Mutate Texture
            Image<Rgba32> image = Image.Load<Rgba32>("texture.png");
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Get Bytes of img
            byte[] pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(pixels);
            
            //Set Image Data
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, image.Width, image.Height, 0, 
                PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            
        }
    }
}