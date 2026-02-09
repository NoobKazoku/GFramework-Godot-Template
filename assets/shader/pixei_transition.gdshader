shader_type canvas_item;

// ========================
// 必选输入
// ========================
uniform sampler2D from_tex;
uniform sampler2D to_tex;
uniform float progress : hint_range(0.0, 1.0);

// ========================
// 屏幕参数（由 C# 注入）
// ========================
uniform vec2 resolution;

// ========================
// 效果参数（可选，可扩展）
// ========================
uniform float min_tiles = 1.0;
uniform float max_tiles = 200.0;

void fragment() {
    vec2 fragCoord = UV * resolution;

    vec2 sample_uv = UV;

    if (progress > 0.0 && progress < 1.0) {
        // 0 → 0.5 → 1 : 清晰 → 像素 → 清晰
        float curve = abs(progress - 0.5) * 2.0;
        float tiles = mix(min_tiles, max_tiles, curve);
        tiles = max(1.0, floor(tiles));

        vec2 tile_size = resolution / tiles;
        vec2 sample_pos =
            floor(fragCoord / tile_size) * tile_size + tile_size * 0.5;

        sample_uv = sample_pos / resolution;
    }

    vec4 from_col = texture(from_tex, sample_uv);
    vec4 to_col   = texture(to_tex, sample_uv);

    COLOR = mix(from_col, to_col, progress);
}
