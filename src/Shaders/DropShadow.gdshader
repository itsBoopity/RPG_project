// Displays a dropshadow behind texture. 
shader_type canvas_item;

// The offset of the dropshadow from the source texture.
uniform vec2 offset;

// Makes the dropshadow lighter at the top if shadowPersistence is low, creating a gradient.
// 1.0 for no gradient.
uniform float shadowPersistence: hint_range(0.0, 1.0, 0.02);

// The color of the dropshadow.
uniform vec4 colorMultiply: source_color;

void fragment() {
	// Get source texture and dropshadow.
	vec4 sprite = texture(TEXTURE, UV);
	vec4 drop = texture(TEXTURE, UV + offset * SCREEN_PIXEL_SIZE) * colorMultiply;
	// Make drop shadow lighter towards the top.
	drop.a *= clamp(UV.y + shadowPersistence, 0.0, 1.0);
	
	// Standard formula alpha compositing, i.e. adding two image layers with transparency together. 
	COLOR.a = sprite.a + drop.a * (1.0 - sprite.a);
	COLOR.rgb = (sprite.rgb * sprite.a + drop.rgb * drop.a * (1.0 - sprite.a)) / COLOR.a;
}