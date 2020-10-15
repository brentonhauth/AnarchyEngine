using AnarchyEngine.Util;
using OpenTK;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnarchyEngine.Rendering.Shaders {
    public class Shader : IPipable {
        public const string PositionName = "aPosition";
        public const string NormalName = "aNormal";
        public const string UVName = "aUV";

        public const string ViewProjectionName = "uViewProjection";
        public const string ModelName = "uModel";
        public const string ColorName = "uColor";
        
        private const string _shaderLoc = "../AnarchyEngine/Rendering/Shaders/GLSL/";

        public static readonly Shader Flat = new Shader($"{_shaderLoc}flat.vert", $"{_shaderLoc}flat.frag");
        public static readonly Shader Default = new Shader($"{_shaderLoc}shader.vert", $"{_shaderLoc}shader.frag");

        public int Handle { get; private set; }

        private readonly Dictionary<string, int> UniformLocations;

        private string vertPath, fragPath;

        private bool Initialized = false;

        public Shader() { }

        public Shader(string vertPath, string fragPath) {
            UniformLocations = new Dictionary<string, int>();
            this.vertPath = vertPath;
            this.fragPath = fragPath;
            Renderer.ScheduleForInit += Init;
        }

        public void Init() {
            if (Initialized) return; else Initialized = true;

            Handle = GL.CreateProgram();

            int vertex = CreateShader(ShaderType.VertexShader, vertPath, Handle);
            int fragment = CreateShader(ShaderType.FragmentShader, fragPath, Handle);

            LinkProgram(Handle);

            DeleteShader(Handle, vertex);
            DeleteShader(Handle, fragment);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            for (var i = 0; i < uniformCount; i++) {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                var location = GL.GetUniformLocation(Handle, key);

                UniformLocations.Add(key, location);
            }
            
            vertPath = fragPath = string.Empty;
        }

        private static int CreateShader(ShaderType type, string path, int handle) {
            string src = FileHelper.LoadLocal(path);
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, src);
            CompileShader(shader);
            GL.AttachShader(handle, shader);
            return shader;
        }

        private static void DeleteShader(int handle, int shader) {
            GL.DetachShader(handle, shader);
            GL.DeleteShader(shader);
        }

        private static void CompileShader(int shader) {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);

            if (code != (int)All.True) {
                throw new Exception(GL.GetShaderInfoLog(shader));
            }
        }

        private static void LinkProgram(int program) {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
            if (code != (int)All.True) {
                throw new Exception(GL.GetProgramInfoLog(program));
            }
        }

        public void Use() => GL.UseProgram(Handle);

        public int GetAttribLocation(string attr) => GL.GetAttribLocation(Handle, attr);

        public void SetInt(string name, int data) {
            Use();
            GL.Uniform1(LocateUniform(name), data);
        }

        public void SetFloat(string name, float data) {
            Use();
            GL.Uniform1(LocateUniform(name), data);
        }

        public void SetMatrix4(string name, Matrix4 data) {
            Use();
            GL.UniformMatrix4(LocateUniform(name), true, ref data);
        }

        public void SetVector3(string name, Vector3 data) {
            Use();
            GL.Uniform3(LocateUniform(name), ref data);
        }

        public void Set(ActiveUniformType type, string name, dynamic data) {
            switch (type) {
                case ActiveUniformType.Int:
                    SetInt(name, data); break;
                case ActiveUniformType.Float:
                    SetFloat(name, data); break;
                case ActiveUniformType.FloatVec3:
                    SetVector3(name, data); break;
                case ActiveUniformType.FloatMat4:
                    SetMatrix4(name, data); break;
                default:
                    throw new Exception($"No type handling for \"{type}\"");
            }
        }

        private int LocateUniform(string name) {
            if (UniformLocations.TryGetValue(name, out int value)) {
                return value;
            }

            int location = GL.GetUniformLocation(Handle, name);
            UniformLocations.Add(name, location);
            return location;
        }

        public void Dispose() {
            GL.DeleteProgram(Handle);
        }
    }
}
