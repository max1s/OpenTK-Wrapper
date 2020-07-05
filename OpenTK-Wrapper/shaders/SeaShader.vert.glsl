uniform mat4 perspective;
uniform mat4 view;
uniform mat4 model;
uniform sampler2D waves;
uniform float  time;
in vec3 in_vertex;

out vec2 var_texcoord;
out vec3 var_normal;

void main(void)
{	
	var_texcoord = in_vertex.xz/ 60.0 + vec2(0.7, 0.3) * time/30.0;

	float left = textureOffset(waves, var_texcoord, ivec2(-1, 0)).r;
	float right = textureOffset(waves, var_texcoord, ivec2(1, 0)).r;
	float top = textureOffset(waves, var_texcoord, ivec2(0, -1)).r;
	float bottom = textureOffset(waves, var_texcoord, ivec2(0, 1)).r;

	vec3 lrDifference = vec3(1.0, 0.0, right - left);
	vec3 tbDifference = vec3(0.0, 1.0, bottom - top);
	var_normal = cross(lrDifference, tbDifference);
	
	vec4 texColor = texture(waves, var_texcoord);
	vec3 position = in_vertex + vec3(0,texColor.r*4,0);
	gl_Position = perspective*(view*(vec4(position, 1.0)));
	
	
}
