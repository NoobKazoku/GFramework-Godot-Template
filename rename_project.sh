#!/bin/bash

set -e

OLD_PROJECT_NAME="GFramework-Godot-Template"
OLD_NAMESPACE="GFrameworkGodotTemplate"

usage() {
    cat << EOF
用法: $0 <新项目名> [选项]

选项:
    -h, --help      显示帮助信息
    --what-if       预览模式，不实际执行更改

示例:
    $0 "MyAwesomeGame"
    $0 "MyAwesomeGame" --what-if
EOF
}

convert_to_pascal_case() {
    local name="$1"
    local cleaned=$(echo "$name" | sed 's/[^a-zA-Z0-9]//g')
    if [ -z "$cleaned" ]; then
        echo ""
        return
    fi
    local first=$(echo "$cleaned" | head -c 1 | tr '[:lower:]' '[:upper:]')
    local rest=$(echo "$cleaned" | tail -c +2)
    echo "${first}${rest}"
}

test_project_name() {
    local name="$1"
    
    if [[ "$name" =~ [^a-zA-Z0-9\-] ]]; then
        echo "错误: 项目名只能包含字母、数字和连字符"
        return 1
    fi
    
    if [[ "$name" =~ ^[0-9] ]]; then
        echo "错误: 项目名不能以数字开头"
        return 1
    fi
    
    if [ ${#name} -lt 2 ] || [ ${#name} -gt 50 ]; then
        echo "错误: 项目名长度必须在 2-50 字符之间"
        return 1
    fi
    
    return 0
}

get_exclude_dirs() {
    echo ".godot|.git|.idea|.vscode|bin|obj|.mono"
}

update_file_content() {
    local file="$1"
    local old_name="$2"
    local new_name="$3"
    local old_ns="$4"
    local new_ns="$5"
    local what_if="$6"
    
    if [ "$what_if" = "true" ]; then
        if grep -q "$old_name" "$file" 2>/dev/null || grep -q "$old_ns" "$file" 2>/dev/null; then
            echo "  [更新] $file"
        fi
    else
        # Perform both replacements in one pass
        perl -i.bak -pe "s/\Q${old_name}\E/${new_name}/g; s/\Q${old_ns}\E/${new_ns}/g" "$file"
        rm -f "${file}.bak"
    fi
}

rename_project_file() {
    local file="$1"
    local old_name="$2"
    local new_name="$3"
    local what_if="$4"
    
    if [ ! -f "$file" ]; then
        return
    fi
    
    local new_file="${file/$old_name/$new_name}"
    
    if [ "$what_if" = "true" ]; then
        echo "  [重命名] $file -> $new_file"
    else
        mv "$file" "$new_file"
        echo "  [重命名] $new_file"
    fi
}

# 参数解析
NEW_PROJECT_NAME=""
WHAT_IF="false"

while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            usage
            exit 0
            ;;
        --what-if)
            WHAT_IF="true"
            shift
            ;;
        -*)
            echo "未知选项: $1"
            usage
            exit 1
            ;;
        *)
            if [ -z "$NEW_PROJECT_NAME" ]; then
                NEW_PROJECT_NAME="$1"
            else
                echo "错误: 只能指定一个项目名"
                exit 1
            fi
            shift
            ;;
    esac
done

if [ -z "$NEW_PROJECT_NAME" ]; then
    echo "错误: 请指定新项目名"
    usage
    exit 1
fi

echo ""
echo "===== 项目重命名工具 ====="
echo -e "旧项目名: \033[33m$OLD_PROJECT_NAME\033[0m"
echo -e "新项目名: \033[32m$NEW_PROJECT_NAME\033[0m"

if ! test_project_name "$NEW_PROJECT_NAME"; then
    exit 1
fi

NEW_NAMESPACE=$(convert_to_pascal_case "$NEW_PROJECT_NAME")
echo -e "新命名空间: \033[32m$NEW_NAMESPACE\033[0m"

EXCLUDE_PATTERN=$(get_exclude_dirs)

echo ""
echo "开始处理..."
if [ "$WHAT_IF" = "true" ]; then
    echo -e "\033[33m[预览模式] 不会实际修改文件\033[0m"
fi

echo ""
echo "1. 更新文件内容..."

processed_files=0

# Use find with multiple patterns
file_pattern_cmd='find . -type f \( -name "*.cs" -o -name "*.csproj" -o -name "*.sln" -o -name "project.godot" -o -name "README.md" -o -name ".gitignore" -o -name "*.yml" \) 2>/dev/null'

while IFS= read -r file; do
    if [[ "$file" =~ $EXCLUDE_PATTERN ]]; then
        continue
    fi
    
    has_changes=false
    if grep -q "$OLD_PROJECT_NAME" "$file" 2>/dev/null || grep -q "$OLD_NAMESPACE" "$file" 2>/dev/null; then
        has_changes=true
    fi
    
    if [ "$has_changes" = "true" ]; then
        rel_path="${file#./}"
        
        if [ "$WHAT_IF" = "true" ]; then
            echo -e "  \033[36m[更新]\033[0m $rel_path"
        else
            update_file_content "$file" "$OLD_PROJECT_NAME" "$NEW_PROJECT_NAME" "$OLD_NAMESPACE" "$NEW_NAMESPACE" "$WHAT_IF"
            echo -e "  \033[32m[更新]\033[0m $rel_path"
        fi
        ((processed_files++))
    fi
done < <(eval "$file_pattern_cmd")

echo ""
echo "2. 重命名项目文件..."

rename_project_file "$OLD_PROJECT_NAME.csproj" "$OLD_PROJECT_NAME" "$NEW_PROJECT_NAME" "$WHAT_IF"
rename_project_file "$OLD_PROJECT_NAME.sln" "$OLD_PROJECT_NAME" "$NEW_PROJECT_NAME" "$WHAT_IF"

if [ -f "$OLD_PROJECT_NAME.sln.DotSettings.user" ]; then
    rename_project_file "$OLD_PROJECT_NAME.sln.DotSettings.user" "$OLD_PROJECT_NAME" "$NEW_PROJECT_NAME" "$WHAT_IF"
fi

echo ""
echo "===== 完成 ====="
echo -e "更新文件数: \033[32m$processed_files\033[0m"

if [ "$WHAT_IF" = "false" ]; then
    echo ""
    echo "提示:"
    echo "1. 在 Rider 中重新打开解决方案"
    echo "2. 运行清理命令: dotnet clean"
    echo "3. 重新构建项目: dotnet build"
fi
