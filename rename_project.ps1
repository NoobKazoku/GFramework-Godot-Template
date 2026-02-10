#!/usr/bin/env pwsh

<#
.SYNOPSIS
    重命名 GFramework-Godot-Template 项目
.DESCRIPTION
    将项目中的 "GFramework-Godot-Template" 和 "GFrameworkGodotTemplate" 替换为新项目名
.PARAMETER NewProjectName
    新项目名称
.PARAMETER WhatIf
    预览模式，不实际执行更改
.EXAMPLE
    .\rename_project.ps1 "MyAwesomeGame"
.EXAMPLE
    .\rename_project.ps1 "MyAwesomeGame" -WhatIf
#>

param(
    [Parameter(Mandatory=$true, Position=0)]
    [string]$NewProjectName,

    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"

$OldProjectName = "GFramework-Godot-Template"
$OldNamespace = "GFrameworkGodotTemplate"

function ConvertTo-PascalCase {
    param([string]$name)
    
    $cleaned = $name -replace '[^a-zA-Z0-9]', ''
    if ($cleaned.Length -eq 0) {
        return ""
    }
    $pascal = $cleaned.Substring(0,1).ToUpper() + $cleaned.Substring(1)
    return $pascal
}

function Test-ProjectName {
    param([string]$name)
    
    if ($name -match '[^a-zA-Z0-9\-]') {
        Write-Host "错误: 项目名只能包含字母、数字和连字符" -ForegroundColor Red
        return $false
    }
    
    if ($name -match '^\d') {
        Write-Host "错误: 项目名不能以数字开头" -ForegroundColor Red
        return $false
    }
    
    if ($name.Length -lt 2 -or $name.Length -gt 50) {
        Write-Host "错误: 项目名长度必须在 2-50 字符之间" -ForegroundColor Red
        return $false
    }
    
    return $true
}

function Get-ExcludeDirs {
    @('.godot', '.git', '.idea', '.vscode', 'bin', 'obj', '.mono')
}

function Update-FileContent {
    param(
        [string]$filePath,
        [hashtable]$replacements
    )
    
    $content = Get-Content $filePath -Raw -Encoding UTF8
    
    foreach ($key in $replacements.Keys) {
        $content = $content -replace [regex]::Escape($key), $replacements[$key]
    }
    
    if (-not $WhatIf) {
        Set-Content $filePath $content -NoNewline -Encoding UTF8
    }
}

function Rename-ProjectFile {
    param(
        [string]$filePath,
        [string]$oldName,
        [string]$newName
    )
    
    if (-not (Test-Path $filePath)) { return }
    
    $newPath = $filePath.Replace($oldName, $newName)
    
    if ($WhatIf) {
        Write-Host "  [重命名] $filePath -> $newPath" -ForegroundColor Cyan
    } else {
        Rename-Item $filePath $newPath
        Write-Host "  [重命名] $newPath" -ForegroundColor Green
    }
}

Write-Host "`n===== 项目重命名工具 =====" -ForegroundColor Cyan
Write-Host "旧项目名: $OldProjectName" -ForegroundColor Yellow
Write-Host "新项目名: $NewProjectName" -ForegroundColor Green

if (-not (Test-ProjectName $NewProjectName)) {
    exit 1
}

$NewNamespace = ConvertTo-PascalCase $NewProjectName
Write-Host "新命名空间: $NewNamespace" -ForegroundColor Green

$replacements = @{
    $OldProjectName = $NewProjectName
    $OldNamespace = $NewNamespace
}

$excludeDirs = Get-ExcludeDirs
$excludePattern = '(' + ($excludeDirs -join '|') + ')'

Write-Host "`n开始处理..." -ForegroundColor Cyan
if ($WhatIf) {
    Write-Host "[预览模式] 不会实际修改文件`n" -ForegroundColor Yellow
}

Write-Host "`n1. 更新文件内容..." -ForegroundColor Cyan

$filePatterns = @('*.cs', '*.csproj', '*.sln', 'project.godot', 'README.md', '.gitignore','*.yml')
$processedFiles = 0

$currentDir = Get-Location

foreach ($pattern in $filePatterns) {
    Get-ChildItem -Path . -Filter $pattern -File -Recurse -ErrorAction SilentlyContinue |
    Where-Object { $_.DirectoryName.Replace($currentDir.Path, "") -notmatch $excludePattern } |
    ForEach-Object {
        $filePath = $_.FullName
        
        $hasChanges = $false
        $content = Get-Content $filePath -Raw -Encoding UTF8 -ErrorAction SilentlyContinue
        
        if ($content) {
            foreach ($key in $replacements.Keys) {
                if ($content -match [regex]::Escape($key)) {
                    $hasChanges = $true
                    break
                }
            }
        }
        
        if ($hasChanges) {
            $relPath = $filePath.Substring($currentDir.Path.Length + 1)
            
            if ($WhatIf) {
                Write-Host "  [更新] $relPath" -ForegroundColor Cyan
            } else {
                Update-FileContent $filePath $replacements
                Write-Host "  [更新] $relPath" -ForegroundColor Green
            }
            $processedFiles++
        }
    }
}

Write-Host "`n2. 重命名项目文件..." -ForegroundColor Cyan

Rename-ProjectFile "$OldProjectName.csproj" $OldProjectName $NewProjectName
Rename-ProjectFile "$OldProjectName.sln" $OldProjectName $NewProjectName

$dotSettingsUserFile = "$OldProjectName.sln.DotSettings.user"
if (Test-Path $dotSettingsUserFile) {
    Rename-ProjectFile $dotSettingsUserFile $OldProjectName $NewProjectName
}

Write-Host "`n===== 完成 =====" -ForegroundColor Cyan
Write-Host "更新文件数: $processedFiles" -ForegroundColor Green

if (-not $WhatIf) {
    Write-Host "`n提示:" -ForegroundColor Yellow
    Write-Host "1. 在 Rider 中重新打开解决方案" -ForegroundColor White
    Write-Host "2. 运行清理命令: dotnet clean" -ForegroundColor White
    Write-Host "3. 重新构建项目: dotnet build" -ForegroundColor White
}
