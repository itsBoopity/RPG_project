shader_type canvas_item;

uniform float coef: hint_range(0.1, 5.0, 0.1);
uniform sampler2D noise_texture: repeat_enable;
uniform sampler2D screen_texture: hint_screen_texture;

void fragment() {
	vec4 scr = texture(screen_texture, SCREEN_UV);
	scr *= (1.0 - UV.y) * clamp(texture(noise_texture, UV + vec2(0.1,0.1) * TIME) * coef,vec4(0.0), vec4(1.0));
	scr *= (1.0 - UV.y) * clamp(texture(noise_texture, UV + vec2(0.05,-0.2) * TIME) * coef,vec4(0.0), vec4(1.0));
	scr = clamp(scr, vec4(0.0), vec4(1.0));
	COLOR = scr;
}