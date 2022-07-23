using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Engine {
    public class Program {
        public static int Main() {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();
            nativeWindowSettings.Size = new Vector2i(800, 600);
            nativeWindowSettings.Title = "Testing";


            using (Window window = new Window(GameWindowSettings.Default, nativeWindowSettings)) {
                window.Run();
            }
            return 0;
        }
    }
}

