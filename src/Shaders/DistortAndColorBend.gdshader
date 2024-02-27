shader_type canvas_item;

uniform sampler2D screen_texture: hint_screen_texture;
uniform float influence: hint_range(-1.0, 1.0, 0.1);
uniform sampler2D bendCurve;
uniform sampler2D gradientMap;

void fragment() {
	float mapToCurve = texture(bendCurve, UV + influence).r;
	COLOR = texture(screen_texture, SCREEN_UV + vec2(0.0, 0.15) * mapToCurve);
	
	COLOR = mix(COLOR, texture(gradientMap, vec2(COLOR.r * 0.299 + COLOR.g * 0.587 + COLOR.b * 0.114)), mapToCurve);
}