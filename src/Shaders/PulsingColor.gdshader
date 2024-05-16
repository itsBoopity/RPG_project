// Overlays a texture that moves shifts to the right indefinitely.
shader_type canvas_item;

// The texture to apply over the original image using Color Dodge blend mode.
uniform sampler2D color: repeat_enable;

// Speed of the pulsing.
uniform float speed: hint_range(0.1, 10.0, 0.01) = 5.0;

// The frequency or repeating the texture.
uniform float pulseLength: hint_range(0.1, 10.0, 0.1) = 5.0;

// The intensity of the texture.
uniform float intensity: hint_range(0.1, 1.0, 0.01) = 1.0;

void fragment() {
	// Get the top layer from the supplied texture,
	// using TIME function and parameters to facilitate pulsing.
	vec4 sourceColor = texture(color, vec2((2.0 / pulseLength) * UV.x) - speed * TIME);
	
	// Add the top layer the base image using Color Dodge blend mode.
	vec3 c = sourceColor.rgb * sourceColor.a * intensity;
	COLOR.rgb = COLOR.rgb/(vec3(1.0) - c);
}
