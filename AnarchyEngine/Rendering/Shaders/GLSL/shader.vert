#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aUV;

uniform mat4 uModel;
uniform mat4 uViewProjection;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoords;

void main() {
	TexCoords = aUV;
	Normal = aNormal * mat3(transpose(inverse(uModel)));
	FragPos = vec3(vec4(aPosition, 1.0) * uModel);
    gl_Position = vec4(aPosition, 1.0) * uModel * uViewProjection;
}
