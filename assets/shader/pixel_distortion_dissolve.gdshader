shader_type canvas_item;

// 场景纹理
uniform sampler2D from_tex;
uniform sampler2D to_tex;

// 噪声纹理
uniform sampler2D noise_tex_normal; // 用于扭曲方向
uniform sampler2D noise_tex;        // 用于溶解模式

// 动画进度
uniform float progress : hint_range(0.0, 1.0) = 0.0;

// 扭曲强度
uniform float strength : hint_range(0.0, 5.0) = 1.0;

// 像素化程度（数值越小越模糊）
uniform float pixel_count : hint_range(1.0, 512.0) = 64.0;

// 溶解边缘软硬度
uniform float dissolve_softness : hint_range(0.0, 0.5) = 0.1;

void fragment() {
    // ========== 1. 像素化处理 ==========
    float pixel_size = 1.0 / pixel_count;
    vec2 pixel_uv = floor(UV / pixel_size) * pixel_size + pixel_size * 0.5;
    
    // ========== 2. 扭曲效果 ==========
    // 从法线纹理获取扭曲方向
    vec2 direction = texture(noise_tex_normal, pixel_uv).xy;
    direction = (direction - 0.5) * 2.0; // 映射到 [-1, 1]
    
    // 旧场景：向外扭曲消失
    vec2 from_distortion = direction * strength * progress;
    vec2 from_uv = clamp(pixel_uv + from_distortion, vec2(0.0), vec2(1.0));
    
    // 新场景：从扭曲中恢复
    vec2 to_distortion = direction * strength * (1.0 - progress);
    vec2 to_uv = clamp(pixel_uv + to_distortion, vec2(0.0), vec2(1.0));
    
    // ========== 3. 采样纹理 ==========
    vec4 from_color = texture(from_tex, from_uv);
    vec4 to_color = texture(to_tex, to_uv);
    
    // ========== 4. 噪声溶解 ==========
    float dissolve_noise = texture(noise_tex, pixel_uv).x;
    
    // 计算溶解遮罩（带软边缘）
    float dissolve_mask = smoothstep(
        progress - dissolve_softness,
        progress + dissolve_softness,
        dissolve_noise
    );
    
    // ========== 5. 混合两个场景 ==========
    // dissolve_mask: 0=显示from, 1=显示to
    COLOR = mix(from_color, to_color, dissolve_mask);
}