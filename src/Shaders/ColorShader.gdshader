// This shader combines code for hue shifting and BCS (Brightness/Contrast/Saturation) filters.
// Hue shift: https://gist.github.com/McSpider/c8c693f1065fdb5ec44c64cb9fdac340
// BCS: https://github.com/godotengine/godot-demo-projects/blob/master/2d/screen_space_shaders/shaders/BCS.gdshader
shader_type canvas_item;

uniform float hue: hint_range(0, 1) = 0;
uniform float brightness: hint_range(0, 4) = 1;
uniform float contrast: hint_range(0, 4) = 1;
uniform float saturation: hint_range(0, 4) = 1;

void fragment() {
// Hue shift
	// Input:3
	// Get color.
	vec3 input_color;
	vec4 texture_color = texture(TEXTURE, UV);
	input_color = texture_color.rgb;

	// VectorFunc:2
	// Convert color from RGB to HSV (Hue/Saturation/Value)
	vec3 color_hsv;
	{
		vec3 c = input_color;
		vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
		vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
		vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));
		float d = q.x - min(q.w, q.y);
		float e = 1.0e-10;
		color_hsv=vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
	}
	// Shift the Hue value.
	color_hsv.x = mod((color_hsv.x + hue), 1.0f);

	// VectorFunc:5
	// Convert the HSV color back to RGB.
	vec3 color_rgb;
	{
		vec3 c = color_hsv;
		vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
		vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
		color_rgb=c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
	}

	// Output:0
	// Hue shift complete.
	vec3 c = color_rgb;

// BCS
    c.rgb = mix(vec3(0.0), c.rgb, brightness);
    c.rgb = mix(vec3(0.5), c.rgb, contrast);
    c.rgb = mix(vec3(dot(vec3(1.0), c.rgb) * 0.33333), c.rgb, saturation);

    COLOR = vec4(c.rgb,texture_color.a);
}