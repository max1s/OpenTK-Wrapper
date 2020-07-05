#version 150

uniform mat4 perspective;
uniform mat4 view;
uniform mat4 model;

in vec3 in_vertex;
in vec3 in_normal;
in vec2 in_uv;

out vec2 var_uv;

out vec3 var_normal;
void main(void)
{
	var_uv = in_uv;
	gl_Position = perspective*(view*(model*vec4(in_vertex, 1.0)));
	var_normal = normalize((model*vec4(in_normal, 0)).xyz);
}
