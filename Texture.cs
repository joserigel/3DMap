using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace Engine {    
    class Texture {
        private int handler;

        public int Handler {
            get => handler;
        }
        public Texture() {
            //Generate and BInd Texture
            handler = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
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
            
            

            
            Image<Rgba32> image = Image.Load<Rgba32>("texture.png");
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            byte[] pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(pixels);

            int width = 64, height = 64;
            

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, image.Width, image.Height, 0, 
                PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.ActiveTexture(TextureUnit.Texture0);
        }
    }
}