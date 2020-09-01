using Assimp;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using AnarchyEngine.Rendering.Vertices;

namespace AnarchyEngine.Util {
    public static class FileHelper {
        public static readonly string Path =
            AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\", "");


        public static IEnumerable<Vertex> LoadFbx(string filename) {
            var context = new AssimpContext();
            var verts = new List<Vertex>();
            Scene scene = context.ImportFile(filename);
            foreach (var mesh in scene.Meshes) {
                var vertices = mesh.Vertices;
                var normals = mesh.Normals;
                var uvs = mesh.TextureCoordinateChannels[0];
                for (int i = 0; i < vertices.Count; i++) {
                    var vertex = new Vertex {
                        Position = vertices[i],
                        Normal = normals[i],
                        UV = ((DataTypes.Vector3)uvs[i]).Xy,
                    };

                    verts.Add(vertex);
                }
            }
            return verts;
        }

        public static string LoadLocal(string localPath) {
            using (var sr = new StreamReader(Path + localPath, Encoding.UTF8)) {
                return sr.ReadToEnd();
            }
        }
    }
}
