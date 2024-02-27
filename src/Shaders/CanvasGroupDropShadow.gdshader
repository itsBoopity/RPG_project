shader_type canvas_item;
render_mode unshaded;

uniform vec2 offset;
uniform float shadowIntensity: hint_range(0.0, 1.0, 0.1);
uniform vec4 colorMultiply: source_color;
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
    vec4 c = textureLod(screen_texture, SCREEN_UV, 0.0);
    if (c.a > 0.0001)  c.rgb /= c.a;

	vec4 sprite = COLOR * c;
	vec4 drop = textureLod(screen_texture, SCREEN_UV + offset * SCREEN_PIXEL_SIZE, 0.0) * colorMultiply;
	drop.a *= clamp(UV.y + shadowIntensity, 0.0, 1.0);
	
	COLOR.a = (sprite.a + drop.a * (1.0 - sprite.a));
	COLOR.rgb = (sprite.rgb * sprite.a + drop.rgb * drop.a * (1.0 - sprite.a)) / COLOR.a;
}