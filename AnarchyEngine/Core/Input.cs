using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Core {
    public static class Input {

        private static KeyboardState PrevKeyboardState, CurrentKeyboardState;

        private static MouseState CurrentMouseState;

        internal static void UpdateState() {
            PrevKeyboardState = CurrentKeyboardState;
            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();
        }


        public static void SetMousePosition(float x, float y) => Mouse.SetPosition(x, y);

        public static bool IsKeyDown(Key key) => CurrentKeyboardState.IsKeyDown(key);

        public static bool IsKeyUp(Key key) => CurrentKeyboardState.IsKeyUp(key);

        public static bool IsKeyPressed(Key key) {
            return IsKeyDown(key) && !PrevKeyboardState.IsKeyDown(key);
        }
    }
}
