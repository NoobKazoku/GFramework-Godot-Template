shader_type canvas_item;

uniform sampler2D from_tex;
uniform sampler2D to_tex;
uniform float progress : hint_range(0.0, 1.0) = 0.0;

uniform int center_number : hint_range(1, 256, 1) = 50;
uniform float seed : hint_range(0.0, 1.0, 0.01) = 0.5;
uniform float growth_speed : hint_range(0.1, 5.0, 0.1) = 2.0;

// 波纹效果
uniform bool enable_ripple = true;
uniform float ripple_width : hint_range(0.0, 0.2, 0.01) = 0.05;
uniform vec4 ripple_color : source_color = vec4(0.5, 0.8, 1.0, 0.6);

uniform bool draw_borders = true;
uniform vec4 border_color : source_color = vec4(1.0, 1.0, 1.0, 0.8);
uniform float border_thickness : hint_range(0.0, 0.02, 0.001) = 0.003;
uniform float minkowski_p : hint_range(1.0, 10.0, 0.1) = 2.0;

float random(vec2 uv) {
    return fract(sin(dot(uv, vec2(12.9898, 78.233)) + seed) * 43758.5453);
}

float minkowski_distance(vec2 p1, vec2 p2, float p) {
    return pow(pow(abs(p1.x - p2.x), p) + pow(abs(p1.y - p2.y), p), 1.0 / p);
}

void fragment() {
    vec2 uv = UV;
    vec2 deriv = fwidth(uv);
    float aspect_ratio = deriv.y / deriv.x;
    vec2 uv_scaled = vec2(uv.x * aspect_ratio, uv.y);
    
    float best_dist = 1e9;
    float second_best_dist = 1e9;
    float best_start_time = 0.0;
    
    for (int i = 0; i < center_number; i++) {
        float fi = float(i);
        vec2 center = vec2(random(vec2(fi, 0.0)), random(vec2(fi, 1.0)));
        center.x *= aspect_ratio;
        
        float start_time = random(vec2(fi, 2.0)) * 0.5;
        float d = minkowski_distance(uv_scaled, center, minkowski_p);
        
        if (d < best_dist) {
            second_best_dist = best_dist;
            best_dist = d;
            best_start_time = start_time;
        } else if (d < second_best_dist) {
            second_best_dist = d;
        }
    }
    
    float cell_progress = clamp((progress - best_start_time) * growth_speed, 0.0, 1.0);
    float max_radius = 1.5;
    float current_radius = cell_progress * max_radius;
    
    bool covered = best_dist < current_radius;
    
    // ========== 波纹检测 ==========
    bool is_ripple = false;
    if (enable_ripple && cell_progress > 0.0 && cell_progress < 1.0) {
        float ripple_outer = current_radius;
        float ripple_inner = current_radius - ripple_width;
        is_ripple = best_dist >= ripple_inner && best_dist < ripple_outer;
    }
    
    // 边界检测
    bool is_border = false;
    if (draw_borders && covered && !is_ripple) {
        float edge_dist = second_best_dist - best_dist;
        float edge_threshold = border_thickness + fwidth(edge_dist) * 0.5;
        is_border = edge_dist < edge_threshold;
    }
    
    // ========== 混合 ==========
    vec4 from_color = texture(from_tex, UV);
    vec4 to_color = texture(to_tex, UV);
    
    vec4 final_color;
    if (is_ripple) {
        // 波纹：半透明发光
        final_color = mix(from_color, ripple_color, ripple_color.a);
    } else if (is_border) {
        final_color = border_color;
    } else if (covered) {
        final_color = to_color;
    } else {
        final_color = from_color;
    }
    
    COLOR = final_color;
}