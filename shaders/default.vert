#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aUV;
layout (location = 2) in vec3 aNormal;

out vec2 texCoord;
out vec3 normal;

uniform mat4 projection;
uniform mat4 view;

void main() {
    texCoord = aUV;
    normal = aNormal;
    gl_Position = vec4(aPosition, 1.0) * view * projection;
}
