#version 330 core

out vec4 FragColor;
in vec2 texCoord;
in vec3 normal;

uniform sampler2D texture0;

void main() 
{
    float dirLight = max(dot(normal, normalize(vec3(1.0, 1.0, 0.0))), 0.0);
    FragColor = texture(texture0, texCoord) * dirLight;
    //FragColor = vec4(1.0f, 0.0f, 0.0f, 1.0f);
}