using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace ParticleVoxel {
    public class Program {
        public static int Main() {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();
            Console.WriteLine(nativeWindowSettings.APIVersion.ToString());
            nativeWindowSettings.Size = new Vector2i(800, 600);
            nativeWindowSettings.Title = "Testing";


            using (Window window = new Window(GameWindowSettings.Default, nativeWindowSettings)) {
                window.Run();
            }
            return 0;
        }
    }
}

