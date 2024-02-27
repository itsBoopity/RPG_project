shader_type canvas_item;

uniform float thickness: hint_range(0.0, 32.0, 0.5);
uniform vec4 color: source_color;

void fragment() {
	if (COLOR.a < 0.9) {
		float a = texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(thickness, 0.0)).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(-thickness, 0.0)).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(0.0, thickness)).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(0.0, -thickness)).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(+thickness, -thickness)*0.7).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(+thickness, +thickness)*0.7).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(-thickness, -thickness)*0.7).a
			+ texture(TEXTURE, UV + TEXTURE_PIXEL_SIZE * vec2(-thickness, +thickness)*0.7).a;
		vec4 og = COLOR;
		vec4 ol = vec4(color.rgb,a);
		COLOR.a = og.a + ol.a * (1.0 - og.a);
		COLOR.rgb = ((og.rgb * og.a) + ol.rgb * ol.a * (1.0 * og.a)) / COLOR.a;
	}
}