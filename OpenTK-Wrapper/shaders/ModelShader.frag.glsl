#version 150

out vec4 out_colour;
uniform vec3 viewNormal;
uniform sampler2D tex;

in float depth;
in vec3 var_normal;
in vec2 var_uv;

const vec3 lightDirection = normalize(vec3( 3.0, -1.0, 4.0));
const vec3 colour = vec3(1.0, 1.0, 1.0);
void main(void)
{
	float diffuse = max(0.0, dot(lightDirection, var_normal))* 0.5 + 0.5;
	float specular = pow(max(0.0, dot(viewNormal, reflect(lightDirection, var_normal))), 4.0);
	out_colour = vec4(texture(tex, var_uv).rgb, 1.0); //*diffuse*(specular * 0.5 + 0.5), 1.0);
	//out_colour = vec4(var_normal.x*0.5 + 0.5, var_normal.y*0.5 + 0.5, var_normal.z * 0.5 + 0.5, 1.0);
}
