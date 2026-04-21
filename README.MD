# Unity 2D 动作 RPG — 核心系统技术验证 Demo

个人学习向项目，侧重 **数据流、UI 架构、可复用模块**；美术与完整业务非重点。目标是为后续中大型 RPG 积累可迭代的基础模板。

**引擎与主要依赖**：Unity 6000.3.x（见下方版本）· Input System · UGUI · DOTween · Cinemachine · `JsonUtility`（JSON 存档）

---

## 功能概览

| 模块 | 说明 |
|------|------|
| **背包** | 槽位数据 + 虚拟滚动与对象池；`ScriptableObject` 模板 + `ItemInstance` 运行时状态（含稀有度）；排序/标签整理（LINQ）；DTO 化存档字段 |
| **UI** | `UIManager` + `BasePanel` + 栈式弹出；面板注册字典；背包关闭时统一收起二级 UI（`BagClose` / `InventoryPanelUI.Close`） |
| **存档** | `JsonProcess` IO 层（路径解析、多槽位、`*.tmp` 原子写入、UTF-8 无 BOM）；`GameData` + 各模块 `*ConnectJson`；跨场景待读档槽位 |
| **战斗** | 新输入系统；玩家/敌人 FSM；攻击输入缓冲与动画事件连招；`OverlapBox` + `LayerMask`；顿帧、震屏、受击反馈（数据在 SO） |
| **其它** | 全局事件总线（`GameEvent`）；关键管理器单例；`PlayerPrefs` 简单图形/音量设置 |

---

## 环境要求

- **Unity 编辑器**：`6000.3.2f1`（与 `ProjectSettings/ProjectVersion.txt` 一致为佳）
- **平台**：当前以 **Windows** 为主验证

---

## 如何运行

1. 使用上述版本 Unity **打开本仓库根目录**（包含 `Assets`、`Packages` 的文件夹）。
2. **构建场景**：工程已包含 `Menu`、`MainScene`（见 `ProjectSettings/EditorBuildSettings.asset`）；若自行调整，请保证主流程场景在 **Build Settings** 中已勾选。
3. **Play**：进入播放模式；**背包**由 `PlayerAction` 输入资产中的 **Bag** 绑定打开/关闭（具体按键以 `Assets/Scripts/Others/PlayerAction.inputactions` 为准）。
4. **存档**：场景中需正确挂载 **`GameDataSaveLoad`**，并指定 **`BagItem`** 物品目录等引用（详见各组件 Inspector / `Tooltip`）。

---

## 仓库结构（脚本侧）

```
Assets/Scripts/
├── GameManager/      # UIManager、GameEvent、LoadManager、HitStop、输入等
├── JsonSave/         # GameData、DTO、GameDataSaveLoad、JsonProcess
├── UI/               # BasePanel、背包与 HUD 等
├── Player/           # 状态机、战斗、防御、翻滚等
├── Enemy/            # AI 状态、血量、对象池等
├── SOData/           # 各类 ScriptableObject 数据
└── ...
```

---

## 设计要点（自述 / 面试用）

- **背包**：逻辑数据在 `InventoryManager`，UI 通过事件刷新；排序会整理槽位顺序，与 JSON 连接器分层。
- **存档**：DTO + 模块连接器；先写临时文件再替换正式 JSON，降低写入中断导致坏档的风险。
- **UI**：`PushIn` / `PopOut` 与 `Pause` / `Resume` 处理多层界面遮挡与恢复。

---

## 已知限制 / 未实现

- **装备**：`ItemEquipped` 事件已预留，**无装备槽与订阅实现**；演示请勿依赖「装备」按钮，或自行扩展。
- **架构**：全局事件与单例为迭代期取舍，非严格教科书 MVC。
- **内容量**：关卡与数值服务于系统验证，不代表商业项目体量。

---

## 第三方与许可

工程内包含 **DOTween**、**Cinemachine** 等插件资源；分发或开源时请自行核对各插件许可条款，并在仓库中补充相应说明。

---

## 作者

个人项目，仅供学习与技术展示。
