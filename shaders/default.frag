#version 330 core

out vec4 FragColor;
in vec2 texCoord;
in vec3 normal;

uniform vec3 lightDir;
uniform sampler2D colorMap;
uniform sampler2D bumpMap;

void main() 
{
    float dirLight = max(dot(normal, lightDir), 0.0);
    FragColor = texture(colorMap, texCoord) * dirLight;
}