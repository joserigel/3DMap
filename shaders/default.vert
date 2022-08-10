#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aUV;
layout (location = 2) in vec3 aNormal;

out vec2 texCoord;
out vec3 normal;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

uniform float bumpIntensity;

uniform sampler2D bumpMap;


void main() {
    texCoord = aUV;
    normal = aNormal;
    
    vec4 offset = texture(bumpMap, aUV);
    vec3 bump = bumpIntensity * aNormal * ((offset.x + offset.y + offset.z) / 3);
    bump -= 0.5;


    gl_Position = vec4(aPosition + bump, 1.0) * model * view * projection;
}
