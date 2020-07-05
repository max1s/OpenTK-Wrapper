#version 150

out vec4 out_colour;
uniform vec3 viewNormal;
uniform sampler2D tex;

in vec2 var_uv;

void main(void)
{
	out_colour = vec4(texture(tex, var_uv).rgb, 1.0);
}
