shader_type canvas_item;
render_mode unshaded;

uniform vec2 offset;
uniform float shadowIntensity: hint_range(0.0, 1.0, 0.1);
uniform vec4 multiplyColor: source_color;

void fragment() {
	vec4 sprite = texture(TEXTURE, UV);
	vec4 drop = texture(TEXTURE, UV + offset * SCREEN_PIXEL_SIZE) * multiplyColor;
	drop.a *= clamp(UV.y + shadowIntensity, 0.0, 1.0);
	
	COLOR.a = sprite.a + drop.a * (1.0 - sprite.a);
	COLOR.rgb = (sprite.rgb * sprite.a + drop.rgb * drop.a * (1.0 - sprite.a)) / COLOR.a;
}