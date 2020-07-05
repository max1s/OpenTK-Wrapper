out vec4 out_colour;
in vec2 var_texcoord;
in vec3 var_normal;

uniform sampler2D waves;
uniform vec3 viewNormal;

const vec3 lightDirection = normalize(vec3( 3.0, -1.0, 4.0));
const vec4 darkBlue = vec4( 0.0, 0.3, 0.7, 0.9);
const vec4 lightBlue = vec4( 0.5 , 0.6, 1.0, 0.9);

void main()
{
	float specular = pow(dot(viewNormal, -reflect(lightDirection, var_normal))*0.5 + 0.5, 2);
	vec4 texColor = texture(waves, var_texcoord);
	out_colour = darkBlue + (specular * (lightBlue - darkBlue));
}