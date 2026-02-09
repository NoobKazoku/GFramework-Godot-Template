shader_type canvas_item;

uniform sampler2D from_tex;
uniform sampler2D to_tex;
uniform float progress : hint_range(0.0, 1.0) = 0.0;

// 像素化控制
uniform int min_pixel_size : hint_range(1, 64) = 1;      // 最小像素大小（正常画面）
uniform int max_pixel_size : hint_range(2, 128) = 64;    // 最大像素大小（完全像素化）
uniform float pixelation_curve : hint_range(0.5, 3.0) = 2.0; // 像素化曲线

// 过渡控制
uniform float crossfade_start : hint_range(0.0, 1.0) = 0.4;
uniform float crossfade_end : hint_range(0.0, 1.0) = 0.6;

// 像素网格颜色（可选）
uniform bool show_grid = false;
uniform vec4 grid_color : source_color = vec4(0.0, 0.0, 0.0, 0.3);
uniform float grid_thickness : hint_range(0.0, 0.5) = 0.1;

void fragment() {
    // ========== 计算像素大小 ==========
    // 使用平滑曲线从min到max
    float t = pow(progress, pixelation_curve);
    float pixel_size = mix(float(min_pixel_size), float(max_pixel_size), t);
    
    // ========== 像素化处理 ==========
    vec2 resolution = 1.0 / SCREEN_PIXEL_SIZE;
    vec2 uv = UV;
    
    // 计算像素化坐标
    vec2 pixel_coord = uv * resolution / pixel_size;
    vec2 pixel_center = floor(pixel_coord) + 0.5; // 居中采样
    vec2 uv_pixelated = pixel_center * pixel_size / resolution;
    uv_pixelated = clamp(uv_pixelated, vec2(0.0), vec2(1.0));
    
    // ========== 采样 ==========
    vec4 from_color = texture(from_tex, uv_pixelated);
    vec4 to_color = texture(to_tex, uv_pixelated);
    
    // ========== 混合 ==========
    float mix_factor = smoothstep(crossfade_start, crossfade_end, progress);
    vec4 base_color = mix(from_color, to_color, mix_factor);
    
    // ========== 可选：绘制像素网格 ==========
    vec4 final_color = base_color;
    
    if (show_grid && pixel_size > 2.0) {
        // 计算像素内部位置
        vec2 pixel_fract = fract(pixel_coord);
        
        // 检测是否在网格线上
        float grid_x = step(1.0 - grid_thickness, pixel_fract.x) + step(pixel_fract.x, grid_thickness);
        float grid_y = step(1.0 - grid_thickness, pixel_fract.y) + step(pixel_fract.y, grid_thickness);
        float grid_mask = clamp(grid_x + grid_y, 0.0, 1.0);
        
        final_color = mix(base_color, grid_color, grid_mask * grid_color.a);
    }
    
    COLOR = final_color;
}