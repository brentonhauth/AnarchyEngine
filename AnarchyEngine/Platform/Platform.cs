using AnarchyEngine.Core;
using AnarchyEngine.Rendering;
using AnarchyEngine.Platform.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Platform {
    internal static class Platform {
        public static IWindow CreateWindow(API api, GameSettings settings) {
            switch (api) {
                case API.OpenGL:
                    return new GLWindow(settings.Name, settings.Width, settings.Height);
                case API.None:
                default:
                    throw new Exception();
            }
        }

        public static IRunner CreateRunner(API api, GameSettings settings) {
            
            switch (api) {
                case API.OpenGL:
                    return Application.Instance.Window as GLWindow;
                case API.None:
                default:
                    throw new Exception();
            }
        }
    }
}
