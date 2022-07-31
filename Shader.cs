using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Engine {
    class Shader {
        private int handler;
        public int Handler {
            get =>  handler;
        }
        public Shader(string vertexShader, string fragmentShader) {
            string vertexSource, fragmentSource;
            
            //Read Shaders from file
            using (StreamReader reader = new StreamReader(vertexShader, Encoding.UTF8)) {
                vertexSource = reader.ReadToEnd();
                reader.Close();
            }
            using (StreamReader reader = new StreamReader(fragmentShader, Encoding.UTF8)) {
                fragmentSource = reader.ReadToEnd();
                reader.Close();
            }
            
            //Compile vertex shader from source code
            int vertShaderHandler = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertShaderHandler, vertexSource);
            GL.CompileShader(vertShaderHandler);

            //Check if vertex shader has compiled successfully
            int success;
            GL.GetShader(vertShaderHandler, ShaderParameter.CompileStatus, out success);
            if (success == 0) {
                string infoLog = GL.GetShaderInfoLog(vertShaderHandler);
                Console.WriteLine(infoLog);
            }

            //Compile fragment shader from source code
            int fragShaderHandler = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragShaderHandler, fragmentSource);
            GL.CompileShader(fragShaderHandler);

            //Check if fragment shader has compiled successfully
            GL.GetShader(fragShaderHandler, ShaderParameter.CompileStatus, out success);
            if (success == 0) {
                string infoLog = GL.GetShaderInfoLog(fragShaderHandler);
                Console.WriteLine(infoLog);
            }

            //Attach and Link program
            handler = GL.CreateProgram();
            GL.AttachShader(handler, vertShaderHandler);
            GL.AttachShader(handler, fragShaderHandler);
            
            GL.LinkProgram(handler);

            GL.GetProgram(handler, GetProgramParameterName.LinkStatus, out success);
            if (success == 0) {
                string infoLog = GL.GetProgramInfoLog(handler);
                Console.WriteLine(infoLog);
            }

            //Delete shaders 
            GL.DetachShader(handler, vertShaderHandler);
            GL.DetachShader(handler, fragShaderHandler);
            GL.DeleteShader(vertShaderHandler);
            GL.DeleteShader(fragShaderHandler);
        }
        public void Use() {
            GL.UseProgram(handler);
        }

        ~Shader() {
            GL.DeleteProgram(handler);
        }
    }
}