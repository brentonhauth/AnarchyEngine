using AnarchyEngine.Util;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Core {
    public class Camera {
        private bool _firstMove = true;
        private Vector2 _lastPos;

        // Those vectors are directions pointing outwards from the camera to define how it rotated
        private Vector3 m_front = -Vector3.UnitZ,
                        m_up = Vector3.UnitY,
                        m_right = Vector3.UnitX;

        // Rotation around the X axis (radians)
        private float m_pitch;

        // Rotation around the Y axis (radians)
        // Without this you would be started rotated 90 degrees right
        private float m_yaw = -MathHelper.PiOver2;

        // The field of view of the camera (radians)
        private float m_fov = MathHelper.PiOver2;

        public Camera(Vector3 position, float aspectRatio) {
            Position = position;
            AspectRatio = aspectRatio;
        }

        // The position of the camera
        public Vector3 Position { get; set; }
        // This is simply the aspect ratio of the viewport, used for the projection matrix
        public float AspectRatio { get; set; }

        public Vector3 Front => m_front;
        public Vector3 Up => m_up;
        public Vector3 Right => m_right;

        public Matrix4 View =>
            Matrix4.LookAt(Position, Position + m_front, m_up);
        public Matrix4 Projection =>
            Matrix4.CreatePerspectiveFieldOfView(m_fov, AspectRatio, .01f, 100f);

        public Matrix4 ViewProjection => View * Projection;

        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Pitch {
            get => MathHelper.RadiansToDegrees(m_pitch);
            set {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, -89f, 89f);
                m_pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Yaw {
            get => MathHelper.RadiansToDegrees(m_yaw);
            set {
                m_yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        // The field of view (FOV) is the vertical angle of the camera view, this has been discussed more in depth in a
        // previous tutorial, but in this tutorial you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Fov {
            get => MathHelper.RadiansToDegrees(m_fov);
            set {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                m_fov = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 GetViewMatrix() {
            return Matrix4.LookAt(Position, Position + m_front, m_up);
        }

        // Get the projection matrix using the same method we have used up until this point
        public Matrix4 GetProjectionMatrix() {
            Matrix4.CreatePerspectiveFieldOfView(m_fov, AspectRatio, .01f, 100f, out Matrix4 m);
            // Console.WriteLine("=========\n" + m);
            return m;
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials
        private void UpdateVectors() {
            //Game.SkyBox = ColorHelper.RandColor;
            // First the front matrix is calculated using some basic trigonometry
            //m_front.X = Maths.Cos(m_pitch) * Maths.Cos(m_yaw);
            //m_front.Y = Maths.Sin(m_pitch);
            //m_front.Z = Maths.Cos(m_pitch) * Maths.Sin(m_yaw);

            m_front = Maths.CalculateFront(m_pitch, m_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results
            m_front = Vector3.Normalize(m_front);

            // Calculate both the right and the up vector using cross product
            // Note that we are calculating the right from the global up, this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera
            m_right = Vector3.Normalize(Vector3.Cross(m_front, Vector3.UnitY));
            m_up = Vector3.Normalize(Vector3.Cross(m_right, m_front));
        }

        public void Start() {

        }

        public void Awake() {

        }

        public void Render() {

        }


        public void Update() {
            float cameraSpeed = 2f;
            const float sensitivity = 0.3f;

            if (Input.IsKeyDown(Key.ControlLeft)) {
                cameraSpeed = 4f;
            }

            if (Input.IsKeyDown(Key.W))
                Position += Front * cameraSpeed * (float)Time.DeltaTime; // Forward 
            if (Input.IsKeyDown(Key.S))
                Position -= Front * cameraSpeed * (float)Time.DeltaTime; // Backwards
            if (Input.IsKeyDown(Key.A))
                Position -= Right * cameraSpeed * (float)Time.DeltaTime; // Left
            if (Input.IsKeyDown(Key.D))
                Position += Right * cameraSpeed * (float)Time.DeltaTime; // Right
            if (Input.IsKeyDown(Key.Space))
                Position += Up * cameraSpeed * (float)Time.DeltaTime; // Up 
            if (Input.IsKeyDown(Key.LShift))
                Position -= Up * cameraSpeed * (float)Time.DeltaTime; // Down

            /*if (Input.IsKeyPressed(Key.R)) {
                ApplyRotation = !ApplyRotation;
            }

            if (Input.IsKeyPressed(Key.F11)) {
                WindowState = WindowState == 0 ? WindowState.Fullscreen : 0;
            }*/

            /*if (Input.IsKeyPressed(Key.Q)) {
                foreach (var cube in Cubes) {
                    var norm = (cube.Transform.Position - camera.Position).Normalized();
                    cube.Transform.Position += norm * 10f;
                }
            }*/


            var mouse = Mouse.GetState();

            if (_firstMove) {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            } else {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                var newPos = new Vector2(mouse.X, mouse.Y);
                if (newPos != _lastPos) {
                    Yaw += deltaX * sensitivity;
                    Pitch -= deltaY * sensitivity;
                    _lastPos = newPos;
                    //Game.SkyBox = Color4.FromXyz(new Vector4(
                    //    new Vector3(newPos.X, newPos.Y, newPos.X + newPos.Y).Normalized(), 1));
                }

            }

            // lightPos = Position;
        }
    }
}
