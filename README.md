# GFramework-Godot-Template



## 命名规范
 
1. 变量名：首字母小写，驼峰命名法
2. 常量名：全大写，下划线分隔
3. 文件夹和文件名： 全小写，下划线分隔

## 文件夹结构

- assets: 包含游戏资源，如美术、数据、字体、音乐和声音文件。
- global: 存放全局C#脚本（例如GameEntryPoint.cs、UiRoot.cs）及相关场景文件，主要用于项目的全局入口点和UI根节点。
- resource: 包含资源文件，如音频总线布局，并有着色器和主题的子目录，用于管理项目范围内的资源，如音频设置和UI主题。
- scenes: 包含主场景文件（main.tscn）和一个tests子目录，通常用于存储Godot场景文件和测试场景。
- script_templates: 包含Godot的自定义脚本模板，包括一个.editorconfig文件和用于标准化代码生成的Node子目录。
- scripts: 组织为core和module子目录，包含GDScript或其他脚本，用于核心功能和模块化组件。 

## CoreGrid 模板内容管线约定

本模板开始吸收 CoreGrid 的非玩法层配置/内容管线约定，但边界仅限模板基础设施，不包含任何玩法逻辑、玩法数据结构或玩法迁移脚本。

### 路径设置

- `project.godot` 中的 `application/config/content/source_root_path` 定义配置与 schema 的源目录，模板默认值为 `res://`。
- `project.godot` 中的 `application/config/content/cache_root_path` 定义运行时缓存目录，模板默认值为 `user://config_cache`。
- `source_root_path` 可以配置为 `res://`、`user://` 或原生绝对路径，但提交到模板仓库的源配置文件仍应放在 `config/` 与 `schemas/` 下。
- `config/` 存放 YAML/YML 配置表，`schemas/` 存放 JSON Schema；生成代码和注册信息必须继续把它们视为相对 `source_root_path` 的内容根。

### `res://` 与 `user://` 规则

- `res://` 用于只读、随模板发布的内容源。编辑器内允许直接读取该目录。
- `user://` 用于运行时可写数据，例如 `user://config_cache`、设置文件和存档；不要把需要玩家写回的内容放到 `res://`。
- 导出后的运行时不能假设 `res://` 对 YAML/schema 仍然表现为普通可遍历目录。若 `source_root_path` 指向 `res://` 且运行环境不是编辑器，模板会先把打包内容同步到 `user://config_cache`，再从缓存加载。
- 若 `source_root_path` 指向 `user://` 或可直接访问的原生目录，并且所需 `config/`、`schemas/` 文件完整存在，则可以直接从源目录读取，不经过 `res://` 回退缓存。

### 嵌入打包约定

- `config/**/*.yaml`、`config/**/*.yml` 与 `schemas/**/*.json` 会作为嵌入资源打进程序集，保证导出包内仍可提取这些文件。
- 运行时缓存同步会优先读源文件；当源文件不可直接访问时，再从嵌入资源恢复到 `user://config_cache`。
- 新增非玩法模板配置时，文件必须继续放在 `config/` 或 `schemas/` 下，并保持生成注册、嵌入资源打包与运行时缓存同步三者一致。

### 本地化刷新约定

- `TemplateContentCatalog` 会根据当前语言环境选择配置表中的语言条目；当前模板约定使用 `en` 与 `zh-cn`。
- 语言切换后的 UI 文本刷新依赖语言变化事件和绑定系统，但配置表本身不会因为切换语言而自动重建注册表。
- 如果新增的非玩法配置内容依赖语言分表或运行时热更新，调用方需要显式触发内容目录刷新，再重新应用对应 UI 或展示层绑定。

### 模板边界

- 这里记录的是模板层约定：路径、打包、缓存、schema 校验根目录和本地化内容刷新预期。
- 不要在这套迁移里引入 CoreGrid 的玩法场景、玩法状态机、战斗规则、地图逻辑或其他 gameplay 内容。

## 框架文档
https://gewuyou.github.io/GFramework

## 发布流程

- 仓库版本号通过 `semantic-release` 根据 Conventional Commits 自动计算。
- `feat` 会触发次版本递增，`fix`、`perf`、`refactor`、`revert` 会触发补丁版本递增，`docs`、`test`、`chore`、`build`、`ci`、`style` 默认不发版。
- 破坏性变更必须使用 `type(scope)!:` 或 `BREAKING CHANGE:`/`BREAKING CHANGES:` 页脚，否则不会自动递增主版本号。
- 正式发布需要手动在 Actions 页面运行 `Semantic Release Version and Tag` 工作流，并将 `mode` 设为 `publish`。
- 若当前没有可发布的提交，工作流会直接跳过，不会创建 tag 或 GitHub Release。
- 若存在可发布版本，需要通过 `release-approval` environment 审批后，才会创建 tag 并触发 `publish.yml` 导出 Godot 构建。
- 如需提前预览版本号，可将 `mode` 设为 `preview`；该模式不会创建 tag 或 GitHub Release。
- 发布依赖仓库 secret `PAT_TOKEN` 和 `release-approval` environment。

## 许可证

### 源代码
本项目的源代码根据Apache许可证第2.0版进行许可。

### 游戏资源
所有游戏资源（包括但不限于美术、音频、字体和文本）
不受Apache许可证2.0的约束。

除非另有说明，否则所有资源均为©作者所有，未经明确许可不得使用。
