shader_type canvas_item;

uniform float speed: hint_range(0.0, 100.0, 0.1);
uniform sampler2D gridTexture: repeat_enable;

void fragment() {
	COLOR.a -= texture(gridTexture, UV + TEXTURE_PIXEL_SIZE * TIME * speed).r;
}
