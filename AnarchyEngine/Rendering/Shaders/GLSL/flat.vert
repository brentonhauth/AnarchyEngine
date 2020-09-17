﻿#version 330 core
layout (location = 0) in vec3 aPosition;
// layout (location = 1) in vec3 aNormal;
// layout (location = 2) in vec2 aUV;

uniform mat4 uModel;
uniform mat4 uViewProjection;

void main() {
    gl_Position = vec4(aPosition, 1.0) * uModel * uViewProjection;
}
