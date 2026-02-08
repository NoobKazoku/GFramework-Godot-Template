shader_type canvas_item;

uniform sampler2D from_tex : hint_default_white, filter_linear;
uniform sampler2D to_tex : hint_default_white, filter_linear;
uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform float hex_size : hint_range(0.01, 0.5) = 0.08;
uniform float randomness = 0.05;
uniform float edge_softness = 0.01;
uniform float transition_speed = 0.3;
uniform vec2 resolution = vec2(1920.0, 1080.0); // 添加分辨率参数

float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

vec2 hextile(inout vec2 p) {
    const vec2 s = vec2(1.0, 1.7320508);
    const vec2 hs = s * 0.5;
    vec2 a = mod(p, s) - hs;
    vec2 b = mod(p - hs, s) - hs;
    vec2 c = dot(a, a) < dot(b, b) ? a : b;
    vec2 id = (c - p + hs) / s;
    p = c;
    return floor(id);
}

float hex(vec2 p, float r) {
    p = abs(p);
    return max(p.x * 0.866025 + p.y * 0.5, p.y) - r;
}

void fragment() {
    vec2 uv = UV;
    vec4 from_col = texture(from_tex, uv);
    vec4 to_col = texture(to_tex, uv);
    
    if (progress <= 0.0) {
        COLOR = from_col;
    } else if (progress >= 1.0) {
        COLOR = to_col;
    } else {
        // 使用传入的分辨率而不是 SCREEN_PIXEL_SIZE
        float aspect = resolution.x / resolution.y;
        
        // 将 UV 转换到屏幕空间
        vec2 p = (uv - 0.5) * vec2(aspect, 1.0);
        
        // 缩放到六边形空间
        vec2 hp = p / hex_size;
        vec2 id = hextile(hp);
        
        // 计算延迟（基于屏幕位置的渐进效果）
        float delay = (uv.x + uv.y) * 0.5;
        
        // 计算当前六边形的进度
        float hex_progress = smoothstep(
            delay - transition_speed,
            delay + transition_speed,
            progress * (1.0 + transition_speed)
        );
        hex_progress = clamp(hex_progress, 0.0, 1.0);
        
        // 添加随机延迟
        float r = hash(id);
        float t = clamp(hex_progress - r * randomness, 0.0, 1.0);
        
        // 计算六边形的距离场
        float h = hex(hp, t * 0.9);
        
        // 生成平滑的遮罩
        float mask = smoothstep(edge_softness, -edge_softness, h);
        mask = clamp(mask, 0.0, 1.0);
        
        // 混合两个纹理
        COLOR = mix(from_col, to_col, mask);
    }
}