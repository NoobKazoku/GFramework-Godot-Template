shader_type canvas_item;

uniform sampler2D from_tex;
uniform sampler2D to_tex;
uniform float progress : hint_range(0.0, 1.0) = 0.0;

// 动态中心（可从C#传入）
uniform vec2 circle_center = vec2(0.5, 0.5);
uniform bool follow_mouse = false;

uniform float max_radius : hint_range(0.0, 2.0) = 1.5;
uniform float softness : hint_range(0.0, 0.5) = 0.1;

// 方向：true=扩散（新场景从中心扩散），false=收缩（旧场景向中心收缩）
uniform bool expand_mode = true;

uniform bool enable_glow = true;
uniform vec4 glow_color : source_color = vec4(1.0, 0.8, 0.3, 1.0);
uniform float glow_width : hint_range(0.0, 0.3) = 0.08;

void fragment() {
    vec2 uv = UV;
    
    // 纵横比校正
    vec2 aspect = vec2(1.0, SCREEN_PIXEL_SIZE.y / SCREEN_PIXEL_SIZE.x);
    vec2 uv_corrected = (uv - circle_center) * aspect;
    
    float dist = distance(uv_corrected, vec2(0.0));
    float current_radius = progress * max_radius;
    
    // 根据模式计算遮罩
    float mask;
    if (expand_mode) {
        // 扩散模式：圆内显示新场景
        mask = smoothstep(current_radius + softness, current_radius - softness, dist);
    } else {
        // 收缩模式：圆外显示新场景
        mask = smoothstep(current_radius - softness, current_radius + softness, dist);
    }
    
    // 发光边缘
    float glow = 0.0;
    if (enable_glow) {
        float edge_dist = abs(dist - current_radius);
        glow = smoothstep(glow_width, 0.0, edge_dist);
    }
    
    // 采样和混合
    vec4 from_color = texture(from_tex, UV);
    vec4 to_color = texture(to_tex, UV);
    
    vec4 base_color;
    if (expand_mode) {
        base_color = mix(from_color, to_color, mask);
    } else {
        base_color = mix(to_color, from_color, mask);
    }
    
    vec4 final_color = mix(base_color, glow_color, glow * glow_color.a);
    
    COLOR = final_color;
}