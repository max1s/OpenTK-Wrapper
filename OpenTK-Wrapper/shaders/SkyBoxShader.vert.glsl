#version 150

uniform mat4 perspective;
uniform mat4 view;

in vec3 in_vertex;

in vec2 in_uv;

out vec2 var_uv;

void main(void)
{
	var_uv = in_uv;
	gl_Position = perspective*vec4((view*vec4(in_vertex, 0.0)).xyz, 1.0);
}
