#version 330 core

uniform vec3 color;
out vec4 FragColor;

int main() {
    FragColor = vec4(color, 1.0);
}