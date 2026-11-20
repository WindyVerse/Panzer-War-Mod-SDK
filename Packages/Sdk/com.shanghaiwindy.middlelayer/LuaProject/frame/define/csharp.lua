---@class Frontend.Runtime.Impl.GameImpl.Battle.AbstractNetBattleGameMode
local AbstractNetBattleGameMode = {}

---@instance
---@function [AbstractNetBattleGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function AbstractNetBattleGameMode:SendAllTextMessage(text) end
---@instance
---@function [AbstractNetBattleGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function AbstractNetBattleGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [AbstractNetBattleGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function AbstractNetBattleGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [AbstractNetBattleGameMode:GetPing]
---@return System.Int32
function AbstractNetBattleGameMode:GetPing() end
---@instance
---@function [AbstractNetBattleGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function AbstractNetBattleGameMode:IsMainPlayerLoaded() end
---@instance
---@function [AbstractNetBattleGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function AbstractNetBattleGameMode:GetPlayerPosition() end
---@instance
---@function [AbstractNetBattleGameMode:GetBattleMeta]
---@return System.String
function AbstractNetBattleGameMode:GetBattleMeta() end
return AbstractNetBattleGameMode

---@class Frontend.Runtime.Impl.GameImpl.Battle.Mode.CaptureZoneNetGameMode
local CaptureZoneNetGameMode = {}

---@instance
---@function [CaptureZoneNetGameMode:PickCaptureZone]
---@return System.Void
---@param zoneName System.String
function CaptureZoneNetGameMode:PickCaptureZone(zoneName) end
---@instance
---@function [CaptureZoneNetGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function CaptureZoneNetGameMode:SendAllTextMessage(text) end
---@instance
---@function [CaptureZoneNetGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function CaptureZoneNetGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [CaptureZoneNetGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function CaptureZoneNetGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [CaptureZoneNetGameMode:GetPing]
---@return System.Int32
function CaptureZoneNetGameMode:GetPing() end
---@instance
---@function [CaptureZoneNetGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function CaptureZoneNetGameMode:IsMainPlayerLoaded() end
---@instance
---@function [CaptureZoneNetGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function CaptureZoneNetGameMode:GetPlayerPosition() end
---@instance
---@function [CaptureZoneNetGameMode:GetBattleMeta]
---@return System.String
function CaptureZoneNetGameMode:GetBattleMeta() end
return CaptureZoneNetGameMode

---@class Frontend.Runtime.Impl.GameImpl.Battle.Mode.ReplayNetGameMode
local ReplayNetGameMode = {}

---@instance
---@function [ReplayNetGameMode:GetReportedPlayerNickName]
---@return System.String
function ReplayNetGameMode:GetReportedPlayerNickName() end
---@instance
---@function [ReplayNetGameMode:AgreeBan]
---@return System.Void
function ReplayNetGameMode:AgreeBan() end
---@instance
---@function [ReplayNetGameMode:DisagreeBan]
---@return System.Void
function ReplayNetGameMode:DisagreeBan() end
---@instance
---@function [ReplayNetGameMode:Ban]
---@return System.Void
function ReplayNetGameMode:Ban() end
---@instance
---@function [ReplayNetGameMode:IsShowBan]
---@return System.Boolean
function ReplayNetGameMode:IsShowBan() end
---@instance
---@function [ReplayNetGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function ReplayNetGameMode:SendAllTextMessage(text) end
---@instance
---@function [ReplayNetGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function ReplayNetGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [ReplayNetGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function ReplayNetGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [ReplayNetGameMode:GetPing]
---@return System.Int32
function ReplayNetGameMode:GetPing() end
---@instance
---@function [ReplayNetGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function ReplayNetGameMode:IsMainPlayerLoaded() end
---@instance
---@function [ReplayNetGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function ReplayNetGameMode:GetPlayerPosition() end
---@instance
---@function [ReplayNetGameMode:GetBattleMeta]
---@return System.String
function ReplayNetGameMode:GetBattleMeta() end
return ReplayNetGameMode

---@class Frontend.Runtime.Impl.GameImpl.Battle.Mode.RtsNetGameMode
local RtsNetGameMode = {}

---@instance
---@function [RtsNetGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function RtsNetGameMode:SendAllTextMessage(text) end
---@instance
---@function [RtsNetGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function RtsNetGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [RtsNetGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function RtsNetGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [RtsNetGameMode:GetPing]
---@return System.Int32
function RtsNetGameMode:GetPing() end
---@instance
---@function [RtsNetGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function RtsNetGameMode:IsMainPlayerLoaded() end
---@instance
---@function [RtsNetGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function RtsNetGameMode:GetPlayerPosition() end
---@instance
---@function [RtsNetGameMode:GetBattleMeta]
---@return System.String
function RtsNetGameMode:GetBattleMeta() end
return RtsNetGameMode

---@class Frontend.Runtime.Impl.GameImpl.Battle.Mode.StandardWarNetGameMode
local StandardWarNetGameMode = {}

---@instance
---@function [StandardWarNetGameMode:GetPlayerVehicleQueue]
---@return ShanghaiWindy.Core.DriveableList
function StandardWarNetGameMode:GetPlayerVehicleQueue() end
---@instance
---@function [StandardWarNetGameMode:GetAvailableVehicles]
---@return ShanghaiWindy.Core.DriveableList
function StandardWarNetGameMode:GetAvailableVehicles() end
---@instance
---@function [StandardWarNetGameMode:GetVehicleUsedNumber]
---@return System.Int32
---@param vehicleGuid System.String
function StandardWarNetGameMode:GetVehicleUsedNumber(vehicleGuid) end
---@instance
---@function [StandardWarNetGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function StandardWarNetGameMode:SendAllTextMessage(text) end
---@instance
---@function [StandardWarNetGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function StandardWarNetGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [StandardWarNetGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function StandardWarNetGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [StandardWarNetGameMode:GetPing]
---@return System.Int32
function StandardWarNetGameMode:GetPing() end
---@instance
---@function [StandardWarNetGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function StandardWarNetGameMode:IsMainPlayerLoaded() end
---@instance
---@function [StandardWarNetGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function StandardWarNetGameMode:GetPlayerPosition() end
---@instance
---@function [StandardWarNetGameMode:GetBattleMeta]
---@return System.String
function StandardWarNetGameMode:GetBattleMeta() end
return StandardWarNetGameMode

---@class Frontend.Runtime.Impl.GameImpl.Battle.Mode.TotalWarNetGameMode
local TotalWarNetGameMode = {}

---@instance
---@function [TotalWarNetGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function TotalWarNetGameMode:SendAllTextMessage(text) end
---@instance
---@function [TotalWarNetGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function TotalWarNetGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [TotalWarNetGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function TotalWarNetGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [TotalWarNetGameMode:GetPing]
---@return System.Int32
function TotalWarNetGameMode:GetPing() end
---@instance
---@function [TotalWarNetGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function TotalWarNetGameMode:IsMainPlayerLoaded() end
---@instance
---@function [TotalWarNetGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function TotalWarNetGameMode:GetPlayerPosition() end
---@instance
---@function [TotalWarNetGameMode:GetBattleMeta]
---@return System.String
function TotalWarNetGameMode:GetBattleMeta() end
return TotalWarNetGameMode

---@class Frontend.Runtime.Impl.GameImpl.Battle.Mode.WarZoneNetGameMode
local WarZoneNetGameMode = {}

---@instance
---@function [WarZoneNetGameMode:SendAllTextMessage]
---@return System.Void
---@param text System.String
function WarZoneNetGameMode:SendAllTextMessage(text) end
---@instance
---@function [WarZoneNetGameMode:SendTeammateTextMessage]
---@return System.Void
---@param text System.String
function WarZoneNetGameMode:SendTeammateTextMessage(text) end
---@instance
---@function [WarZoneNetGameMode:PickVehicle]
---@return System.Void
---@param vehicleGuid System.String
function WarZoneNetGameMode:PickVehicle(vehicleGuid) end
---@instance
---@function [WarZoneNetGameMode:GetPing]
---@return System.Int32
function WarZoneNetGameMode:GetPing() end
---@instance
---@function [WarZoneNetGameMode:IsMainPlayerLoaded]
---@return System.Boolean
function WarZoneNetGameMode:IsMainPlayerLoaded() end
---@instance
---@function [WarZoneNetGameMode:GetPlayerPosition]
---@return UnityEngine.Vector3
function WarZoneNetGameMode:GetPlayerPosition() end
---@instance
---@function [WarZoneNetGameMode:GetBattleMeta]
---@return System.String
function WarZoneNetGameMode:GetBattleMeta() end
return WarZoneNetGameMode

---@class ShanghaiWindy.Data.ZoneChain
---@field fromZone System.String
---@field toZone System.String
local ZoneChain = {}

return ZoneChain

---@class ShanghaiWindy.Data.ZoneCapturePoint
---@field defaultOwner ShanghaiWindy.Core.TeamManager+Team
---@field pointName System.String
---@field position UnityEngine.Vector3
---@field radius System.Single
local ZoneCapturePoint = {}

return ZoneCapturePoint

---@class ShanghaiWindy.Data.CaptureZone
---@field zonePoints UnityEngine.Vector3[]
---@field zoneCapturePoint ShanghaiWindy.Data.ZoneCapturePoint
---@field spawnTransformInfos ShanghaiWindy.Data.TransformInfo[]
local CaptureZone = {}

return CaptureZone

---@class ShanghaiWindy.Data.CaptureZoneModeConfig
---@field captureZoneName System.String
---@field displayName ShanghaiWindy.Core.LocalizedName
---@field mapGuid System.String
---@field captureZones ShanghaiWindy.Data.CaptureZone[]
---@field zoneChains ShanghaiWindy.Data.ZoneChain[]
---@field backgroundCameraTransformInfo ShanghaiWindy.Data.TransformInfo
local CaptureZoneModeConfig = {}

return CaptureZoneModeConfig

---@class ShanghaiWindy.Data.TransformInfo
---@field pos UnityEngine.Vector3
---@field eulerAngle UnityEngine.Vector3
local TransformInfo = {}

return TransformInfo

---@class ShanghaiWindy.Data.WarZoneCaptureZone
---@field zoneCapturePoint ShanghaiWindy.Data.ZoneCapturePoint
local WarZoneCaptureZone = {}

return WarZoneCaptureZone

---The SP economy of a war zone, so offline and a multiplayer room play by the same numbers.
---A room host cannot touch these; he only re-prices vehicles.
---@class ShanghaiWindy.Data.WarZoneSPRule
local WarZoneSPRule = {}

return WarZoneSPRule

---@class ShanghaiWindy.Data.WarZoneModeConfig
---@field warZoneName System.String
---@field displayName ShanghaiWindy.Core.LocalizedName
---@field mapGuid System.String
---@field captureZones ShanghaiWindy.Data.WarZoneCaptureZone[]
---@field backgroundCameraTransformInfo ShanghaiWindy.Data.TransformInfo
---@field spRule ShanghaiWindy.Data.WarZoneSPRule
local WarZoneModeConfig = {}

return WarZoneModeConfig

---@class ShanghaiWindy.Data.WaveAttackInfo
---@field comment System.String
---@field vehicleGuid System.String
---@field canShowWayPoint System.Boolean
---@field waveWayPoints UnityEngine.Vector3[]
---@field waveTransformInfos ShanghaiWindy.Data.TransformInfo[]
local WaveAttackInfo = {}

return WaveAttackInfo

---@class ShanghaiWindy.Data.WaveInfo
---@field friendWaveList ShanghaiWindy.Data.WaveAttackInfo[]
---@field enemyWaveList ShanghaiWindy.Data.WaveAttackInfo[]
---@field destSize UnityEngine.Vector3
---@field destTransformInfo ShanghaiWindy.Data.TransformInfo
local WaveInfo = {}

return WaveInfo

---@class ShanghaiWindy.Data.WaveMissionConfig
---@field missionName System.String
---@field displayName ShanghaiWindy.Core.LocalizedName
---@field mapGuid System.String
---@field maxEscapeEnemyCount System.Int32
---@field playerTeam ShanghaiWindy.Core.TeamManager+Team
---@field playerVehicleType ShanghaiWindy.Data.EPlayerVehicleType
---@field playerPickMinRank System.Int32
---@field playerPickMaxRank System.Int32
---@field playerTransformInfo ShanghaiWindy.Data.TransformInfo
---@field waveInfos ShanghaiWindy.Data.WaveInfo[]
local WaveMissionConfig = {}

return WaveMissionConfig

---@class ShanghaiWindy.Data.MaterialConfig
local MaterialConfig = {}

return MaterialConfig

---雷达工作模式：关机、搜索、单目标跟踪、格斗、边扫边跟。
---Radar work mode: off, search, single-target track, dogfight (auto-lock), track while scan.
---@class ShanghaiWindy.Core.ERadarWorkMode
local ERadarWorkMode = {}

return ERadarWorkMode

---雷达告警（RWR）收到的威胁类型：被搜索扫到、被锁定跟踪、有导弹来袭。
---Threat a radar warning receiver reports: painted by a search, locked by a track, a missile inbound.
---@class ShanghaiWindy.Core.ERWRThreatType
local ERWRThreatType = {}

return ERWRThreatType

---@class ShanghaiWindy.Core.ObjectPoolComponent
local ObjectPoolComponent = {}

---从对象池取物体
---Get object from the pool
---@instance
---@function [ObjectPoolComponent:Get]
---@return UnityEngine.GameObject
---@param stayWorldPos System.Boolean
function ObjectPoolComponent:Get(stayWorldPos) end
---回收对象池物体
---Return object back to the pool
---@instance
---@function [ObjectPoolComponent:Release]
---@param instance UnityEngine.GameObject
function ObjectPoolComponent:Release(instance) end
---回收所以对象池物体
---Return all the objects back to the pool
---@instance
---@function [ObjectPoolComponent:Clear]
function ObjectPoolComponent:Clear() end
---@instance
---@function [ObjectPoolComponent:GetObjectList]
---@return System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ObjectPoolComponent:GetObjectList() end
---释放对象池
---Dispose the pool
---@instance
---@function [ObjectPoolComponent:Dispose]
function ObjectPoolComponent:Dispose() end
return ObjectPoolComponent

---@class ShanghaiWindy.Core.VehicleInfo
local VehicleInfo = {}

---@instance
---@function [VehicleInfo:GetDisplayName]
---@return System.String
function VehicleInfo:GetDisplayName() end
---@instance
---@function [VehicleInfo:GetDescription]
---@return System.String
function VehicleInfo:GetDescription() end
---@instance
---@function [VehicleInfo:GetVehicleName]
---@return System.String
function VehicleInfo:GetVehicleName() end
---@instance
---@function [VehicleInfo:GetLoadGUID]
---@return System.String
function VehicleInfo:GetLoadGUID() end
---@instance
---@function [VehicleInfo:GetRepairCost]
---@return System.Int32
---@param isMultiplayer System.Boolean
function VehicleInfo:GetRepairCost(isMultiplayer) end
---@instance
---@function [VehicleInfo:IsSupportModuleMode]
---@return System.Boolean
function VehicleInfo:IsSupportModuleMode() end
---@instance
---@function [VehicleInfo:GetRank]
---@return System.Int32
function VehicleInfo:GetRank() end
---@instance
---@function [VehicleInfo:GetPreferSpawnPointType]
---@return ShanghaiWindy.Core.ESpawnPointType
function VehicleInfo:GetPreferSpawnPointType() end
---@instance
---@function [VehicleInfo:GetVehicleProduceYear]
---@return System.Int32
function VehicleInfo:GetVehicleProduceYear() end
return VehicleInfo

---@class ShanghaiWindy.Core.CaptureZoneTask
---@field ZoneId System.Int32
---@field Weight System.Single
local CaptureZoneTask = {}

return CaptureZoneTask

---基本初始化系统类，用于游戏任何载具的基本初始化。该类实现了 IDynamicDataPatchable 接口和 IIndex 接口。
---The BaseInitSystem class is used for basic initialization of any vehicle. This class implements the IDynamicDataPatchable and IIndex interfaces.
---@class ShanghaiWindy.Core.BaseInitSystem
---@field basePawn ShanghaiWindy.Core.IPawn @基础玩家状态类，用于处理玩家状态的改变，如受到伤害、击中反弹等事件。             Base player state class for handling changes in player states such as taking damage, ricocheting hits, etc.
---@field equipmentBuffDataList System.Collections.Generic.List`1[ShanghaiWindy.Core.EquipmentBuffData] @装备 Buff 数据列表             Equipment Buff Data List
---@field OnVehicleLoaded UnityEngine.Events.UnityEvent
---@field OnVehiclePreLoaded UnityEngine.Events.UnityEvent
---@field OnVehicleDestroyed UnityEngine.Events.UnityEvent
---@field OnGameObjectDestroyed UnityEngine.Events.UnityEvent
---@field OnDamagedInternalModule ShanghaiWindy.Core.BaseInitSystem+DamagedInternalModule
---@field VehicleName System.String
---@field VehicleInfo ShanghaiWindy.Core.VehicleInfo
---@field OwnerTeam ShanghaiWindy.Core.TeamManager+Team
---@field PlayerAvatarInfo ShanghaiWindy.Core.ClientAvatarInfo
---@field OwnerBattlePlayer ShanghaiWindy.Core.AbstractBattlePlayer
---@field IsDestroyed System.Boolean
---@field IsLoaded System.Boolean
local BaseInitSystem = {}

return BaseInitSystem

---@class ShanghaiWindy.Core.ECaptureStage
local ECaptureStage = {}

return ECaptureStage

---@class ShanghaiWindy.Core.CaptureZoneInfo
---@field zoneName System.String
---@field point UnityEngine.Vector3
---@field radius System.Single
---@field currentCaptureProgress System.Single
---@field capturingTeam ShanghaiWindy.Core.TeamManager+Team
---@field captureStage ShanghaiWindy.Core.ECaptureStage
local CaptureZoneInfo = {}

---@instance
---@function [CaptureZoneInfo:IsComplete]
---@return System.Boolean
function CaptureZoneInfo:IsComplete() end
---@instance
---@function [CaptureZoneInfo:GetIndex]
---@return System.Int64
function CaptureZoneInfo:GetIndex() end
return CaptureZoneInfo

---@class ShanghaiWindy.Core.EIconPosition
local EIconPosition = {}

return EIconPosition

---战斗玩家数据
---Battle Player
---@class ShanghaiWindy.Core.AbstractBattlePlayer
---@field Uid System.Int32 @唯一 Id             Uid
---@field NickName System.String @昵称             Nick name
---@field AvatarInfo ShanghaiWindy.Core.ClientAvatarInfo @角色头像             Avatar
---@field VehicleInfo ShanghaiWindy.Core.VehicleInfo @载具信息             Vehicle Info
---@field Vehicle ShanghaiWindy.Core.BaseInitSystem @载具（主单位，只由带 Life 计分的单位写入）             Vehicle (the primary unit; only units scoring a Life write it)
---@field Info System.Object
---@field OnVehicleLoaded UnityEngine.Events.UnityEvent
---@field OnVehicleDestroyed UnityEngine.Events.UnityEvent
---@field OnGameObjectDestroyed UnityEngine.Events.UnityEvent
---@field IsAlive System.Boolean
local AbstractBattlePlayer = {}

---@instance
---@function [AbstractBattlePlayer:IsLocalPlayer]
---@return System.Boolean
function AbstractBattlePlayer:IsLocalPlayer() end
---@instance
---@function [AbstractBattlePlayer:GetVehicleName]
---@return System.String
function AbstractBattlePlayer:GetVehicleName() end
---获取总伤害
---Get total damaged
---@instance
---@function [AbstractBattlePlayer:GetTotalDamage]
---@return System.Int32
function AbstractBattlePlayer:GetTotalDamage() end
---获取总击毁数
---Get total destroyed number
---@instance
---@function [AbstractBattlePlayer:GetTotalDestroyed]
---@return System.Int32
function AbstractBattlePlayer:GetTotalDestroyed() end
---创建载具
---Create Vehicle
---@instance
---@function [AbstractBattlePlayer:CreateVehicle]
---@return ShanghaiWindy.Core.BaseInitSystem
---@param vehicleInfo ShanghaiWindy.Core.VehicleInfo
---@param pos UnityEngine.Vector3
---@param rot UnityEngine.Quaternion
---@param onPreBind System.Action`1[[ShanghaiWindy.Core.BaseInitSystem, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function AbstractBattlePlayer:CreateVehicle(vehicleInfo, pos, rot, onPreBind) end
---获得队伍
---Get Team
---@instance
---@function [AbstractBattlePlayer:GetTeam]
---@return ShanghaiWindy.Core.TeamManager+Team
function AbstractBattlePlayer:GetTeam() end
return AbstractBattlePlayer

---@class ShanghaiWindy.Core.TeamManager
local TeamManager = {}

return TeamManager

---@class ShanghaiWindy.Core.UIEnum
local UIEnum = {}

return UIEnum

---@class ShanghaiWindy.Core.TankFire
---@field canControl System.Boolean @是否可控制             Can Control
---@field useGravity System.Boolean @使用重力             Use Gravity
---@field projectile ShanghaiWindy.Core.ProjectileManager @投射物管理器             Projectile Manager
---@field fireTransforms System.Collections.Generic.List`1[ShanghaiWindy.Core.TankFireTransformInstance] @开火位置             Tank Fire Transforms
---@field mainBody UnityEngine.Transform
---@field fireRecoilPoint UnityEngine.Transform
---@field attachedRigidbody UnityEngine.Rigidbody
---@field isAutoCalculateGravity System.Boolean
---@field isExtraTurret System.Boolean
---@field ffPoint UnityEngine.Transform
---@field fireEffectPoint UnityEngine.Transform
---@field gunDym UnityEngine.Transform
local TankFire = {}

return TankFire

---坦克初始化系统
---Tank Initialization System
---@class ShanghaiWindy.Core.VehicleInitSystem
---@field groundType ShanghaiWindy.Core.EGroundControlType @载具类型
---@field InstanceMesh UnityEngine.GameObject @实例网格             Instance Mesh
---@field vehicleTextData ShanghaiWindy.Core.VehicleTextData @车辆文本数据             Vehicle Text Data
---@field referenceManager ShanghaiWindy.Core.VehicleComponentsReferenceManager @车辆组件引用管理器             Vehicle Components Reference Manager
---@field diyReferenceManager ShanghaiWindy.Core.DIYVehicleComponentsReferenceManager @DIY 车辆组件引用管理器             DIY Vehicle Components Reference Manager
---@field vehicleComponents ShanghaiWindy.Core.VehicleComponents @车辆组件集合             Vehicle Components Collection
---@field thinkLogic ShanghaiWindy.Core.BotLogic @机器人逻辑             Bot Logic
---@field vehicleRemoveManagerModule ShanghaiWindy.Core.VehicleRemoveManagerModule @车辆移除管理模块             Vehicle Remove Manager Module
---@field basePawn ShanghaiWindy.Core.IPawn @基础玩家状态类，用于处理玩家状态的改变，如受到伤害、击中反弹等事件。             Base player state class for handling changes in player states such as taking damage, ricocheting hits, etc.
---@field equipmentBuffDataList System.Collections.Generic.List`1[ShanghaiWindy.Core.EquipmentBuffData] @装备 Buff 数据列表             Equipment Buff Data List
---@field CurEngineParam ShanghaiWindy.Core.TankControllerParameter
---@field CurMainTankFireParam ShanghaiWindy.Core.TankFireParameter
---@field CurMainTurretParam ShanghaiWindy.Core.TurretParameter
---@field CurPlayerStateParam ShanghaiWindy.Core.PlayerStateParameter
---@field CurWaterPassiveParam ShanghaiWindy.Core.Data.PassiveWaterInteractParameter
---@field OnVehicleLoaded UnityEngine.Events.UnityEvent
---@field OnVehiclePreLoaded UnityEngine.Events.UnityEvent
---@field OnVehicleDestroyed UnityEngine.Events.UnityEvent
---@field OnGameObjectDestroyed UnityEngine.Events.UnityEvent
---@field OnDamagedInternalModule ShanghaiWindy.Core.BaseInitSystem+DamagedInternalModule
---@field VehicleName System.String
---@field VehicleInfo ShanghaiWindy.Core.VehicleInfo
---@field OwnerTeam ShanghaiWindy.Core.TeamManager+Team
---@field PlayerAvatarInfo ShanghaiWindy.Core.ClientAvatarInfo
---@field OwnerBattlePlayer ShanghaiWindy.Core.AbstractBattlePlayer
---@field IsDestroyed System.Boolean
---@field IsLoaded System.Boolean
local VehicleInitSystem = {}

return VehicleInitSystem

---车辆组件
---Vehicle Components
---@class ShanghaiWindy.Core.VehicleComponents
---@field multiInputWrapper ShanghaiWindy.Core.MultiInputWrapperManageComponent @多输入映射管理组件             Multi Input Mapping Manage Component
---@field trackController ShanghaiWindy.Core.TrackController @坦克履带控制器             Tank Track Controller
---@field playerCamera ShanghaiWindy.Core.VehicleCameraPlugin @玩家相机控制器             Player Camera Controller
---@field mainCamera UnityEngine.Camera @主相机             Main Camera
---@field mainTankFireIndex ShanghaiWindy.Core.PropValue`1[System.Int32] @主坦克火力索引             Main Tank Fire Index
---@field tankfireList ShanghaiWindy.Core.ObservedList`1[ShanghaiWindy.Core.TankFire] @坦克火力列表             Tank Fire List
---@field indexTankFireMap System.Collections.Generic.Dictionary`2[System.Int64,ShanghaiWindy.Core.TankFire] @坦克火力索引与火力的映射             Tank Fire Index and Fire Mapping
---@field selfExplosionList System.Collections.Generic.List`1[ShanghaiWindy.Core.VehicleSelfExplosionFireSystem] @自爆火力系统列表             Vehicle Self-Explosion Fire System List
---@field turretControllerList ShanghaiWindy.Core.ObservedList`1[ShanghaiWindy.Core.TurretController] @炮塔控制器列表             Turret Controller List
---@field identity ShanghaiWindy.Core.Identity @身份信息             Identity
---@field baseVehiclePawn ShanghaiWindy.Core.IVehiclePawn @坦克状态             Tank State
---@field battleVehiclePawn ShanghaiWindy.Core.BattleVehiclePawn
---@field basePawn ShanghaiWindy.Core.IPawn @基本玩家状态             Base Player State
---@field damageStickManager ShanghaiWindy.Core.VehicleDamageStickManager @车辆伤害棒管理器             Vehicle Damage Stick Manager
---@field HitBoxes System.Collections.Generic.List`1[ShanghaiWindy.Core.HitBox] @碰撞箱列表             Hit Box List
---@field rigidbody UnityEngine.Rigidbody @刚体组件             Rigidbody Component
---@field vehicleFireCrossEffect ShanghaiWindy.Core.VehicleFireCrossEffect @车辆火力交叉特效             Vehicle Fire Cross Effect
---@field renderers UnityEngine.Renderer[] @渲染器数组             Renderer Array
---@field mainBodyTransform UnityEngine.Transform @主体变换组件             Main Body Transform Component
---@field tracksControllers System.Collections.Generic.List`1[ShanghaiWindy.Core.SplineTrackComponent] @履带控制器列表             Tracks Controller List
---@field tankFireFireGroupManager ShanghaiWindy.Core.TankFireFireGroupManager @坦克火力分组管理器             Tank Fire Group Manager
---@field groundAARadarSystem ShanghaiWindy.Core.GroundAARadarSystem
---@field engineFmodSoundModules System.Collections.Generic.List`1[ShanghaiWindy.Core.EngineFmodSoundModule]
---@field passiveWaterInteractComponent ShanghaiWindy.Core.PassiveWaterInteractComponent
---@field tankFireGuids ShanghaiWindy.Core.Framework.BiDirectionalDictionary`2[ShanghaiWindy.Core.TankFire,System.String]
---@field turretControllerGuids ShanghaiWindy.Core.Framework.BiDirectionalDictionary`2[ShanghaiWindy.Core.TurretController,System.String]
---@field attachments System.Collections.Generic.List`1[ShanghaiWindy.Core.AbstractVehicleAttachment]
---@field vehicleController ShanghaiWindy.Core.IVehicleController
---@field mainTankFire ShanghaiWindy.Core.TankFire
---@field mainTurretController ShanghaiWindy.Core.TurretController
local VehicleComponents = {}

return VehicleComponents

---@class ShanghaiWindy.Core.TurretController
---@field turretParams ShanghaiWindy.Core.TurretParameter
---@field InitialTrans ShanghaiWindy.Core.VirtualTransform
---@field isLocked System.Boolean
---@field fireAngle System.Single
---@field turretParamProvider ShanghaiWindy.Core.Interface.ITurretParamProvider
---@field isLocalPlayer System.Boolean
---@field turretTransform UnityEngine.Transform
---@field gunTransform UnityEngine.Transform
---@field dymTransform UnityEngine.Transform
---@field isTargetWithinRange System.Boolean
---@field maxElevation System.Single
---@field maxDepression System.Single
---@field turretTraverseAngle System.Single
---@field elevationAngle System.Single
---@field TurretSpeedBuffedCoff ShanghaiWindy.Core.BuffProperty
---@field GunSpeedBuffedCoff ShanghaiWindy.Core.BuffProperty
local TurretController = {}

return TurretController

---@class ShanghaiWindy.Core.MultiInputWrapperManageComponent
---@field isShipControllerActive ShanghaiWindy.Core.PropValue`1[System.Boolean]
local MultiInputWrapperManageComponent = {}

return MultiInputWrapperManageComponent

---Lua 模组接口，用于定义模组的基本信息。
---Lua mod interface, providing basic information about the mod.
---@class ShanghaiWindy.Core.Lua.ILuaBase
---@field modName System.String
---@field author System.String
---@field description System.String
---@field isPreview System.Boolean
---@field isDefinitiveOnly System.Boolean
local ILuaBase = {}

return ILuaBase

---@class ShanghaiWindy.Core.Lua.ILuaBehavior
local ILuaBehavior = {}

return ILuaBehavior

---@class ShanghaiWindy.Core.Lua.ILuaBuffCaster
local ILuaBuffCaster = {}

return ILuaBuffCaster

---Vehicle() 定义下的类型接口。Lua 游戏载具模块接口，用于定义游戏载具的行为和逻辑。
---Interface for Vehicle() define. Lua vehicle module interface, providing vehicle behaviors and logic.
---@class ShanghaiWindy.Core.Lua.ILuaVehicleController
local ILuaVehicleController = {}

return ILuaVehicleController

---@class ShanghaiWindy.Core.Lua.LuaBehaviorMono
local LuaBehaviorMono = {}

return LuaBehaviorMono

---子弹发射信息
---Bullet Fired Info
---@class ShanghaiWindy.Core.Data.BulletFiredInfo
local BulletFiredInfo = {}

return BulletFiredInfo

---资源加载完成时的委托
---Asset Loaded Delegate
---@class ShanghaiWindy.Core.Delegate.InstantiateNonPoolObjectDelegate
local InstantiateNonPoolObjectDelegate = {}

return InstantiateNonPoolObjectDelegate

---@class ShanghaiWindy.Core.Delegate.LoadPoolAssetDelegate
local LoadPoolAssetDelegate = {}

return LoadPoolAssetDelegate

---当载具资源和组件加载时触发全局游戏事件。
---Dispatch global event when vehicle assets and components are loaded.
---@class ShanghaiWindy.Core.Delegate.OnGameVehicleLoadedDelegate
local OnGameVehicleLoadedDelegate = {}

return OnGameVehicleLoadedDelegate

---当载具被摧毁时触发全局游戏事件。
---Dispatch global event when vehicle is destroyed.
---@class ShanghaiWindy.Core.Delegate.OnGameVehicleDestroyedDelegate
local OnGameVehicleDestroyedDelegate = {}

return OnGameVehicleDestroyedDelegate

---当载具物体被摧毁时触发全局游戏事件。
---Dispatch global event when vehicle gameobject is destroyed.
---@class ShanghaiWindy.Core.Delegate.OnGameVehicleGameObjectRemovedDelegate
local OnGameVehicleGameObjectRemovedDelegate = {}

return OnGameVehicleGameObjectRemovedDelegate

---按键执行时的委托
---Key Performed Delegate
---@class ShanghaiWindy.Core.Delegate.OnKeyPerformed
local OnKeyPerformed = {}

return OnKeyPerformed

---按键取消时的委托
---Key Canceled Delegate
---@class ShanghaiWindy.Core.Delegate.OnKeyCanceled
local OnKeyCanceled = {}

return OnKeyCanceled

---@class ShanghaiWindy.Core.Delegate.OnKeyRegistered
local OnKeyRegistered = {}

return OnKeyRegistered

---@class ShanghaiWindy.Core.Delegate.OnKeyUnregistered
local OnKeyUnregistered = {}

return OnKeyUnregistered

---无参数的事件回调。
---Event callback without arguments.
---@class ShanghaiWindy.Core.Delegate.OnLuaEventDelegate
local OnLuaEventDelegate = {}

return OnLuaEventDelegate

---带一辆载具的事件回调。
---Event callback with a vehicle.
---@class ShanghaiWindy.Core.Delegate.OnLuaVehicleEventDelegate
local OnLuaVehicleEventDelegate = {}

return OnLuaVehicleEventDelegate

---带一个布尔值的事件回调。
---Event callback with a boolean.
---@class ShanghaiWindy.Core.Delegate.OnLuaBoolEventDelegate
local OnLuaBoolEventDelegate = {}

return OnLuaBoolEventDelegate

---带一个整数的事件回调。
---Event callback with an integer.
---@class ShanghaiWindy.Core.Delegate.OnLuaIntEventDelegate
local OnLuaIntEventDelegate = {}

return OnLuaIntEventDelegate

---带一个浮点数的事件回调。
---Event callback with a number.
---@class ShanghaiWindy.Core.Delegate.OnLuaFloatEventDelegate
local OnLuaFloatEventDelegate = {}

return OnLuaFloatEventDelegate

---武器命中造成伤害时的回调。
---Callback when a weapon's hit deals damage.
---@class ShanghaiWindy.Core.Delegate.OnLuaHitEventDelegate
local OnLuaHitEventDelegate = {}

return OnLuaHitEventDelegate

---内部模块状态变化的回调。
---Callback when an internal module changes state.
---@class ShanghaiWindy.Core.Delegate.OnLuaModuleStateEventDelegate
local OnLuaModuleStateEventDelegate = {}

return OnLuaModuleStateEventDelegate

---雷达锁定变化的回调。
---Callback when a radar lock changes.
---@class ShanghaiWindy.Core.Delegate.OnLuaRadarLockEventDelegate
local OnLuaRadarLockEventDelegate = {}

return OnLuaRadarLockEventDelegate

---雷达工作模式变化的回调。
---Callback when a radar's work mode changes.
---@class ShanghaiWindy.Core.Delegate.OnLuaRadarModeEventDelegate
local OnLuaRadarModeEventDelegate = {}

return OnLuaRadarModeEventDelegate

---雷达告警（RWR）收到威胁时的回调。
---Callback when the radar warning receiver gets a threat.
---@class ShanghaiWindy.Core.Delegate.OnLuaRadarThreatEventDelegate
local OnLuaRadarThreatEventDelegate = {}

return OnLuaRadarThreatEventDelegate

---@class ShanghaiWindy.Core.Delegate.OnBattleSceneLoadedDelegate
local OnBattleSceneLoadedDelegate = {}

return OnBattleSceneLoadedDelegate

---@class ShanghaiWindy.Core.Delegate.OnBattleUILoadedDelegate
local OnBattleUILoadedDelegate = {}

return OnBattleUILoadedDelegate

---音频事件加载完成时的委托
---Post Event Loaded Delegate
---@class ShanghaiWindy.Core.Delegate.PostEventLoadedDelegate
local PostEventLoadedDelegate = {}

return PostEventLoadedDelegate

---@class ShanghaiWindy.Core.Delegate.OnQuarterTickDelegate
local OnQuarterTickDelegate = {}

return OnQuarterTickDelegate

---@class ShanghaiWindy.Core.Delegate.OnTickDelegate
local OnTickDelegate = {}

return OnTickDelegate

---当载具资源和组件加载时触发事件。
---Dispatch event when vehicle assets and components are loaded.
---@class ShanghaiWindy.Core.Delegate.OnVehicleLoadedDelegate
local OnVehicleLoadedDelegate = {}

return OnVehicleLoadedDelegate

---当载具被摧毁时触发事件。
---Dispatch event when vehicle is destroyed.
---@class ShanghaiWindy.Core.Delegate.OnVehicleDestroyedDelegate
local OnVehicleDestroyedDelegate = {}

return OnVehicleDestroyedDelegate

---当载具从场景中移除时触发事件。
---Dispatch event when vehicle is removed from scene.
---@class ShanghaiWindy.Core.Delegate.OnVehicleRemovedDelegate
local OnVehicleRemovedDelegate = {}

return OnVehicleRemovedDelegate

---当载具开火时触发事件。
---Dispatch event when vehicle is fired.
---@class ShanghaiWindy.Core.Delegate.OnFiredDelegate
local OnFiredDelegate = {}

return OnFiredDelegate

---载具缩略图加载完毕回调
---@class ShanghaiWindy.Core.Delegate.OnVehicleThumbnailLoadedDelegate
local OnVehicleThumbnailLoadedDelegate = {}

return OnVehicleThumbnailLoadedDelegate

---陆军相关 API
---Army-related API
---@class ShanghaiWindy.Core.API.ArmyAPI
local ArmyAPI = {}

---从陆军载具中获取所有武器控制器。
---Get all the weapon controllers from the army vehicle.
---@static
---@function [ArmyAPI.GetWeaponList]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.FPS.ArmyWeapon, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] 武器控制器列表 List of weapon controllers
---@param vehicle ShanghaiWindy.Core.FPS.ArmyInitSystem
function ArmyAPI.GetWeaponList(vehicle) end
return ArmyAPI

---资源相关 API
---Asset-related API
---@class ShanghaiWindy.Core.API.AssetAPI
local AssetAPI = {}

---实例化非对象池物体
---Load non-pool asset
---@static
---@function [AssetAPI.InstantiateNonPoolObject]
---@param id System.String
---@param fileName System.String
---@param callBack ShanghaiWindy.Core.Delegate.InstantiateNonPoolObjectDelegate
function AssetAPI.InstantiateNonPoolObject(id, fileName, callBack) end
---从对象池加载资源
---Load asset from object pool
---@static
---@function [AssetAPI.LoadPoolAsset]
---@param id System.String
---@param fileName System.String
---@param callBack ShanghaiWindy.Core.Delegate.LoadPoolAssetDelegate
function AssetAPI.LoadPoolAsset(id, fileName, callBack) end
---将对方返回对象池
---Release asset to object pool
---@static
---@function [AssetAPI.ReleasePoolAsset]
---@param assetPoolRef ShanghaiWindy.Core.AssetPoolRef
function AssetAPI.ReleasePoolAsset(assetPoolRef) end
---重新加载配置表
---Reload config
---@static
---@function [AssetAPI.ReloadConfig]
function AssetAPI.ReloadConfig() end
---强制解析资源包，调用方需 await 等待资源注册全部完成
---Force resolve package — caller must await for the registrar chain
---@static
---@function [AssetAPI.ForceResolvePackageAsync]
function AssetAPI.ForceResolvePackageAsync() end
return AssetAPI

---战斗玩家
---Battle Player
---@class ShanghaiWindy.Core.API.BattlePlayerAPI
local BattlePlayerAPI = {}

---创建离线单机 Bot
---Create offline bot
---@static
---@function [BattlePlayerAPI.CreateOfflineBotPlayer]
---@return ShanghaiWindy.Core.OfflineBotBattlePlayer 离线 Bot 玩家 Offline bot player
---@param uid System.Int32
---@param nickName System.String
---@param info System.Object
function BattlePlayerAPI.CreateOfflineBotPlayer(uid, nickName, info) end
---创建离线主角
---Create main player bot
---@static
---@function [BattlePlayerAPI.CreateOfflineMainPlayer]
---@return ShanghaiWindy.Core.OfflineMainBattlePlayer 离线主角玩家 Offline main player
---@param uid System.Int32
---@param info System.Object
function BattlePlayerAPI.CreateOfflineMainPlayer(uid, info) end
---获取离线完成名称
---Get random bot name
---@static
---@function [BattlePlayerAPI.GetRandomBotName]
---@return System.String 随机名称 Random bot name
function BattlePlayerAPI.GetRandomBotName() end
---Create the free camera battle player: replay slot + registration + camera vehicle.
---The uid must not collide with ones the mode already uses.
---@static
---@function [BattlePlayerAPI.CreateFreeCameraPlayer]
---@param uid System.Int32
---@param position UnityEngine.Vector3
---@param rotation UnityEngine.Quaternion
---@param onPreBind System.Action`1[[ShanghaiWindy.Core.FreeCameraInitSystem, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function BattlePlayerAPI.CreateFreeCameraPlayer(uid, position, rotation, onPreBind) end
return BattlePlayerAPI

---Bot AI API
---Bot AI API
---@class ShanghaiWindy.Core.API.BotAPI
local BotAPI = {}

---创建一个坦克 Bot 任务逻辑。
---Create a tank bot task logic.
---@static
---@function [BotAPI.GetTankBotTaskLogic]
---@return ShanghaiWindy.Core.TankBotTaskLogic 坦克 Bot 任务逻辑 Tank bot task logic
function BotAPI.GetTankBotTaskLogic() end
---为 Bot 添加占领区任务。
---Add a capture zone task to the bot.
---@static
---@function [BotAPI.AddCaptureTaskToBot]
---@return ShanghaiWindy.Core.CaptureZoneTask 占领区任务 Capture zone task
---@param logic ShanghaiWindy.Core.TankBotTaskLogic
function BotAPI.AddCaptureTaskToBot(logic) end
return BotAPI

---Buff API
---@class ShanghaiWindy.Core.API.BuffAPI
local BuffAPI = {}

---为载具添加 Buff
---Add a buff to vehicle
---@static
---@function [BuffAPI.AddBuff]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param buffCaster ShanghaiWindy.Core.Lua.ILuaBuffCaster
function BuffAPI.AddBuff(vehicle, buffCaster) end
---为载具删除 Buff
---Remove a buff to vehicle
---@static
---@function [BuffAPI.RemoveBuff]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param buffCaster ShanghaiWindy.Core.Lua.ILuaBuffCaster
---@param isInterrupt System.Boolean
function BuffAPI.RemoveBuff(vehicle, buffCaster, isInterrupt) end
---@static
---@function [BuffAPI.TryGetBuffReceiverAsTank]
---@return System.Boolean
---@param buffReceiver ShanghaiWindy.Core.IBuffReceiver
---@param vehicleInitSystem ShanghaiWindy.Core.VehicleInitSystem&
function BuffAPI.TryGetBuffReceiverAsTank(buffReceiver, vehicleInitSystem) end
---@static
---@function [BuffAPI.TryGetBuffReceiverAsFlight]
---@return System.Boolean
---@param buffReceiver ShanghaiWindy.Core.IBuffReceiver
---@param flightInitSystem ShanghaiWindy.Core.FlightInitSystem&
function BuffAPI.TryGetBuffReceiverAsFlight(buffReceiver, flightInitSystem) end
---@static
---@function [BuffAPI.TryGetBuffReceiverAsArmy]
---@return System.Boolean
---@param buffReceiver ShanghaiWindy.Core.IBuffReceiver
---@param armyInitSystem ShanghaiWindy.Core.FPS.ArmyInitSystem&
function BuffAPI.TryGetBuffReceiverAsArmy(buffReceiver, armyInitSystem) end
---创建一个数值可变的 buff。用属性自带的 AddBuff 挂上一次（例如 weapon.DamageBuffedCoff:AddBuff(buff)），之后只调用
---buff:SetValue(x)：属性原地刷新，不用反复移除再新建。不用时记得 RemoveBuff。
---Create a buff whose value can change. Attach it once with the stat's own AddBuff (for example
---weapon.DamageBuffedCoff:AddBuff(buff)), then only call buff:SetValue(x): the stat refreshes in place, with no removing and
---re-creating. RemoveBuff it when done.
---@static
---@function [BuffAPI.CreateValueBuff]
---@return ShanghaiWindy.Core.API.LuaValueBuff buff 对象 The buff
---@param displayName System.String
---@param isMultiplier System.Boolean
---@param value System.Single
function BuffAPI.CreateValueBuff(displayName, isMultiplier, value) end
return BuffAPI

---摄像机 API
---Camera API
---@class ShanghaiWindy.Core.API.CameraAPI
local CameraAPI = {}

---获取摄像机 Transform
---Get camera transform
---@static
---@function [CameraAPI.GetCameraTransform]
---@return UnityEngine.Transform 摄像机 Transform Camera transform
function CameraAPI.GetCameraTransform() end
---设置背景摄像机位置
---Set background camera position
---@static
---@function [CameraAPI.SetBackgroundCameraPosition]
---@param position UnityEngine.Vector3
function CameraAPI.SetBackgroundCameraPosition(position) end
---设置背景摄像机旋转
---Set background camera euler angles
---@static
---@function [CameraAPI.SetBackgroundCameraEulerAngles]
---@param eulerAngles UnityEngine.Vector3
function CameraAPI.SetBackgroundCameraEulerAngles(eulerAngles) end
---获取背景摄像机
---Get background camera
---@static
---@function [CameraAPI.GetBackGroundCamera]
---@return UnityEngine.Camera 背景摄像机 Background camera
function CameraAPI.GetBackGroundCamera() end
---屏幕位置转为世界坐标
---Screen point to world point
---@static
---@function [CameraAPI.ScreenToWorldPoint]
---@return UnityEngine.Vector3 世界坐标 World point
---@param camera UnityEngine.Camera
---@param screenPoint UnityEngine.Vector3
function CameraAPI.ScreenToWorldPoint(camera, screenPoint) end
---世界坐标转为屏幕位置
---World point to screen point
---@static
---@function [CameraAPI.WorldToScreenPoint]
---@return UnityEngine.Vector3 屏幕坐标 Screen point
---@param camera UnityEngine.Camera
---@param worldPoint UnityEngine.Vector3
function CameraAPI.WorldToScreenPoint(camera, worldPoint) end
---获取当前的游戏摄像机
---Get the current main game camera
---@static
---@function [CameraAPI.GetGameCamera]
---@return UnityEngine.Camera 游戏摄像机 Game camera
function CameraAPI.GetGameCamera() end
---@static
---@function [CameraAPI.GetScreenPosition]
---@return UnityEngine.Vector3
---@param worldPos UnityEngine.Vector3
---@param iconPosition ShanghaiWindy.Core.EIconPosition&
function CameraAPI.GetScreenPosition(worldPos, iconPosition) end
---当前玩家相机的模式（第三人称、瞄准镜、望远镜、车内……）。没有玩家相机时返回 Unknown。
---The mode of the player's camera (third person, sight, binocular, interior …). Unknown when there is no player camera.
---@static
---@function [CameraAPI.GetCameraMode]
---@return ShanghaiWindy.Core.EPlayerCameraMode 相机模式 Camera mode
function CameraAPI.GetCameraMode() end
---玩家是否开镜（地面载具的放大状态）。玩家不在地面载具上时返回 false。
---Whether the player is zoomed in (a ground vehicle's zoom). False when the player is not in a ground vehicle.
---@static
---@function [CameraAPI.IsZoomed]
---@return System.Boolean 是否开镜 Whether zoomed in
function CameraAPI.IsZoomed() end
---玩家相机当前锁定的载具（地面载具的瞄准锁定），没有锁定时返回 nil。
---The vehicle the player's camera is locked on (a ground vehicle's aim lock), or nil when nothing is locked.
---@static
---@function [CameraAPI.GetLockTarget]
---@return ShanghaiWindy.Core.BaseInitSystem 锁定的载具 Locked vehicle
function CameraAPI.GetLockTarget() end
return CameraAPI

---占领区 API
---Capture Zone API
---@class ShanghaiWindy.Core.API.CaptureZoneAPI
local CaptureZoneAPI = {}

---增加一个可占领区域
---Add a capture zone
---@static
---@function [CaptureZoneAPI.AddCaptureZone]
---@return System.Int64 占领区 ID Capture zone ID
---@param zoneName System.String
---@param point UnityEngine.Vector3
---@param radius System.Single
function CaptureZoneAPI.AddCaptureZone(zoneName, point, radius) end
---@static
---@function [CaptureZoneAPI.CapturingZone]
---@return System.Void
---@param id System.Int32
---@param team ShanghaiWindy.Core.TeamManager+Team
---@param delta System.Single
function CaptureZoneAPI.CapturingZone(id, team, delta) end
---@static
---@function [CaptureZoneAPI.SetCapturingZoneStatus]
---@return System.Void
---@param id System.Int64
---@param progress System.Single
---@param capturingTeam ShanghaiWindy.Core.TeamManager+Team
---@param stage ShanghaiWindy.Core.ECaptureStage
function CaptureZoneAPI.SetCapturingZoneStatus(id, progress, capturingTeam, stage) end
---获取占领点
---Get capture zone
---@static
---@function [CaptureZoneAPI.GetCaptureZone]
---@return ShanghaiWindy.Core.CaptureZoneInfo 占领区信息 Capture zone info
---@param id System.Int64
function CaptureZoneAPI.GetCaptureZone(id) end
---从名称获取占领点
---Get capture zone from index
---@static
---@function [CaptureZoneAPI.GetCaptureZoneFromName]
---@return ShanghaiWindy.Core.CaptureZoneInfo 占领区信息 Capture zone info
---@param name System.String
function CaptureZoneAPI.GetCaptureZoneFromName(name) end
---销毁一个占领区域
---Remove a capture zone
---@static
---@function [CaptureZoneAPI.RemoveCaptureZone]
---@param id System.Int32
function CaptureZoneAPI.RemoveCaptureZone(id) end
---获取占领点列表
---Get capture zone infos
---@static
---@function [CaptureZoneAPI.GetCaptureZoneInfos]
---@return ShanghaiWindy.Core.CaptureZoneInfo[] 占领区信息数组 Capture zone info array
function CaptureZoneAPI.GetCaptureZoneInfos() end
---在占领点脚下画一圈高亮环
---Draw the highlight ring under a capture zone
---@static
---@function [CaptureZoneAPI.DrawZoneCircle]
---@return UnityEngine.GameObject 环的 GameObject The ring's GameObject
---@param center UnityEngine.Vector3
---@param radius System.Single
function CaptureZoneAPI.DrawZoneCircle(center, radius) end
---给区域网格套上占领点高亮材质
---Put the zone highlight material on every renderer under the mesh
---@static
---@function [CaptureZoneAPI.ApplyZoneMaterial]
---@param instance UnityEngine.GameObject
function CaptureZoneAPI.ApplyZoneMaterial(instance) end
return CaptureZoneAPI

---颜色 API
---Color API
---@class ShanghaiWindy.Core.API.ColorAPI
local ColorAPI = {}

---通过 RGBA 值创建颜色。
---Create a color from RGBA values.
---@static
---@function [ColorAPI.GetColor]
---@return UnityEngine.Color 颜色 Color
---@param r System.Single
---@param g System.Single
---@param b System.Single
---@param a System.Single
function ColorAPI.GetColor(r, g, b, a) end
return ColorAPI

---@class ShanghaiWindy.Core.API.ComponentAPI
local ComponentAPI = {}

---根据类名获取指定 GameObject 上的 Lua 组件。
---Gets a Lua component with the specified class name on the given GameObject.
---@static
---@function [ComponentAPI.GetLuaComponent]
---@return XLua.LuaTable 指定的 Lua 组件，如果不存在则返回 null。The specified Lua component, or null if it does not exist.
---@param target UnityEngine.GameObject
---@param className System.String
function ComponentAPI.GetLuaComponent(target, className) end
---根据类名获取指定 GameObject 上的所有 Lua 组件。
---Gets all Lua components with the specified class name on the given GameObject.
---@static
---@function [ComponentAPI.GetLuaComponents]
---@return XLua.LuaTable[] 指定的 Lua 组件数组。An array of the specified Lua components.
---@param target UnityEngine.GameObject
---@param className System.String
function ComponentAPI.GetLuaComponents(target, className) end
---在指定 GameObject 的父级中，根据类名获取所有 Lua 组件。
---Gets all Lua components with the specified class name in the parent hierarchy of the given GameObject.
---@static
---@function [ComponentAPI.GetLuaComponentsInParent]
---@return XLua.LuaTable[] 指定的 Lua 组件数组。An array of the specified Lua components.
---@param target UnityEngine.GameObject
---@param className System.String
function ComponentAPI.GetLuaComponentsInParent(target, className) end
---在指定 GameObject 的子级中，根据类名获取所有 Lua 组件。
---Gets all Lua components with the specified class name in the children of the given GameObject.
---@static
---@function [ComponentAPI.GetLuaComponentsInChildren]
---@return XLua.LuaTable[] 指定的 Lua 组件数组。An array of the specified Lua components.
---@param target UnityEngine.GameObject
---@param className System.String
function ComponentAPI.GetLuaComponentsInChildren(target, className) end
---根据组件类型名获取指定 GameObject 上的原生组件。
---Gets the native component with the specified type name on the given GameObject.
---@static
---@function [ComponentAPI.GetNativeComponent]
---@return UnityEngine.Component 指定的原生组件。The specified native component.
---@param target UnityEngine.GameObject
---@param componentName System.String
function ComponentAPI.GetNativeComponent(target, componentName) end
---根据组件类型名获取指定 GameObject 上的所有原生组件。
---Gets all native components with the specified type name on the given GameObject.
---@static
---@function [ComponentAPI.GetNativeComponents]
---@return UnityEngine.Component[] 指定的原生组件数组。An array of the specified native components.
---@param target UnityEngine.GameObject
---@param componentName System.String
function ComponentAPI.GetNativeComponents(target, componentName) end
---在指定 GameObject 的父级中，根据组件类型名获取所有原生组件。
---Gets all native components with the specified type name in the parent hierarchy of the given GameObject.
---@static
---@function [ComponentAPI.GetNativeComponentsInParent]
---@return UnityEngine.Component[] 指定的原生组件数组。An array of the specified native components.
---@param target UnityEngine.GameObject
---@param componentName System.String
function ComponentAPI.GetNativeComponentsInParent(target, componentName) end
---在指定 GameObject 的子级中，根据组件类型名获取所有原生组件。
---Gets all native components with the specified type name in the children of the given GameObject.
---@static
---@function [ComponentAPI.GetNativeComponentsInChildren]
---@return UnityEngine.Component[] 指定的原生组件数组。An array of the specified native components.
---@param target UnityEngine.GameObject
---@param componentName System.String
function ComponentAPI.GetNativeComponentsInChildren(target, componentName) end
---通过组件找到指定物体组
---Find objects by component
---@static
---@function [ComponentAPI.GetObjectsInWorld]
---@return UnityEngine.Object[]
---@param componentName System.String
function ComponentAPI.GetObjectsInWorld(componentName) end
return ComponentAPI

---配置表 API
---Config API
---@class ShanghaiWindy.Core.API.ConfigAPI
local ConfigAPI = {}

---获取防守任务配置列表
---Get wave mission config list
---@static
---@function [ConfigAPI.GetWaveMissionConfigs]
---@return ShanghaiWindy.Data.WaveMissionConfig[] 防守任务配置数组 Wave mission config array
function ConfigAPI.GetWaveMissionConfigs() end
---获取防守任务配置
---Get wave mission config
---@static
---@function [ConfigAPI.GetWaveMissionConfig]
---@return ShanghaiWindy.Data.WaveMissionConfig 防守任务配置 Wave mission config
---@param guid System.String
function ConfigAPI.GetWaveMissionConfig(guid) end
---获取占领点配置列表
---Get capture zone config list
---@static
---@function [ConfigAPI.GetCaptureZoneConfigs]
---@return ShanghaiWindy.Data.CaptureZoneModeConfig[] 占领点配置数组 Capture zone config array
function ConfigAPI.GetCaptureZoneConfigs() end
---获取占领区任务配置
---Get capture zone config
---@static
---@function [ConfigAPI.GetCaptureZoneConfig]
---@return ShanghaiWindy.Data.CaptureZoneModeConfig 占领区配置 Capture zone config
---@param guid System.String
function ConfigAPI.GetCaptureZoneConfig(guid) end
---获取战区配置列表
---Get war zone config list
---@static
---@function [ConfigAPI.GetWarZoneConfigs]
---@return ShanghaiWindy.Data.WarZoneModeConfig[] 战区配置数组 War zone config array
function ConfigAPI.GetWarZoneConfigs() end
return ConfigAPI

---将 Lua Table 转为 C# 对象
---Convert Lua Table to C# object
---@class ShanghaiWindy.Core.API.ConvertAPI
local ConvertAPI = {}

---将 Lua Class 转换为 IBuffCaster 对象
---Covert Lua Class to IBuffCaster Object
---@static
---@function [ConvertAPI.CovertToBuffCaster]
---@return ShanghaiWindy.Core.Lua.ILuaBuffCaster ILuaBuffCaster 对象 ILuaBuffCaster object
---@param luaTable XLua.LuaTable
function ConvertAPI.CovertToBuffCaster(luaTable) end
return ConvertAPI

---自定义 Option UI
---Custom Option UI
---@class ShanghaiWindy.Core.API.CustomOptionUIAPI
local CustomOptionUIAPI = {}

---切换是否显示自定义 Option UI
---Toggle Option UI
---@static
---@function [CustomOptionUIAPI.ToggleUI]
---@param state System.Boolean
function CustomOptionUIAPI.ToggleUI(state) end
---添加选项
---Add option
---@static
---@function [CustomOptionUIAPI.AddOption]
---@param optionName System.String
---@param defaultValue System.String
---@param options System.String[]
---@param onValueChanged System.Action`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
function CustomOptionUIAPI.AddOption(optionName, defaultValue, options, onValueChanged) end
---增加滑条
---Add Slider
---@static
---@function [CustomOptionUIAPI.AddSlider]
---@param optionName System.String
---@param defaultValue System.Single
---@param minVal System.Single
---@param maxVal System.Single
---@param isWholeNumbers System.Boolean
---@param onValueChanged System.Action`1[[System.Single, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
function CustomOptionUIAPI.AddSlider(optionName, defaultValue, minVal, maxVal, isWholeNumbers, onValueChanged) end
---添加按钮
---Add Button
---@static
---@function [CustomOptionUIAPI.AddButton]
---@param optionName System.String
---@param buttonName System.String
---@param onValueChanged System.Action
function CustomOptionUIAPI.AddButton(optionName, buttonName, onValueChanged) end
---添加标题
---Add Title
---@static
---@function [CustomOptionUIAPI.AddTitle]
---@param optionName System.String
function CustomOptionUIAPI.AddTitle(optionName) end
---添加文本框
---Add text
---@static
---@function [CustomOptionUIAPI.AddTextField]
---@param optionName System.String
---@param onInputChanged System.Action`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
function CustomOptionUIAPI.AddTextField(optionName, onInputChanged) end
---添加文字
---Add text
---@static
---@function [CustomOptionUIAPI.AddText]
---@param textArea System.String
function CustomOptionUIAPI.AddText(textArea) end
---清空 Options
---Clear Options
---@static
---@function [CustomOptionUIAPI.ClearOptions]
function CustomOptionUIAPI.ClearOptions() end
return CustomOptionUIAPI

---飞行器相关 API
---Flight-related API
---@class ShanghaiWindy.Core.API.FlightAPI
local FlightAPI = {}

---从飞行载具中获取所有飞行火力系统。
---Get all the flight fire systems from the flight vehicle.
---@static
---@function [FlightAPI.GetFlightFireList]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.FlightFireSystem, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] 飞行火力系统列表 List of flight fire systems
---@param vehicle ShanghaiWindy.Core.FlightInitSystem
function FlightAPI.GetFlightFireList(vehicle) end
---飞机是否着地，与游戏自己的起降判定相同：任一机轮接地；起落架放下时机轮附近有地面或建筑也算。
---起落架放下且离地时，每次调用会为每个机轮做一次物理重叠检测（不分配内存），不需要每帧调用时请降低调用频率。
---Whether the aircraft is on the ground, the same test the game uses for take-off and landing: any wheel touching
---down, or, with the gear down, ground or a building right at a wheel. With the gear down in the air each call runs
---one physics overlap per wheel (no allocation); call it less often than every frame where you can.
---@static
---@function [FlightAPI.IsOnGround]
---@return System.Boolean 是否着地；不是飞机时为 false Whether it is on the ground; false for a vehicle that is not an aircraft
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function FlightAPI.IsOnGround(vehicle) end
---起落架是否放下。
---Whether the landing gear is down.
---@static
---@function [FlightAPI.IsGearDown]
---@return System.Boolean 起落架是否放下 Whether the gear is down
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function FlightAPI.IsGearDown(vehicle) end
---获取空速（公里/小时），沿机头方向，与飞行界面显示的一致。
---Get the airspeed in km/h along the nose, as the flight HUD shows it.
---@static
---@function [FlightAPI.GetAirspeedKmh]
---@return System.Single 空速（公里/小时） Airspeed in km/h
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function FlightAPI.GetAirspeedKmh(vehicle) end
---获取高度（米），即世界坐标的 Y 值，不是离地高度。
---Get the altitude in metres: the world Y coordinate, not the height above the ground.
---@static
---@function [FlightAPI.GetAltitude]
---@return System.Single 高度（米） Altitude in metres
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function FlightAPI.GetAltitude(vehicle) end
---获取油门，0 到 120，超过 100 为加力。
---Get the throttle, 0 to 120; above 100 is boost.
---@static
---@function [FlightAPI.GetThrottle]
---@return System.Single 油门 Throttle
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function FlightAPI.GetThrottle(vehicle) end
---发动机是否在运转。
---Whether the engine is running.
---@static
---@function [FlightAPI.IsEngineRunning]
---@return System.Boolean 发动机是否运转 Whether the engine runs
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function FlightAPI.IsEngineRunning(vehicle) end
---订阅起落架收放，回调参数为起落架是否放下。
---Subscribe to the landing gear going up or down; the callback gets whether it is down.
---@static
---@function [FlightAPI.OnGearChanged]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaBoolEventDelegate
function FlightAPI.OnGearChanged(owner, vehicle, callback) end
return FlightAPI

---字体 API
---Font API
---@class ShanghaiWindy.Core.API.FontAPI
local FontAPI = {}

---替换目标对象下所有 Text 组件的字体为主字体。
---Replace all Text fonts under the target GameObject with the main font.
---@static
---@function [FontAPI.ReplaceAsMainFont]
---@param instance UnityEngine.GameObject
function FontAPI.ReplaceAsMainFont(instance) end
return FontAPI

---游戏相关全局 API
---Game related global api
---@class ShanghaiWindy.Core.API.GameAPI
local GameAPI = {}

---注册车辆生成事件
---Register Vehicl eLoaded Event
---@static
---@function [GameAPI.RegisterVehicleLoadedEvent]
---@param gameVehicleLoadedDelegate ShanghaiWindy.Core.Delegate.OnGameVehicleLoadedDelegate
function GameAPI.RegisterVehicleLoadedEvent(gameVehicleLoadedDelegate) end
---注销车辆生成事件
---UnRegister Vehicle Loaded Event
---@static
---@function [GameAPI.UnRegisterVehicleLoadedEvent]
---@param gameVehicleLoadedDelegate ShanghaiWindy.Core.Delegate.OnGameVehicleLoadedDelegate
function GameAPI.UnRegisterVehicleLoadedEvent(gameVehicleLoadedDelegate) end
---注册车辆销毁事件
---Register Vehicle Destroyed Event
---@static
---@function [GameAPI.RegisterVehicleDestroyedEvent]
---@param gameVehicleDestroyedDelegate ShanghaiWindy.Core.Delegate.OnGameVehicleDestroyedDelegate
function GameAPI.RegisterVehicleDestroyedEvent(gameVehicleDestroyedDelegate) end
---注销车辆销毁事件
---UnRegister Vehicle Destroyed Event
---@static
---@function [GameAPI.UnRegisterVehicleDestroyedEvent]
---@param gameVehicleDestroyedDelegate ShanghaiWindy.Core.Delegate.OnGameVehicleDestroyedDelegate
function GameAPI.UnRegisterVehicleDestroyedEvent(gameVehicleDestroyedDelegate) end
---注册车辆物体销毁事件
---Register Vehicle GameObject Destroyed Event
---@static
---@function [GameAPI.RegisterVehicleGameObjectDestroyedEvent]
---@param gameVehicleGameObjectDestroyedDelegate ShanghaiWindy.Core.Delegate.OnGameVehicleGameObjectRemovedDelegate
function GameAPI.RegisterVehicleGameObjectDestroyedEvent(gameVehicleGameObjectDestroyedDelegate) end
---注销车辆物体销毁事件
---UnRegister Vehicle GameObject Destroyed Event
---@static
---@function [GameAPI.UnRegisterVehicleGameObjectDestroyedEvent]
---@param gameVehicleGameObjectDestroyedDelegate ShanghaiWindy.Core.Delegate.OnGameVehicleGameObjectRemovedDelegate
function GameAPI.UnRegisterVehicleGameObjectDestroyedEvent(gameVehicleGameObjectDestroyedDelegate) end
---当前客户端提供的 Lua API 版本，与全局变量 GAME_API_VERSION 相同。没有这个函数的老客户端按版本 1 处理。
---The Lua API version of this client, the same as the global GAME_API_VERSION. Treat an older client without it as version 1.
---@static
---@function [GameAPI.GetApiVersion]
---@return System.Int32 API 版本 API version
function GameAPI.GetApiVersion() end
return GameAPI

---游戏对象 API
---GameObject API
---@class ShanghaiWindy.Core.API.GameObjectAPI
local GameObjectAPI = {}

---寻找路径物体
---Find child by path
---@static
---@function [GameObjectAPI.Find]
---@return UnityEngine.GameObject 目标子物体 Target child GameObject
---@param go UnityEngine.GameObject
---@param path System.String
function GameObjectAPI.Find(go, path) end
---切换场景时候保留物体
---Do not destroy the target Object when loading a new Scene
---@static
---@function [GameObjectAPI.DontDestroyOnLoad]
---@param go UnityEngine.GameObject
function GameObjectAPI.DontDestroyOnLoad(go) end
---复制物体
---Clone object
---@static
---@function [GameObjectAPI.Clone]
---@return UnityEngine.GameObject 复制的对象 Cloned GameObject
---@param go UnityEngine.GameObject
function GameObjectAPI.Clone(go) end
---销毁物体
---Destroy object
---@static
---@function [GameObjectAPI.DestroyObject]
---@param go UnityEngine.GameObject
function GameObjectAPI.DestroyObject(go) end
---是否激活物体
---Set object active
---@static
---@function [GameObjectAPI.SetActive]
---@param go UnityEngine.GameObject
---@param isActive System.Boolean
function GameObjectAPI.SetActive(go, isActive) end
---是否显示物体
---Set object visible
---@static
---@function [GameObjectAPI.SetVisible]
---@param go UnityEngine.GameObject
---@param isVisible System.Boolean
function GameObjectAPI.SetVisible(go, isVisible) end
return GameObjectAPI

---装甲碰撞盒 API
---HitBox Armor API
---@class ShanghaiWindy.Core.API.HitBoxAPI
local HitBoxAPI = {}

---从 GameObject 获取 HitBoxComponent。
---Get HitBoxComponent from a GameObject.
---@static
---@function [HitBoxAPI.GetHitBox]
---@return ShanghaiWindy.Core.HitBoxComponent HitBoxComponent，不存在则返回 null
---@param gameObject UnityEngine.GameObject
function HitBoxAPI.GetHitBox(gameObject) end
---从 Collider 获取 HitBoxComponent。
---Get HitBoxComponent from a Collider.
---@static
---@function [HitBoxAPI.GetHitBoxFromCollider]
---@return ShanghaiWindy.Core.HitBoxComponent HitBoxComponent，不存在则返回 null
---@param collider UnityEngine.Collider
function HitBoxAPI.GetHitBoxFromCollider(collider) end
---获取 HitBox 的装甲厚度。
---Get the armor thickness of a HitBox.
---@static
---@function [HitBoxAPI.GetArmor]
---@return System.Int32 装甲厚度 Armor thickness
---@param hitBox ShanghaiWindy.Core.HitBoxComponent
function HitBoxAPI.GetArmor(hitBox) end
---获取 HitBox 所属载具。
---Get the vehicle that owns this HitBox.
---@static
---@function [HitBoxAPI.GetOwner]
---@return ShanghaiWindy.Core.BaseInitSystem 所属载具 Owner vehicle
---@param hitBox ShanghaiWindy.Core.HitBoxComponent
function HitBoxAPI.GetOwner(hitBox) end
---获取载具上所有 HitBoxComponent。
---Get all HitBoxComponents on a vehicle.
---@static
---@function [HitBoxAPI.GetAllHitBoxes]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.HitBoxComponent, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] HitBoxComponent 列表 List of HitBoxComponent
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function HitBoxAPI.GetAllHitBoxes(vehicle) end
return HitBoxAPI

---默认值输入
---Default value input
---@class ShanghaiWindy.Core.API.eDefaultValueInputType
local eDefaultValueInputType = {}

return eDefaultValueInputType

---默认按键输入
---Default button input
---@class ShanghaiWindy.Core.API.eDefaultButtonInputType
local eDefaultButtonInputType = {}

return eDefaultButtonInputType

---输入处理 API
---Input Handler API
---@class ShanghaiWindy.Core.API.InputAPI
local InputAPI = {}

---注册按键输入
---Register Key Input
---@static
---@function [InputAPI.RegisterKeyInput]
---@param actionName System.String
---@param keyCode System.String
---@param keyPerformed ShanghaiWindy.Core.Delegate.OnKeyPerformed
---@param keyCanceled ShanghaiWindy.Core.Delegate.OnKeyCanceled
function InputAPI.RegisterKeyInput(actionName, keyCode, keyPerformed, keyCanceled) end
---取消注册按键输入
---Unregister Key Input
---@static
---@function [InputAPI.UnregisterKeyInput]
---@param actionName System.String
function InputAPI.UnregisterKeyInput(actionName) end
---注册按键添加事件回调。
---Register a callback when a key input is added.
---@static
---@function [InputAPI.RegisterAddKeyInput]
---@param onKeyRegistered ShanghaiWindy.Core.Delegate.OnKeyRegistered
function InputAPI.RegisterAddKeyInput(onKeyRegistered) end
---取消注册按键添加事件回调。
---Unregister a callback when a key input is added.
---@static
---@function [InputAPI.UnregisterAddKeyInput]
---@param onKeyRegistered ShanghaiWindy.Core.Delegate.OnKeyRegistered
function InputAPI.UnregisterAddKeyInput(onKeyRegistered) end
---注册按键移除事件回调。
---Register a callback when a key input is removed.
---@static
---@function [InputAPI.RegisterRemoveKeyInput]
---@param onKeyUnregistered ShanghaiWindy.Core.Delegate.OnKeyUnregistered
function InputAPI.RegisterRemoveKeyInput(onKeyUnregistered) end
---取消注册按键移除事件回调。
---Unregister a callback when a key input is removed.
---@static
---@function [InputAPI.UnregisterRemoveKeyInput]
---@param onKeyUnregistered ShanghaiWindy.Core.Delegate.OnKeyUnregistered
function InputAPI.UnregisterRemoveKeyInput(onKeyUnregistered) end
---获取所有已注册的按键绑定。
---Get all registered key bindings.
---@static
---@function [InputAPI.GetBindings]
---@return System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[ShanghaiWindy.Core.API.InputAPICache, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] 按键绑定字典 Key bindings dictionary
function InputAPI.GetBindings() end
---获取默认值类型输入的当前值。
---Get the current value of a default value input.
---@static
---@function [InputAPI.GetDefaultValueInput]
---@return System.Single 输入值 Input value
---@param inputType ShanghaiWindy.Core.API.eDefaultValueInputType
function InputAPI.GetDefaultValueInput(inputType) end
---检查默认按键是否被按下。
---Check if a default button input is pressed.
---@static
---@function [InputAPI.IsDefaultPressed]
---@return System.Boolean 是否按下 Is pressed
---@param inputType ShanghaiWindy.Core.API.eDefaultButtonInputType
function InputAPI.IsDefaultPressed(inputType) end
return InputAPI

---内部模块 API
---Internal Module API
---@class ShanghaiWindy.Core.API.InternalModuleAPI
local InternalModuleAPI = {}

---从模块所在的 GameObject 获取内部模块组件（不是 InternalModule 脚本本身），挂在模块物体上的脚本传 self.gameObject 即可。
---模块组件在载具装配时创建：在 OnVehicleReady 里或之后调用；装配前、物体上没有模块时返回 nil。
---Get the InternalModuleComponent (not the InternalModule script itself) from the module's GameObject; a script on
---the module object passes self.gameObject. The component is created when the vehicle is assembled: call this in
---OnVehicleReady or later. nil before that, or when the object has no module.
---@static
---@function [InternalModuleAPI.GetInternalModule]
---@return ShanghaiWindy.Core.InternalModuleComponent InternalModuleComponent，不存在则返回 nil InternalModuleComponent, nil when there is none
---@param gameObject UnityEngine.GameObject
function InternalModuleAPI.GetInternalModule(gameObject) end
---从 Collider 获取内部模块组件。
---Get InternalModuleComponent from a Collider.
---@static
---@function [InternalModuleAPI.GetInternalModuleFromCollider]
---@return ShanghaiWindy.Core.InternalModuleComponent InternalModuleComponent，不存在则返回 nil InternalModuleComponent, nil when there is none
---@param collider UnityEngine.Collider
function InternalModuleAPI.GetInternalModuleFromCollider(collider) end
---获取模块类型。
---Get the module type.
---@static
---@function [InternalModuleAPI.GetModuleType]
---@return ShanghaiWindy.Core.EInternalModuleType 模块类型 Module type
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.GetModuleType(module) end
---获取模块状态。
---Get module state (Good/Slight/Middle/Critical/Serious/Destroyed).
---@static
---@function [InternalModuleAPI.GetModuleState]
---@return ShanghaiWindy.Core.InternalModuleState 模块状态 Module state
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.GetModuleState(module) end
---获取模块当前血量。
---Get the current hit point.
---@static
---@function [InternalModuleAPI.GetCurrentHitPoint]
---@return System.Int32 当前血量 Current HP
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.GetCurrentHitPoint(module) end
---获取模块最大血量。
---Get the max hit point.
---@static
---@function [InternalModuleAPI.GetMaxHitPoint]
---@return System.Int32 最大血量 Max HP
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.GetMaxHitPoint(module) end
---获取当前血量占最大血量的比例，0 到 1。
---Get the current hit points as a share of the maximum, 0 to 1.
---@static
---@function [InternalModuleAPI.GetHitPointRatio]
---@return System.Single 血量比例 Hit point ratio
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.GetHitPointRatio(module) end
---模块是否已被摧毁。
---Whether the module is destroyed.
---@static
---@function [InternalModuleAPI.IsDestroyed]
---@return System.Boolean 是否已摧毁 Whether it is destroyed
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.IsDestroyed(module) end
---获取模块所属载具。
---Get the vehicle that owns this module.
---@static
---@function [InternalModuleAPI.GetOwner]
---@return ShanghaiWindy.Core.BaseInitSystem 所属载具 Owner vehicle
---@param module ShanghaiWindy.Core.InternalModuleComponent
function InternalModuleAPI.GetOwner(module) end
---获取载具上所有内部模块组件。每次调用都会新建列表，只在初始化时调用。
---Get all InternalModuleComponents on a vehicle. Each call builds a new list; call it once at set-up.
---@static
---@function [InternalModuleAPI.GetAllInternalModules]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.InternalModuleComponent, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] 模块列表 List of InternalModuleComponent
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function InternalModuleAPI.GetAllInternalModules(vehicle) end
---订阅模块状态变化（Good / Slight / Middle / Critical / Serious / Destroyed 之间切换），回调参数为旧状态和新状态。
---Subscribe to the module changing state (Good / Slight / Middle / Critical / Serious / Destroyed); the callback gets
---the old and the new state.
---@static
---@function [InternalModuleAPI.OnStateChanged]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param module ShanghaiWindy.Core.InternalModuleComponent
---@param callback ShanghaiWindy.Core.Delegate.OnLuaModuleStateEventDelegate
function InternalModuleAPI.OnStateChanged(owner, module, callback) end
---订阅模块受到伤害，回调参数为这次扣除的血量（回放中为 0）。模块已毁、伤害转给替补模块时由替补模块发出。
---Subscribe to the module taking damage; the callback gets the hit points this hit took (0 in a replay). Once the
---module is destroyed and a substitute takes the damage, the substitute raises it.
---@static
---@function [InternalModuleAPI.OnDamaged]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param module ShanghaiWindy.Core.InternalModuleComponent
---@param callback ShanghaiWindy.Core.Delegate.OnLuaFloatEventDelegate
function InternalModuleAPI.OnDamaged(owner, module, callback) end
---订阅模块被修复（自动修复、手动修复或维修点）。
---Subscribe to the module being repaired (auto repair, manual repair or a repair point).
---@static
---@function [InternalModuleAPI.OnRepaired]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param module ShanghaiWindy.Core.InternalModuleComponent
---@param callback ShanghaiWindy.Core.Delegate.OnLuaEventDelegate
function InternalModuleAPI.OnRepaired(owner, module, callback) end
return InternalModuleAPI

---载具实例所处的场合。
---Where a vehicle instance lives.
---@class ShanghaiWindy.Core.API.VehicleContext
local VehicleContext = {}

return VehicleContext

---载具大类，用于查询过滤。
---Vehicle kind, used to filter queries.
---@class ShanghaiWindy.Core.API.VehicleKind
local VehicleKind = {}

return VehicleKind

---相对于某辆载具的敌友关系，用于查询过滤。
---Relation to a vehicle, used to filter queries.
---@class ShanghaiWindy.Core.API.VehicleRelation
local VehicleRelation = {}

return VehicleRelation

---一次事件订阅。订阅时传入 owner（self.script）的，行为销毁时自动退订；目标载具被移除时也自动退订。也可以随时调用 Dispose 提前退订。
---One event subscription. With an owner (self.script) it ends when the behaviour is destroyed; it also ends when the target vehicle
---is removed. Call Dispose to end it earlier.
---@class ShanghaiWindy.Core.API.LuaSubscription
---@field IsActive System.Boolean
local LuaSubscription = {}

---退订。重复调用无效果。
---End the subscription. Calling it again does nothing.
---@instance
---@function [LuaSubscription:Dispose]
function LuaSubscription:Dispose() end
return LuaSubscription

---数值可变的 buff，由 BuffAPI.CreateValueBuff 创建。挂到属性上之后调用 SetValue，属性会原地刷新。
---A buff whose value can change, made by BuffAPI.CreateValueBuff. Once it is on a stat, SetValue refreshes that stat in place.
---@class ShanghaiWindy.Core.API.LuaValueBuff
local LuaValueBuff = {}

---当前系数。
---The current coefficient.
---@instance
---@function [LuaValueBuff:GetValue]
---@return System.Single 系数 Coefficient
function LuaValueBuff:GetValue() end
---修改系数；挂着它的属性会立即刷新。数值没变时什么也不做，所以可以每帧调用。
---Change the coefficient; every stat it is attached to refreshes at once. Does nothing when the value is unchanged, so it is fine
---to call every frame.
---@instance
---@function [LuaValueBuff:SetValue]
---@param value System.Single
function LuaValueBuff:SetValue(value) end
return LuaValueBuff

---地图 API
---Map API
---@class ShanghaiWindy.Core.API.MapAPI
local MapAPI = {}

---通过 GUID 获取地图数据。
---Get map data by GUID.
---@static
---@function [MapAPI.GetMapDataByGuid]
---@return ShanghaiWindy.Core.MapData 地图数据 Map data
---@param guid System.String
function MapAPI.GetMapDataByGuid(guid) end
return MapAPI

---材质 API
---Material API
---@class ShanghaiWindy.Core.API.MaterialAPI
local MaterialAPI = {}

---异步为目标对象应用材质。
---Asynchronously apply a material to the target GameObject.
---@static
---@function [MaterialAPI.AsyncApplyMaterial]
---@param guid System.String
---@param instance UnityEngine.GameObject
function MaterialAPI.AsyncApplyMaterial(guid, instance) end
return MaterialAPI

---网格与碰撞体 API
---Mesh and Collider API
---@class ShanghaiWindy.Core.API.MeshAPI
local MeshAPI = {}

---创建 mesh
---Create mesh
---@static
---@function [MeshAPI.CreateMesh]
---@return System.Int32 Mesh ID
---@param points UnityEngine.Vector3[]
---@param height System.Single
function MeshAPI.CreateMesh(points, height) end
---获取 mesh
---Get mesh
---@static
---@function [MeshAPI.GetMesh]
---@return UnityEngine.GameObject Mesh 对应的 GameObject
---@param meshId System.Int32
function MeshAPI.GetMesh(meshId) end
---删除 mesh
---Delete mesh
---@static
---@function [MeshAPI.DeleteMesh]
---@param meshId System.Int32
function MeshAPI.DeleteMesh(meshId) end
---创建球形触发区域。
---Create a sphere trigger box.
---@static
---@function [MeshAPI.CreateSphereTriggerBox]
---@return System.Int32 触发器 ID Trigger ID
---@param position UnityEngine.Vector3
---@param radius System.Single
---@param onEnter System.Action`1[[UnityEngine.Collider, UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@param onExit System.Action`1[[UnityEngine.Collider, UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function MeshAPI.CreateSphereTriggerBox(position, radius, onEnter, onExit) end
---判断点是否在多边形内（XZ 平面）。
---Check if a point is inside a polygon (XZ plane).
---@static
---@function [MeshAPI.IsPointInPolygon]
---@return System.Boolean 是否在多边形内 Is inside polygon
---@param point UnityEngine.Vector3
---@param polygonPoints UnityEngine.Vector3[]
function MeshAPI.IsPointInPolygon(point, polygonPoints) end
return MeshAPI

---模式 API
---Mode API
---@class ShanghaiWindy.Core.API.ModeAPI
local ModeAPI = {}

---退出模式
---Exit Mode
---@static
---@function [ModeAPI.ExitMode]
function ModeAPI.ExitMode() end
---显示选择载具 UI
---Show pick vehicle ui
---@static
---@function [ModeAPI.ShowPickVehicleUI]
---@param isForcePick System.Boolean
function ModeAPI.ShowPickVehicleUI(isForcePick) end
---显示选择载具 UI (带列表)
---Show pick vehicle ui (with list)
---@static
---@function [ModeAPI.ShowPickVehicleUIWithList]
---@param isForcePick System.Boolean
---@param vehicleList ShanghaiWindy.Core.DriveableList
function ModeAPI.ShowPickVehicleUIWithList(isForcePick, vehicleList) end
---@static
---@function [ModeAPI.ShowPickVehicleListUI]
---@return System.Void
---@param callback System.Action`1[[System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
function ModeAPI.ShowPickVehicleListUI(callback) end
---注册选中载具回调
---Register vehicle pick callabck
---@static
---@function [ModeAPI.RegisterPickVehicleCallBack]
---@param pickDelegate ShanghaiWindy.Core.EventDispatcherDelegate`1[[ShanghaiWindy.Core.VehiclePickEvent, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ModeAPI.RegisterPickVehicleCallBack(pickDelegate) end
---取消注册选中载具回调
---UnRegister vehicle pick callabck
---@static
---@function [ModeAPI.UnRegisterPickVehicleCallBack]
---@param pickDelegate ShanghaiWindy.Core.EventDispatcherDelegate`1[[ShanghaiWindy.Core.VehiclePickEvent, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ModeAPI.UnRegisterPickVehicleCallBack(pickDelegate) end
---加载战斗场景
---Load battle scene
---@static
---@function [ModeAPI.LoadBattleScene]
---@param mapData ShanghaiWindy.Core.MapData
---@param callback ShanghaiWindy.Core.Delegate.OnBattleSceneLoadedDelegate
function ModeAPI.LoadBattleScene(mapData, callback) end
---加载战斗 UI
---Load battle ui
---@static
---@function [ModeAPI.LoadBattleUI]
---@param callback ShanghaiWindy.Core.Delegate.OnBattleUILoadedDelegate
function ModeAPI.LoadBattleUI(callback) end
---添加玩家
---Add battle player
---@static
---@function [ModeAPI.AddBattlePlayer]
---@param battlePlayer ShanghaiWindy.Core.AbstractBattlePlayer
function ModeAPI.AddBattlePlayer(battlePlayer) end
---删除玩家
---Remove battle player
---@static
---@function [ModeAPI.RemoveBattlePlayer]
---@param uid System.Int32
function ModeAPI.RemoveBattlePlayer(uid) end
---更新比分
---Update score
---@static
---@function [ModeAPI.UpdateScore]
---@param friendTeamScore System.Int32
---@param enemyTeamScore System.Int32
---@param totalFriendTeamScore System.Int32
---@param totalEnemyTeamScore System.Int32
function ModeAPI.UpdateScore(friendTeamScore, enemyTeamScore, totalFriendTeamScore, totalEnemyTeamScore) end
---注册占领点事件
---Register capture point
---@static
---@function [ModeAPI.RegisterCapturePointCallback]
---@param captureDelegate ShanghaiWindy.Core.EventDispatcherDelegate`1[[ShanghaiWindy.Core.CaptureEvent, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ModeAPI.RegisterCapturePointCallback(captureDelegate) end
---取消注册占领点事件
---Unregister capture point
---@static
---@function [ModeAPI.UnRegisterCapturePointCallback]
---@param captureDelegate ShanghaiWindy.Core.EventDispatcherDelegate`1[[ShanghaiWindy.Core.CaptureEvent, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ModeAPI.UnRegisterCapturePointCallback(captureDelegate) end
---增加菜单自定义选项
---Add custom menu option
---@static
---@function [ModeAPI.AddCustomMenuOption]
---@param optionName System.String
---@param callback System.Action
function ModeAPI.AddCustomMenuOption(optionName, callback) end
---删除菜单自定义选项
---Remove custom menu option
---@static
---@function [ModeAPI.RemoveCustomMenuOption]
---@param optionName System.String
function ModeAPI.RemoveCustomMenuOption(optionName) end
---显示成功失败界面
---Show victory or default
---@static
---@function [ModeAPI.ShowVictoryOrDefeat]
---@param isVictory System.Boolean
function ModeAPI.ShowVictoryOrDefeat(isVictory) end
---显示与隐藏比分
---Show or hide score
---@static
---@function [ModeAPI.ToggleScore]
---@param isShownScore System.Boolean
function ModeAPI.ToggleScore(isShownScore) end
---开启作弊
---Enable cheat
---@static
---@function [ModeAPI.EnableCheat]
function ModeAPI.EnableCheat() end
---关闭作弊
---Disable cheat
---@static
---@function [ModeAPI.DisableCheat]
function ModeAPI.DisableCheat() end
---切换是否锁住载具
---Toggle lock vehicle state
---@static
---@function [ModeAPI.ToggleVehicleLockState]
---@param isLock System.Boolean
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function ModeAPI.ToggleVehicleLockState(isLock, vehicle) end
---显示倒计时
---Enable count down
---@static
---@function [ModeAPI.EnableCountDown]
---@param countDown System.Int32
---@param countDownTitle System.String
---@param countDownDescription System.String
---@param callback System.Action
function ModeAPI.EnableCountDown(countDown, countDownTitle, countDownDescription, callback) end
---获取 c# 游戏模式实例
---Get the native game mode in c#
---@static
---@function [ModeAPI.GetNativeMode]
---@return ShanghaiWindy.Core.GameMode.BaseGameMode 游戏模式实例 Game mode instance
function ModeAPI.GetNativeMode() end
---开始录制
---Start Record
---@static
---@function [ModeAPI.StartRecord]
function ModeAPI.StartRecord() end
---停止录制
---Stop Record
---@static
---@function [ModeAPI.EndRecord]
function ModeAPI.EndRecord() end
---发送战斗信息
---Send battle information
---@static
---@function [ModeAPI.SendInformation]
---@param title System.String
---@param information System.String
function ModeAPI.SendInformation(title, information) end
---打开载具选择器（单选，点击即关闭）
---Open vehicle picker (single select, closes on pick)
---@static
---@function [ModeAPI.ShowVehiclePicker]
---@param titleKey System.String
---@param onPicked System.Action`1[[ShanghaiWindy.Core.VehicleInfo, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ModeAPI.ShowVehiclePicker(titleKey, onPicked) end
---打开载具队列选择器（多选，确认后关闭）
---Open vehicle queue picker (multi select, closes on confirm)
---@static
---@function [ModeAPI.ShowVehicleQueuePicker]
---@param titleKey System.String
---@param queue ShanghaiWindy.Core.ClientUserVehicleQueue
---@param maxVehicles System.Int32
---@param onDone System.Action`1[[ShanghaiWindy.Core.ClientUserVehicleQueue, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function ModeAPI.ShowVehicleQueuePicker(titleKey, queue, maxVehicles, onDone) end
return ModeAPI

---位置点 API
---Point API
---@class ShanghaiWindy.Core.API.PointAPI
local PointAPI = {}

---获取 A 队起始点
---Get Team A Start Points
---@static
---@function [PointAPI.GetTeamAStartPoints]
---@return UnityEngine.GameObject[] 包含 A 队起始点的 GameObject 数组
function PointAPI.GetTeamAStartPoints() end
---获取 B 队起始点
---Get Team B Start Points
---@static
---@function [PointAPI.GetTeamBStartPoints]
---@return UnityEngine.GameObject[] 包含 B 队起始点的 GameObject 数组
function PointAPI.GetTeamBStartPoints() end
---获取巡逻点
---Get Patrol Points
---@static
---@function [PointAPI.GetPatrolPoints]
---@return UnityEngine.GameObject[] 包含巡逻点的 GameObject 数组
function PointAPI.GetPatrolPoints() end
---设置是否开启地图边界
---Toggle if map boundary open
---@static
---@function [PointAPI.ToggleMapBoundary]
---@param state System.Boolean
function PointAPI.ToggleMapBoundary(state) end
return PointAPI

---雷达 API：读取载具雷达（地面防空雷达或机载雷达）的状态、目标与告警，并控制本地玩家自己的雷达。雷达参数在载具数据的 groundAARadarData（飞机为 radarData）里配置。联机远程副本的雷达不在本机扫描：没有目标，扫描方位不动，锁定目标与工作模式来自网络，锁定 / 模式事件不触发。
---Radar API: read a vehicle's radar (ground air-defence or airborne): status, contacts and warnings, and control the local player's own radar. Radar parameters are set in the vehicle data's groundAARadarData (radarData for aircraft). A remote copy's radar does not scan on this client: no contacts, a still scan azimuth, lock target and work mode from the network, and no lock / mode events.
---@class ShanghaiWindy.Core.API.RadarAPI
local RadarAPI = {}

---@static
---@function [RadarAPI.TryGetRadar]
---@return System.Boolean
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param radar ShanghaiWindy.Core.BaseRadarSystem&
function RadarAPI.TryGetRadar(vehicle, radar) end
---雷达是否开机。
---Whether the radar is on.
---@static
---@function [RadarAPI.IsActive]
---@return System.Boolean 是否开机 Is on
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.IsActive(radar) end
---获取雷达工作模式。
---Get the radar's work mode.
---@static
---@function [RadarAPI.GetWorkMode]
---@return ShanghaiWindy.Core.ERadarWorkMode 工作模式；无效时为 Off Work mode; Off when invalid
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetWorkMode(radar) end
---获取天线当前方位角（度，-180~180，0 为正前方，向右为正）。正前方指雷达安装处的朝向：地面防空雷达是炮塔，机载雷达是机头（水平稳定的雷达取机头在水平面上的朝向）。扇扫雷达在 ±扇区半角之间往返，全向旋转雷达一圈圈转，跟踪（TRK）时指向锁定目标。把它作为天线模型相对安装处的偏航角即可让模型跟着转。
---Get the antenna's current azimuth in degrees, -180 to 180, 0 straight ahead, positive to the right. Ahead is where the radar is mounted: the turret for ground air defence, the nose for aircraft (a horizon-stabilized radar uses the nose heading in the horizontal plane). A sector radar sweeps back and forth within its half-angle, a full-rotation radar spins, and in TRK it points at the locked target. Use it as your antenna model's yaw relative to its mount to turn the model with it.
---@static
---@function [RadarAPI.GetScanAzimuth]
---@return System.Single 方位角（度）；无效时为 0 Azimuth in degrees; 0 when invalid
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetScanAzimuth(radar) end
---获取雷达安装处朝向的世界航向（度，-180~180，从世界 +Z 顺时针）。航向 + 方位角 = 世界方位，用来画正北朝上的 PPI。
---Get the world heading of the radar's mount in degrees, -180 to 180, clockwise from world +Z. Heading + azimuth = world bearing, for a north-up PPI.
---@static
---@function [RadarAPI.GetWorldHeading]
---@return System.Single 航向（度）；无效时为 0 Heading in degrees; 0 when invalid
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetWorldHeading(radar) end
---获取当前信号模式下的最大探测距离（米），以 5 平方米雷达截面的目标为准；截面更大的目标看得更远（按截面的四次方根），IRST 用自己的距离。
---Get the maximum detection range in metres in the current signal mode, for a target of 5 m² radar cross-section; larger targets are seen farther (by the fourth root of the cross-section), IRST uses its own range.
---@static
---@function [RadarAPI.GetMaxRange]
---@return System.Single 距离（米）；无效时为 0 Range in metres; 0 when invalid
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetMaxRange(radar) end
---获取雷达所属载具。
---Get the vehicle the radar belongs to.
---@static
---@function [RadarAPI.GetOwner]
---@return ShanghaiWindy.Core.BaseInitSystem 载具；已移除时为 nil Vehicle; nil once removed
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetOwner(radar) end
---获取雷达画面上的目标数量（含箔条假回波）。远程副本为 0；联机时敌方的雷达为 0。
---Get the number of contacts on the radar picture (chaff false echoes included). 0 for a remote copy, and online 0 for an enemy's radar.
---@static
---@function [RadarAPI.GetContactCount]
---@return System.Int32 目标数量 Number of contacts
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetContactCount(radar) end
---@static
---@function [RadarAPI.GetContact]
---@return System.Boolean
---@param radar ShanghaiWindy.Core.BaseRadarSystem
---@param index System.Int32
---@param target ShanghaiWindy.Core.BaseInitSystem&
---@param distance System.Single&
---@param azimuth System.Single&
---@param elevation System.Single&
---@param altitude System.Single&
---@param radialSpeed System.Single&
---@param isFriendly System.Boolean&
function RadarAPI.GetContact(radar, index, target, distance, azimuth, elevation, altitude, radialSpeed, isFriendly) end
---获取第 index 个目标的世界方位（度，-180~180，从世界 +Z 顺时针），为波束扫过时的快照，画正北朝上的 PPI 用。
---Get the world bearing of contact number index in degrees, -180 to 180, clockwise from world +Z, as of the beam's sweep; for a north-up PPI.
---@static
---@function [RadarAPI.GetContactWorldAzimuth]
---@return System.Single 世界方位（度）；越界时为 0 World bearing in degrees; 0 when out of range
---@param radar ShanghaiWindy.Core.BaseRadarSystem
---@param index System.Int32
function RadarAPI.GetContactWorldAzimuth(radar, index) end
---获取当前锁定（TRK）的目标。远程副本为网络同步的锁定目标。
---Get the target locked in TRK. For a remote copy, the lock target synced over the network.
---@static
---@function [RadarAPI.GetLockedTarget]
---@return ShanghaiWindy.Core.BaseInitSystem 锁定的载具；未锁定时为 nil Locked vehicle; nil when nothing is locked
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.GetLockedTarget(radar) end
---锁定是否处于丢失信号的重捕宽限中（游戏界面上锁定框会闪烁）。
---Whether the lock is in its reacquire grace after losing the signal (the game's lock box blinks).
---@static
---@function [RadarAPI.IsLockDegraded]
---@return System.Boolean 是否信号不稳 Is degraded
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.IsLockDegraded(radar) end
---锁定雷达画面上的一个目标，与玩家在雷达列表里选中锁定相同：要先经过锁定捕获时间，锁上后触发 OnLockChanged；跟踪中会直接换锁。只对本地玩家正在控制的载具的雷达有效。
---Lock a target on the radar picture, the same as the player locking it from the radar list: it goes through the lock acquisition time and OnLockChanged fires once locked; while tracking, the lock switches at once. Only works on the radar of the vehicle the local player controls.
---@static
---@function [RadarAPI.TryLock]
---@return System.Boolean 已锁定或正在锁定时为 true；不允许、雷达关机或目标不在画面上时为 false True when locked or locking; false when not allowed, the radar is off, or the target is not on the picture
---@param radar ShanghaiWindy.Core.BaseRadarSystem
---@param target ShanghaiWindy.Core.BaseInitSystem
function RadarAPI.TryLock(radar, target) end
---解除锁定或取消正在进行的锁定，雷达回到搜索模式。只对本地玩家正在控制的载具的雷达有效。
---Release the lock, or cancel a lock in progress; the radar returns to search. Only works on the radar of the vehicle the local player controls.
---@static
---@function [RadarAPI.Unlock]
---@return System.Boolean 是否有锁定被解除 Whether a lock was released
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.Unlock(radar) end
---切换目标，与玩家的选择键相同：搜索时依次指定下一个目标（最后一次清除指定），跟踪时直接换锁下一个。只对本地玩家正在控制的载具的雷达有效。
---Cycle targets, like the player's select key: in search it designates the next target (the last press clears it), in TRK it locks the next one at once. Only works on the radar of the vehicle the local player controls.
---@static
---@function [RadarAPI.CycleTarget]
---@return System.Boolean 是否执行 Whether it ran
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.CycleTarget(radar) end
---切换工作模式，与玩家的模式键相同：关机时开机，搜索（SRC）与边扫边跟（TWS）之间切换（雷达支持 TWS 时），跟踪或格斗时回到搜索。只对本地玩家正在控制的载具的雷达有效。
---Switch the work mode, like the player's mode key: turns the radar on when off, toggles search (SRC) and track-while-scan (TWS) when the radar has TWS, and returns to search from TRK or ACM. Only works on the radar of the vehicle the local player controls.
---@static
---@function [RadarAPI.SwitchMode]
---@return System.Boolean 是否执行 Whether it ran
---@param radar ShanghaiWindy.Core.BaseRadarSystem
function RadarAPI.SwitchMode(radar) end
---订阅锁定变化（锁上、换锁、解锁或丢失）。远程副本不触发。
---Subscribe to lock changes (locked, switched, released or lost). Does not fire for a remote copy.
---@static
---@function [RadarAPI.OnLockChanged]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅；参数无效时为 nil Subscription; nil for invalid arguments
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param radar ShanghaiWindy.Core.BaseRadarSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaRadarLockEventDelegate
function RadarAPI.OnLockChanged(owner, radar, callback) end
---订阅工作模式变化（只在模式真的变了时触发，切换信号模式不触发）。远程副本不触发。
---Subscribe to work mode changes (fires only when the mode really changes; a signal mode switch does not count). Does not fire for a remote copy.
---@static
---@function [RadarAPI.OnModeChanged]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅；参数无效时为 nil Subscription; nil for invalid arguments
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param radar ShanghaiWindy.Core.BaseRadarSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaRadarModeEventDelegate
function RadarAPI.OnModeChanged(owner, radar, callback) end
---订阅雷达告警（RWR）：出现新威胁时触发（同一来源、同一类型的威胁还在告警列表里时，约 5 秒内，再次出现只刷新、不再触发）。联机时本队看不到的来源传 nil。
---Subscribe to the radar warning receiver: fires when a new threat appears (the same source and type again while it is still listed, about 5 s, only refreshes it). Online, a source your team cannot see is passed as nil.
---@static
---@function [RadarAPI.OnThreat]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅；参数无效时为 nil Subscription; nil for invalid arguments
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param radar ShanghaiWindy.Core.BaseRadarSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaRadarThreatEventDelegate
function RadarAPI.OnThreat(owner, radar, callback) end
return RadarAPI

---随机 API
---Random API
---@class ShanghaiWindy.Core.API.RandomAPI
local RandomAPI = {}

---从载具列表随机一个载具
---Random vehicle from list
---@static
---@function [RandomAPI.GetRandomVehicleFromList]
---@return ShanghaiWindy.Core.VehicleInfo 随机选中的载具 Randomly selected vehicle
---@param vehicleInfos System.Collections.Generic.List`1[[ShanghaiWindy.Core.VehicleInfo, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function RandomAPI.GetRandomVehicleFromList(vehicleInfos) end
return RandomAPI

---声音相关 API
---Sound-related API
---@class ShanghaiWindy.Core.API.SoundAPI
local SoundAPI = {}

---播放一次性音效
---Play One-Shot Sound
---@static
---@function [SoundAPI.PlayOneShot]
---@param guid System.String
---@param position UnityEngine.Vector3
function SoundAPI.PlayOneShot(guid, position) end
---发布音频事件
---Post Sound Event
---@static
---@function [SoundAPI.PostEvent]
---@param guid System.String
---@param position UnityEngine.Vector3
---@param postEventLoadedDelegate ShanghaiWindy.Core.Delegate.PostEventLoadedDelegate
function SoundAPI.PostEvent(guid, position, postEventLoadedDelegate) end
---释放音频事件
---Release Sound Event
---@static
---@function [SoundAPI.ReleaseEvent]
---@param eventInstance FMOD.Studio.EventInstance
function SoundAPI.ReleaseEvent(eventInstance) end
return SoundAPI

---重生点 API
---Spawn API
---@class ShanghaiWindy.Core.API.SpawnAPI
local SpawnAPI = {}

---@static
---@function [SpawnAPI.AsyncSpawn]
---@return System.Void
---@param spawnPointType ShanghaiWindy.Core.ESpawnPointType
---@param team ShanghaiWindy.Core.TeamManager+Team
---@param onSpawnPoint System.Action`1[[UnityEngine.Transform, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function SpawnAPI.AsyncSpawn(spawnPointType, team, onSpawnPoint) end
---异步寻找当前的出生点可用点
---Find spawn given point in async
---@static
---@function [SpawnAPI.AsyncSpawnGivenPoints]
---@param spawnPointType ShanghaiWindy.Core.ESpawnPointType
---@param points UnityEngine.Transform[]
---@param onSpawnPoint System.Action`1[[UnityEngine.Transform, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function SpawnAPI.AsyncSpawnGivenPoints(spawnPointType, points, onSpawnPoint) end
return SpawnAPI

---载具生成API
---Vehicle Spawn APIs
---@class ShanghaiWindy.Core.API.SpawnVehicleAPI
local SpawnVehicleAPI = {}

---@static
---@function [SpawnVehicleAPI.CreateLocalBot]
---@return System.Void
---@param vehicleInfo ShanghaiWindy.Core.VehicleInfo
---@param pos UnityEngine.Vector3
---@param rot UnityEngine.Quaternion
---@param botTeam ShanghaiWindy.Core.TeamManager+Team
---@param isKeepWreckage System.Boolean
---@param isIdle System.Boolean
function SpawnVehicleAPI.CreateLocalBot(vehicleInfo, pos, rot, botTeam, isKeepWreckage, isIdle) end
---创建一个本地玩家。您可以从GameDataManager.PlayerTeam设置玩家队伍
---Create a Local Player. You can set player team from GameDataManager.PlayerTeam
---@static
---@function [SpawnVehicleAPI.CreatePlayer]
---@return ShanghaiWindy.Core.BaseInitSystem 在生成位置创建的本地玩家 A created local player at spawn tranform
---@param vehicleInfo ShanghaiWindy.Core.VehicleInfo
---@param pos UnityEngine.Vector3
---@param rot UnityEngine.Quaternion
---@param onPreBind System.Action`1[[ShanghaiWindy.Core.BaseInitSystem, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function SpawnVehicleAPI.CreatePlayer(vehicleInfo, pos, rot, onPreBind) end
---创建一个自由摄像机
---Create a free camera
---@static
---@function [SpawnVehicleAPI.CreateFreeCamera]
---@return ShanghaiWindy.Core.FreeCameraInitSystem 在生成位置创建的自由摄像机 A created free camera at spawn tranform
---@param pos UnityEngine.Vector3
---@param rot UnityEngine.Quaternion
function SpawnVehicleAPI.CreateFreeCamera(pos, rot) end
return SpawnVehicleAPI

---存储 API
---Storage API
---@class ShanghaiWindy.Core.API.StorageAPI
local StorageAPI = {}

---获得 Number
---Get Number
---@static
---@function [StorageAPI.GetNumberValue]
---@return System.Single 存储的数值 Stored number value
---@param group System.String
---@param key System.String
---@param defaultValue System.Single
function StorageAPI.GetNumberValue(group, key, defaultValue) end
---设置 Number
---Set Number
---@static
---@function [StorageAPI.SetNumberValue]
---@param group System.String
---@param key System.String
---@param value System.Single
function StorageAPI.SetNumberValue(group, key, value) end
---获得 String
---Get String
---@static
---@function [StorageAPI.GetStringValue]
---@return System.String
---@param group System.String
---@param key System.String
---@param defaultValue System.String
function StorageAPI.GetStringValue(group, key, defaultValue) end
---设置 String
---Set String
---@static
---@function [StorageAPI.SetStringValue]
---@param group System.String
---@param key System.String
---@param value System.String
function StorageAPI.SetStringValue(group, key, value) end
---获得 string array
---Get string array
---@static
---@function [StorageAPI.GetStringArrayValue]
---@return System.String[]
---@param group System.String
---@param key System.String
---@param defaultValue System.String[]
function StorageAPI.GetStringArrayValue(group, key, defaultValue) end
---设置 string array
---Set string array
---@static
---@function [StorageAPI.SetStringArrayValue]
---@param group System.String
---@param key System.String
---@param value System.String[]
function StorageAPI.SetStringArrayValue(group, key, value) end
---获得布尔
---Get Boolean
---@static
---@function [StorageAPI.GetBooleanValue]
---@return System.Boolean
---@param group System.String
---@param key System.String
---@param defaultValue System.Boolean
function StorageAPI.GetBooleanValue(group, key, defaultValue) end
---设置布尔
---Set Boolean
---@static
---@function [StorageAPI.SetBooleanValue]
---@param group System.String
---@param key System.String
---@param value System.Boolean
function StorageAPI.SetBooleanValue(group, key, value) end
---保存
---Save Storage
---@static
---@function [StorageAPI.SaveStorage]
function StorageAPI.SaveStorage() end
---清空所有存储数据
---Clear all stored data
---@static
---@function [StorageAPI.Clear]
function StorageAPI.Clear() end
return StorageAPI

---坦克相关 API
---Tank-related API
---@class ShanghaiWindy.Core.API.TankAPI
local TankAPI = {}

---从坦克载具中获取所有坦克火力系统。
---Get all the tank fire systems from the tank vehicle.
---@static
---@function [TankAPI.GetTankFireList]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.TankFire, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] 坦克火力系统列表 List of tank fire systems
---@param vehicle ShanghaiWindy.Core.VehicleInitSystem
function TankAPI.GetTankFireList(vehicle) end
---获取炮塔数量（主炮塔和附加炮塔）。不是地面载具时为 0。
---Get how many turrets the vehicle has (main and extra turrets). 0 for a vehicle that is not a ground vehicle.
---@static
---@function [TankAPI.GetTurretCount]
---@return System.Int32 炮塔数量 Turret count
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function TankAPI.GetTurretCount(vehicle) end
---获取炮塔的水平旋转节点。序号从 0 开始，0 是主炮塔。
---Get a turret's traverse transform. Index from 0; 0 is the main turret.
---@static
---@function [TankAPI.GetTurretTransform]
---@return UnityEngine.Transform 炮塔节点，没有时为 nil Turret transform, nil when there is none
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param index System.Int32
function TankAPI.GetTurretTransform(vehicle, index) end
---获取炮塔的俯仰（炮管）节点。序号从 0 开始，0 是主炮塔。
---Get a turret's elevation (gun) transform. Index from 0; 0 is the main turret.
---@static
---@function [TankAPI.GetGunTransform]
---@return UnityEngine.Transform 炮管节点，没有时为 nil Gun transform, nil when there is none
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param index System.Int32
function TankAPI.GetGunTransform(vehicle, index) end
---@static
---@function [TankAPI.GetEngineState]
---@return System.Boolean
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param rpm System.Single&
---@param rpmPercent System.Single&
---@param gear System.Int32&
---@param speedKmh System.Single&
---@param reversing System.Boolean&
function TankAPI.GetEngineState(vehicle, rpm, rpmPercent, gear, speedKmh, reversing) end
return TankAPI

---团队接口
---Team API
---@class ShanghaiWindy.Core.API.TeamAPI
local TeamAPI = {}

---@static
---@function [TeamAPI.SetPlayerTeam]
---@return System.Void
---@param playerTeam ShanghaiWindy.Core.TeamManager+Team
function TeamAPI.SetPlayerTeam(playerTeam) end
---设置自己队伍为 Red Team
---Set player team as red team
---@static
---@function [TeamAPI.SetPlayerTeamAsRedTeam]
function TeamAPI.SetPlayerTeamAsRedTeam() end
---设置自己的队伍为 Blue Team
---Set player team as blue team
---@static
---@function [TeamAPI.SetPlayerTeamAsBlueTeam]
function TeamAPI.SetPlayerTeamAsBlueTeam() end
---获取玩家所在团队
---Get Player Team
---@static
---@function [TeamAPI.GetPlayerTeam]
---@return ShanghaiWindy.Core.TeamManager+Team 玩家所在团队 Player Team
function TeamAPI.GetPlayerTeam() end
---获取敌对团队
---Get Enemy Team
---@static
---@function [TeamAPI.GetEnemyTeam]
---@return ShanghaiWindy.Core.TeamManager+Team 敌对团队 Enemy Team
function TeamAPI.GetEnemyTeam() end
---获取红队
---Get red team
---@static
---@function [TeamAPI.GetRedTeam]
---@return ShanghaiWindy.Core.TeamManager+Team 红队 Red team
function TeamAPI.GetRedTeam() end
---获取蓝队
---Get blue team
---@static
---@function [TeamAPI.GetBlueTeam]
---@return ShanghaiWindy.Core.TeamManager+Team 蓝队 Blue team
function TeamAPI.GetBlueTeam() end
---获取无队伍
---Get none team
---@static
---@function [TeamAPI.GetNoneTeam]
---@return ShanghaiWindy.Core.TeamManager+Team 无队伍 None team
function TeamAPI.GetNoneTeam() end
return TeamAPI

---时间与帧回调 API
---Time and Tick API
---@class ShanghaiWindy.Core.API.TimeAPI
local TimeAPI = {}

---注册 Quarter Tick 回调（每 0.25 秒触发一次）。
---Register a quarter tick callback (triggered every 0.25 seconds).
---@static
---@function [TimeAPI.RegisterQuarterTick]
---@param callback ShanghaiWindy.Core.Delegate.OnQuarterTickDelegate
function TimeAPI.RegisterQuarterTick(callback) end
---取消注册 Quarter Tick 回调。
---Unregister a quarter tick callback.
---@static
---@function [TimeAPI.UnRegisterQuarterTick]
---@param callback ShanghaiWindy.Core.Delegate.OnQuarterTickDelegate
function TimeAPI.UnRegisterQuarterTick(callback) end
---注册 LateUpdate 帧回调。
---Register a LateUpdate frame tick callback.
---@static
---@function [TimeAPI.RegisterLateFrameTick]
---@param callback ShanghaiWindy.Core.Delegate.OnTickDelegate
function TimeAPI.RegisterLateFrameTick(callback) end
---取消注册 LateUpdate 帧回调。
---Unregister a LateUpdate frame tick callback.
---@static
---@function [TimeAPI.UnRegisterLateFrameTick]
---@param callback ShanghaiWindy.Core.Delegate.OnTickDelegate
function TimeAPI.UnRegisterLateFrameTick(callback) end
---注册 FixedUpdate 帧回调。
---Register a FixedUpdate frame tick callback.
---@static
---@function [TimeAPI.RegisterFixedFrameTick]
---@param callback ShanghaiWindy.Core.Delegate.OnTickDelegate
function TimeAPI.RegisterFixedFrameTick(callback) end
---取消注册 FixedUpdate 帧回调。
---Unregister a FixedUpdate frame tick callback.
---@static
---@function [TimeAPI.UnRegisterFixedFrameTick]
---@param callback ShanghaiWindy.Core.Delegate.OnTickDelegate
function TimeAPI.UnRegisterFixedFrameTick(callback) end
---获取上一帧的时间增量。
---Get the time elapsed since the last frame.
---@static
---@function [TimeAPI.GetDeltaTime]
---@return System.Single 帧间隔时间 Delta time in seconds
function TimeAPI.GetDeltaTime() end
---获取固定时间步长。
---Get the fixed time step interval.
---@static
---@function [TimeAPI.GetFixedDeltaTime]
---@return System.Single 固定帧间隔时间 Fixed delta time in seconds
function TimeAPI.GetFixedDeltaTime() end
---获取游戏已运行时间。
---Get the time since the game started.
---@static
---@function [TimeAPI.GetTime]
---@return System.Single 游戏时间 Time in seconds
function TimeAPI.GetTime() end
---游戏是否处于暂停（时间缩放为 0）。慢动作不算暂停。
---Whether the game is paused (time scale is 0). Slow motion does not count.
---@static
---@function [TimeAPI.IsPaused]
---@return System.Boolean 是否暂停 Whether the game is paused
function TimeAPI.IsPaused() end
return TimeAPI

---Transform 操作 API
---Transform API
---@class ShanghaiWindy.Core.API.TransformAPI
local TransformAPI = {}

---将欧拉角转为四元数旋转。
---Convert euler angles to quaternion rotation.
---@static
---@function [TransformAPI.EulerToRot]
---@return UnityEngine.Quaternion 四元数 Quaternion
---@param eulerAngle UnityEngine.Vector3
function TransformAPI.EulerToRot(eulerAngle) end
---寻找路径物体
---Find child by path
---@static
---@function [TransformAPI.Find]
---@return UnityEngine.Transform 目标 Transform Target transform
---@param trans UnityEngine.Transform
---@param path System.String
function TransformAPI.Find(trans, path) end
---创建 Transform
---Create a new transform
---@static
---@function [TransformAPI.CreateTransform]
---@return UnityEngine.Transform 新建的 Transform New transform
---@param transformName System.String
function TransformAPI.CreateTransform(transformName) end
---在所有层级的子物体里按名字查找（含未激活物体，不含 root 自身），没有时返回 nil。在 C# 里一次完成，不产生垃圾；
---每一层先找直接子物体，再往下找。请在 OnVehicleReady 里查一次并缓存，返回 nil 就说明模型上没有这个节点，不要每帧重试。
---Find a descendant by exact name at any depth (inactive ones included, root excluded); nil when there is none. Done in one
---C# call without garbage; each level checks its direct children before going deeper. Look it up once in OnVehicleReady and
---cache it: nil means the model has no such node, so do not retry every frame.
---@static
---@function [TransformAPI.FindDeep]
---@return UnityEngine.Transform 找到的 Transform，没有时为 nil The transform found, or nil
---@param root UnityEngine.Transform
---@param name System.String
function TransformAPI.FindDeep(root, name) end
---找出所有名字包含某段文字的子物体（任意层级，含未激活物体，不含 root 自身），按层级顺序返回数组，没有时返回空数组。
---会分配一个数组：只在初始化时调用一次。
---Find every descendant whose name contains the given text (any depth, inactive ones included, root excluded), in hierarchy
---order; an empty array when there are none. Allocates an array: call it once while initialising.
---@static
---@function [TransformAPI.FindAllDeep]
---@return UnityEngine.Transform[] 找到的 Transform 数组 Array of the transforms found
---@param root UnityEngine.Transform
---@param namePart System.String
function TransformAPI.FindAllDeep(root, namePart) end
return TransformAPI

---模式 UI API
---Mode UI API
---@class ShanghaiWindy.Core.API.UIAPI
local UIAPI = {}

---显示内置 UI
---Show builtin ui
---@static
---@function [UIAPI.ShowUI]
---@param index ShanghaiWindy.Core.UIEnum
function UIAPI.ShowUI(index) end
---关闭内置 UI
---Remove builtin ui
---@static
---@function [UIAPI.RemoveUI]
---@param index ShanghaiWindy.Core.UIEnum
function UIAPI.RemoveUI(index) end
---UI 对象池
---UI Pool
---@static
---@function [UIAPI.GetUIPool]
---@return ShanghaiWindy.Core.UIPoolComponent UI 对象池组件 UI pool component
---@param prototype UnityEngine.GameObject
function UIAPI.GetUIPool(prototype) end
---@static
---@function [UIAPI.ShowPopup]
---@return System.Void
---@param message System.String
---@param hasCancel System.Boolean
---@param callback ShanghaiWindy.Core.Delegate.UIAPIDelegate+OnPopupClick
function UIAPI.ShowPopup(message, hasCancel, callback) end
---获取本地化文字
---Get localized content
---@static
---@function [UIAPI.GetLocalizedContent]
---@return System.String 本地化文字 Localized text
---@param text System.String
function UIAPI.GetLocalizedContent(text) end
---格式化文本
---Format string
---@static
---@function [UIAPI.FormatString]
---@return System.String 格式化结果 Formatted string
---@param format System.String
---@param objects System.Object[]
function UIAPI.FormatString(format, objects) end
return UIAPI

---载具通用 API
---Vehicle Common APIs
---@class ShanghaiWindy.Core.API.VehicleAPI
local VehicleAPI = {}

---载具实例所处的场合：车库、本地玩家正在控制、本地 AI、联机远程副本、结算展示。每辆载具只得到一个结果；本地玩家拥有但此刻没在驾驶的单位（召唤、编组里的其他车）算 Bot。
---Where a vehicle instance lives: garage, controlled by the local player, local AI, a remote copy online, or a reward showcase. Each vehicle gets exactly one answer; a unit the local player owns but is not driving right now (a summon, another vehicle of a group) is Bot.
---@static
---@function [VehicleAPI.GetContext]
---@return ShanghaiWindy.Core.API.VehicleContext 场合；nil 或已销毁时为 None Context; None for nil or a destroyed vehicle
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetContext(vehicle) end
---是否为车库里的展示载具。
---Whether the vehicle is shown in the garage.
---@static
---@function [VehicleAPI.IsGarage]
---@return System.Boolean GetContext 为 Garage 时为 true True when GetContext is Garage
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsGarage(vehicle) end
---是否为本地玩家此刻正在控制的载具（包括玩家接管的 AI 载具）。
---Whether the local player controls the vehicle right now (a bot vehicle the player took over included).
---@static
---@function [VehicleAPI.IsLocalPlayer]
---@return System.Boolean GetContext 为 LocalPlayer 时为 true True when GetContext is LocalPlayer
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsLocalPlayer(vehicle) end
---是否由本机 AI 驾驶。
---Whether AI on this client drives the vehicle.
---@static
---@function [VehicleAPI.IsBot]
---@return System.Boolean GetContext 为 Bot 时为 true True when GetContext is Bot
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsBot(vehicle) end
---是否为联机中由其他客户端或服务器驱动的副本。副本不在本机跑模拟（例如雷达不扫描），状态来自网络。
---Whether the vehicle is a copy driven by another client or the server online. A copy runs no local simulation (its radar does not scan, for example); its state comes from the network.
---@static
---@function [VehicleAPI.IsRemote]
---@return System.Boolean GetContext 为 Remote 时为 true True when GetContext is Remote
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsRemote(vehicle) end
---载具是否已加载完毕且未被击毁。
---Whether the vehicle has finished loading and is not destroyed.
---@static
---@function [VehicleAPI.IsAlive]
---@return System.Boolean 是否存活 Is alive
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsAlive(vehicle) end
---获取载具所属队伍。
---Get the vehicle's team.
---@static
---@function [VehicleAPI.GetTeam]
---@return ShanghaiWindy.Core.TeamManager+Team 队伍；nil 时为 none Team; none for nil
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetTeam(vehicle) end
---两辆载具是否互为敌方。没有队伍（none）的载具与谁都不是敌方。
---Whether two vehicles are enemies. A vehicle without a team (none) is nobody's enemy.
---@static
---@function [VehicleAPI.IsEnemy]
---@return System.Boolean 是否敌对 Are enemies
---@param a ShanghaiWindy.Core.BaseInitSystem
---@param b ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsEnemy(a, b) end
---获取游戏里显示给玩家的载具名称（按当前语言）。
---Get the vehicle name the game shows players, in the current language.
---@static
---@function [VehicleAPI.GetDisplayName]
---@return System.String 显示名；未知时为空字符串 Display name; an empty string when unknown
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetDisplayName(vehicle) end
---获取载具资源 GUID，与模组包里该载具的 GUID 相同，可用来认出自己的配套载具。
---Get the vehicle's asset GUID, the same as the vehicle's GUID in its mod package; use it to recognise your own companion vehicles.
---@static
---@function [VehicleAPI.GetGuid]
---@return System.String GUID；未知时为空字符串 GUID; an empty string when unknown
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetGuid(vehicle) end
---获取载具的世界速度（米/秒）。步兵也有效。
---Get the vehicle's world velocity in metres per second. Works for infantry too.
---@static
---@function [VehicleAPI.GetVelocity]
---@return UnityEngine.Vector3 速度；nil 时为零向量 Velocity; zero for nil
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetVelocity(vehicle) end
---获取载具速度（千米/小时）。
---Get the vehicle's speed in km/h.
---@static
---@function [VehicleAPI.GetSpeedKmh]
---@return System.Single 速度；nil 时为 0 Speed; 0 for nil
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetSpeedKmh(vehicle) end
---获取 AI 瞄准这辆载具时用的点（世界坐标）。未加载完时为载具位置。
---Get the point AI aims at on this vehicle, in world space. The vehicle's position while it is still loading.
---@static
---@function [VehicleAPI.GetAimPoint]
---@return UnityEngine.Vector3 瞄准点；nil 时为零向量 Aim point; zero for nil
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetAimPoint(vehicle) end
---创建一个战场载具查询。建一次，之后反复调用 Run：查询本身不再分配内存。
---Create a battlefield vehicle query. Create it once and call Run again and again: the query allocates nothing after that.
---@static
---@function [VehicleAPI.CreateQuery]
---@return ShanghaiWindy.Core.API.VehicleQuery 新查询 New query
function VehicleAPI.CreateQuery() end
---订阅全场载具加载完毕事件（任意载具，含车库里的）。联机时本队看不到的敌方不会通知。
---Subscribe to any vehicle in the battle finishing loading (garage vehicles included). Online, an enemy your team cannot see is not reported.
---@static
---@function [VehicleAPI.OnVehicleSpawned]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅；参数无效时为 nil Subscription; nil for invalid arguments
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param callback ShanghaiWindy.Core.Delegate.OnLuaVehicleEventDelegate
function VehicleAPI.OnVehicleSpawned(owner, callback) end
---订阅全场载具被击毁事件。联机时本队看不到的敌方不会通知。
---Subscribe to any vehicle in the battle being destroyed. Online, an enemy your team cannot see is not reported.
---@static
---@function [VehicleAPI.OnVehicleKilled]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅；参数无效时为 nil Subscription; nil for invalid arguments
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param callback ShanghaiWindy.Core.Delegate.OnLuaVehicleEventDelegate
function VehicleAPI.OnVehicleKilled(owner, callback) end
---注册载具加载事件。
---Register an event dispatched when vehicle loaded.
---@static
---@function [VehicleAPI.RegisterVehicleLoadedEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param vehicleLoadedDelegate ShanghaiWindy.Core.Delegate.OnVehicleLoadedDelegate
function VehicleAPI.RegisterVehicleLoadedEvent(vehicle, vehicleLoadedDelegate) end
---取消注册载具加载事件。
---Unregister an event dispatched when vehicle loaded.
---@static
---@function [VehicleAPI.UnRegisterVehicleLoaedEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param vehicleLoadedDelegate ShanghaiWindy.Core.Delegate.OnVehicleLoadedDelegate
function VehicleAPI.UnRegisterVehicleLoaedEvent(vehicle, vehicleLoadedDelegate) end
---注册载具被摧毁事件。
---Register an event dispatched when vehicle destroyed.
---@static
---@function [VehicleAPI.RegisterVehicleDestroyedEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param vehicleDestroyedDelegate ShanghaiWindy.Core.Delegate.OnVehicleDestroyedDelegate
function VehicleAPI.RegisterVehicleDestroyedEvent(vehicle, vehicleDestroyedDelegate) end
---注销载具被摧毁时触发的事件。
---Unregister an event dispatched when vehicle destroyed.
---@static
---@function [VehicleAPI.UnRegisterVehicleDestroyedEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param vehicleDestroyedDelegate ShanghaiWindy.Core.Delegate.OnVehicleDestroyedDelegate
function VehicleAPI.UnRegisterVehicleDestroyedEvent(vehicle, vehicleDestroyedDelegate) end
---注册载具物体被摧毁时触发的事件。
---Register an event dispatched when vehicle gamobject destroyed.
---@static
---@function [VehicleAPI.RegisterVehicleGameObjectDestroyedEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param vehicleGameObjectDestroyed ShanghaiWindy.Core.Delegate.OnVehicleRemovedDelegate
function VehicleAPI.RegisterVehicleGameObjectDestroyedEvent(vehicle, vehicleGameObjectDestroyed) end
---注销载具物体被摧毁时触发的事件。
---Unregister an event dispatched when vehicle gamobject destroyed.
---@static
---@function [VehicleAPI.UnRegisterVehicleGameObjectDestroyedEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param vehicleGameObjectDestroyed ShanghaiWindy.Core.Delegate.OnVehicleRemovedDelegate
function VehicleAPI.UnRegisterVehicleGameObjectDestroyedEvent(vehicle, vehicleGameObjectDestroyed) end
---注册载具开火时触发的事件。
---Register an event dispatched when vehicle fired.
---@static
---@function [VehicleAPI.RegisterBulletFiredEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param index System.Int32
---@param tankFiredDelegate ShanghaiWindy.Core.Delegate.OnFiredDelegate
function VehicleAPI.RegisterBulletFiredEvent(vehicle, index, tankFiredDelegate) end
---注销载具开火时触发的事件。
---UnRegister an event dispatched when vehicle fired.
---@static
---@function [VehicleAPI.UnRegisterBulletFiredEvent]
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param index System.Int32
---@param tankFiredDelegate ShanghaiWindy.Core.Delegate.OnFiredDelegate
function VehicleAPI.UnRegisterBulletFiredEvent(vehicle, index, tankFiredDelegate) end
---获取玩家的载具。如果玩家未出现，则可能为空。
---Get the player vehicle. It can be null if player is not spawned.
---@static
---@function [VehicleAPI.GetPlayerVehicle]
---@return ShanghaiWindy.Core.BaseInitSystem 一个已存在的本地玩家 A exist local player
function VehicleAPI.GetPlayerVehicle() end
---判断载具是否为坦克类型
---@static
---@function [VehicleAPI.IsTankVehicle]
---@return System.Boolean 返回载具是否为坦克类型 true if TankVehicle, false otherwise
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsTankVehicle(vehicle) end
---判断载具是否为飞行器类型
---@static
---@function [VehicleAPI.IsFlightVehicle]
---@return System.Boolean 返回载具是否为飞行器类型 true if FlightVehicle, false otherwise
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsFlightVehicle(vehicle) end
---判断载具是否为陆军类型
---@static
---@function [VehicleAPI.IsArmyVehicle]
---@return System.Boolean 返回载具是否为陆军类型 true if ArmyVehicle, false otherwise
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.IsArmyVehicle(vehicle) end
---@static
---@function [VehicleAPI.TryGetBaseInitSystemFromGameObject]
---@return System.Boolean
---@param gameObject UnityEngine.GameObject
---@param baseInitSystem ShanghaiWindy.Core.BaseInitSystem&
function VehicleAPI.TryGetBaseInitSystemFromGameObject(gameObject, baseInitSystem) end
---@static
---@function [VehicleAPI.TryGetTankInitSystemFromGameObject]
---@return System.Boolean
---@param gameObject UnityEngine.GameObject
---@param vehicleInitSystem ShanghaiWindy.Core.VehicleInitSystem&
function VehicleAPI.TryGetTankInitSystemFromGameObject(gameObject, vehicleInitSystem) end
---@static
---@function [VehicleAPI.TryGetFlightInitSystemFromGameObject]
---@return System.Boolean
---@param gameObject UnityEngine.GameObject
---@param flightInitSystem ShanghaiWindy.Core.FlightInitSystem&
function VehicleAPI.TryGetFlightInitSystemFromGameObject(gameObject, flightInitSystem) end
---@static
---@function [VehicleAPI.TryGetArmyInitSystemFromGameObject]
---@return System.Boolean
---@param gameObject UnityEngine.GameObject
---@param armyInitSystem ShanghaiWindy.Core.FPS.ArmyInitSystem&
function VehicleAPI.TryGetArmyInitSystemFromGameObject(gameObject, armyInitSystem) end
---从载具中获取所有火力系统。
---Get all the fire system from vehicle.
---@static
---@function [VehicleAPI.GetFireList]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.IBaseFireSystem, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] 火力系统数组 Array of fire system
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function VehicleAPI.GetFireList(vehicle) end
---获取载具信息
---Get Vehicle Info
---@static
---@function [VehicleAPI.GetVehicleInfoByName]
---@return ShanghaiWindy.Core.VehicleInfo 载具信息 Vehicle Info
---@param vehicleName System.String
function VehicleAPI.GetVehicleInfoByName(vehicleName) end
---获取载具信息
---Get vehicle info guid
---@static
---@function [VehicleAPI.GetVehicleInfoByGuid]
---@return ShanghaiWindy.Core.VehicleInfo 载具信息 Vehicle info
---@param guid System.String
function VehicleAPI.GetVehicleInfoByGuid(guid) end
---获取所有可驾驶载具列表
---Get All Driveable Vehicle List
---@static
---@function [VehicleAPI.GetAllDriveableVehicleList]
---@return ShanghaiWindy.Core.DriveableList 可驾驶载具列表 Driveable Vehicle List
---@param ignoreModule System.Boolean
function VehicleAPI.GetAllDriveableVehicleList(ignoreModule) end
---过滤载具
---Filter Vehicle
---@static
---@function [VehicleAPI.GetFilteredVehicles]
---@return ShanghaiWindy.Core.DriveableList 载具列表 Vehicle List
---@param minRank System.Int32
---@param maxRank System.Int32
function VehicleAPI.GetFilteredVehicles(minRank, maxRank) end
---@static
---@function [VehicleAPI.GetFilteredBotVehicles]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.VehicleInfo, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@param minRank System.Int32
---@param maxRank System.Int32
---@param allowArtillery System.Boolean
---@param vehicleType ShanghaiWindy.Core.VehicleInfo+Type
function VehicleAPI.GetFilteredBotVehicles(minRank, maxRank, allowArtillery, vehicleType) end
---@static
---@function [VehicleAPI.RandomPickVehicleFromList]
---@return System.Collections.Generic.List`1[[ShanghaiWindy.Core.VehicleInfo, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@param vehicleList System.Collections.Generic.List`1[[ShanghaiWindy.Core.VehicleInfo, Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@param maxVehicles System.Int32
function VehicleAPI.RandomPickVehicleFromList(vehicleList, maxVehicles) end
---加载缩略图
---@static
---@function [VehicleAPI.LoadVehicleThumbnail]
---@param vehicleInfo ShanghaiWindy.Core.VehicleInfo
---@param callback ShanghaiWindy.Core.Delegate.OnVehicleThumbnailLoadedDelegate
function VehicleAPI.LoadVehicleThumbnail(vehicleInfo, callback) end
return VehicleAPI

---战场载具查询：按类型、敌友、距离筛选场上存活的载具（含联机远程玩家、召唤和部署的单位），可按距离排序并限量。用 VehicleAPI.CreateQuery 建一次，反复 Run；Run 之后用 Count / Get / GetDistance 读结果。联机时只返回本队当前看得到的敌方。
---Battlefield vehicle query: filters the live vehicles in the battle (remote players online, summoned and deployed units included) by kind, relation and distance, optionally sorted by distance and capped. Create it once with VehicleAPI.CreateQuery and Run it again and again; read the results with Count / Get / GetDistance after Run. Online it only returns enemies your team can currently see.
---@class ShanghaiWindy.Core.API.VehicleQuery
---@field Kind ShanghaiWindy.Core.API.VehicleKind
---@field Relation ShanghaiWindy.Core.API.VehicleRelation
---@field MaxDistance System.Single
---@field MaxCount System.Int32
---@field SortByDistance System.Boolean
---@field Count System.Int32
local VehicleQuery = {}

---以某辆载具为中心查询：距离从它的位置量起，敌友相对它的队伍，不含它自己。
---Query around a vehicle: distances from its position, relation relative to its team, the vehicle itself left out.
---@instance
---@function [VehicleQuery:Run]
---@return System.Int32 结果数量；nil、不在战斗中时为 0 Number of results; 0 for nil or outside a battle
---@param self ShanghaiWindy.Core.BaseInitSystem
function VehicleQuery:Run(self) end
---@instance
---@function [VehicleQuery:RunAt]
---@return System.Int32
---@param position UnityEngine.Vector3
---@param team ShanghaiWindy.Core.TeamManager+Team
function VehicleQuery:RunAt(position, team) end
---获取第 index 个结果（从 0 开始）。结果是 Run 那一刻的；之后被移除的载具返回 nil。
---Get result number index (0-based). Results are as of the Run; a vehicle removed since returns nil.
---@instance
---@function [VehicleQuery:Get]
---@return ShanghaiWindy.Core.BaseInitSystem 载具；越界时为 nil Vehicle; nil when out of range
---@param index System.Int32
function VehicleQuery:Get(index) end
---获取第 index 个结果在 Run 那一刻与中心的距离（米）。
---Get the distance in metres of result number index from the centre, as of the Run.
---@instance
---@function [VehicleQuery:GetDistance]
---@return System.Single 距离；越界时为 0 Distance; 0 when out of range
---@param index System.Int32
function VehicleQuery:GetDistance(index) end
return VehicleQuery

---武器 API。坦克、飞机和步兵的武器通用。
---Weapon API, shared by tank, aircraft and infantry weapons.
---@class ShanghaiWindy.Core.API.WeaponAPI
local WeaponAPI = {}

---获取载具的武器数量。
---Get how many weapons a vehicle has.
---@static
---@function [WeaponAPI.GetWeaponCount]
---@return System.Int32 武器数量，载具无效时为 0 Weapon count, 0 for an invalid vehicle
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
function WeaponAPI.GetWeaponCount(vehicle) end
---按序号获取载具的武器，序号从 0 开始，与 VehicleAPI.GetFireList 的顺序一致。
---Get a vehicle's weapon by index, from 0, in the order of VehicleAPI.GetFireList.
---@static
---@function [WeaponAPI.GetWeapon]
---@return ShanghaiWindy.Core.IBaseFireSystem 武器，没有时为 nil The weapon, nil when there is none
---@param vehicle ShanghaiWindy.Core.BaseInitSystem
---@param index System.Int32
function WeaponAPI.GetWeapon(vehicle, index) end
---获取武器的弹种数量。
---Get how many shell types a weapon carries.
---@static
---@function [WeaponAPI.GetAmmoTypeCount]
---@return System.Int32 弹种数量 Shell type count
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetAmmoTypeCount(weapon) end
---获取某一弹种的剩余弹量。
---Get the rounds left of one shell type.
---@static
---@function [WeaponAPI.GetAmmo]
---@return System.Int32 剩余弹量 Rounds left
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param typeIndex System.Int32
function WeaponAPI.GetAmmo(weapon, typeIndex) end
---获取所有弹种的剩余弹量之和。
---Get the rounds left over all shell types.
---@static
---@function [WeaponAPI.GetTotalAmmo]
---@return System.Int32 剩余弹量 Rounds left
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetTotalAmmo(weapon) end
---获取当前选用的弹种序号。
---Get the index of the selected shell type.
---@static
---@function [WeaponAPI.GetSelectedAmmoType]
---@return System.Int32 弹种序号 Shell type index
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetSelectedAmmoType(weapon) end
---某一弹种是否已经打光。序号超出范围时视为打光。
---Whether a shell type is used up. An index out of range counts as used up.
---@static
---@function [WeaponAPI.IsOutOfAmmo]
---@return System.Boolean 是否打光；武器无效时为 false Whether it is used up; false for an invalid weapon
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param typeIndex System.Int32
function WeaponAPI.IsOutOfAmmo(weapon, typeIndex) end
---武器是否装填完毕、可以开火（当前弹种还有弹）。
---Whether the weapon is loaded and ready to fire (the selected shell type has rounds left).
---@static
---@function [WeaponAPI.IsReloaded]
---@return System.Boolean 是否装填完毕 Whether it is loaded
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.IsReloaded(weapon) end
---获取当前这一段装填的进度，0 到 1，装填完毕为 1。弹匣 / 弹鼓武器是正在进行的那一段（换弹匣或两发之间）。
---Get the progress of the reload step in progress, 0 to 1, 1 when loaded. For magazine and drum weapons this is the
---step under way (a new magazine, or the gap between two rounds).
---@static
---@function [WeaponAPI.GetReloadProgress]
---@return System.Single 装填进度 Reload progress
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetReloadProgress(weapon) end
---获取当前这一段装填的总时长（秒），已计入装填 buff，与游戏装填界面显示的一致；装填完毕时是开火后下一段的时长。
---装填手受伤等造成的减速不计入（游戏界面另行显示）。步兵武器为 0。
---Get the length in seconds of the reload step in progress, reload buffs included, as the game's reload HUD shows it;
---when loaded, of the step the next shot starts. Slowdowns such as an injured loader are not included (the HUD
---shows them apart). 0 for infantry weapons.
---@static
---@function [WeaponAPI.GetReloadTime]
---@return System.Single 装填时长（秒） Reload time in seconds
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetReloadTime(weapon) end
---获取炮口节点（炮弹生成点）。多管武器为下一发使用的炮管。
---Get the muzzle (where shells spawn). For a multi-barrel weapon, the barrel the next shot leaves from.
---@static
---@function [WeaponAPI.GetMuzzle]
---@return UnityEngine.Transform 炮口节点 Muzzle transform
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetMuzzle(weapon) end
---获取当前弹种的初速（米/秒），即游戏发射时使用的速度。火箭和导弹是加速后的速度。
---Get the muzzle velocity of the selected shell in m/s, the speed the game fires it at. For rockets and missiles,
---the speed they accelerate to.
---@static
---@function [WeaponAPI.GetShellSpeed]
---@return System.Single 初速（米/秒） Muzzle velocity in m/s
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
function WeaponAPI.GetShellSpeed(weapon) end
---@static
---@function [WeaponAPI.PredictLead]
---@return System.Boolean
---@param shooter ShanghaiWindy.Core.BaseInitSystem
---@param target ShanghaiWindy.Core.BaseInitSystem
---@param point UnityEngine.Vector3&
function WeaponAPI.PredictLead(shooter, target, point) end
---订阅开火（多发齐射在整组打完后发出一次）。
---Subscribe to the weapon firing (a salvo raises it once, after its last round).
---@static
---@function [WeaponAPI.OnFired]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaEventDelegate
function WeaponAPI.OnFired(owner, weapon, callback) end
---订阅弹量变化：开火消耗、飞机挂架重新装弹、补给、联机同步都会发出。回调里用 GetAmmo / GetTotalAmmo 读取新弹量。
---Subscribe to round count changes: a shot, an aircraft rack rearming, a resupply and an online sync all raise it.
---Read the new counts with GetAmmo / GetTotalAmmo in the callback.
---@static
---@function [WeaponAPI.OnAmmoChanged]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaEventDelegate
function WeaponAPI.OnAmmoChanged(owner, weapon, callback) end
---订阅装填完成：武器从未就绪变为可以开火时发出（弹匣 / 弹鼓武器每一发就绪都会发出；打光的弹种补给后也会发出）。
---只在有订阅时检测，没有订阅时没有开销。步兵武器不发出。
---Subscribe to reload completion: raised when the weapon goes from not ready to ready to fire (for magazine and drum
---weapons, each round that becomes ready; also after a used-up shell is resupplied). Checked only while subscribed.
---Infantry weapons never raise it.
---@static
---@function [WeaponAPI.OnReloaded]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaEventDelegate
function WeaponAPI.OnReloaded(owner, weapon, callback) end
---订阅命中造成伤害。point 在单机血量模式下是被击中载具的位置，纯模块伤害模式下为 (0,0,0)，联机时为服务器给出的命中点。
---Subscribe to this weapon's hits dealing damage. point is the hit vehicle's position in offline health-based damage,
---(0,0,0) in pure module damage, and the server's hit point online.
---@static
---@function [WeaponAPI.OnHit]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaHitEventDelegate
function WeaponAPI.OnHit(owner, weapon, callback) end
---订阅击毁：回调参数是被这件武器击毁的载具。不计击杀的目标（如拖车）不会发出。
---Subscribe to kills: the callback gets the vehicle this weapon destroyed. Targets that grant no kill (a trailer) do
---not raise it.
---@static
---@function [WeaponAPI.OnKill]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaVehicleEventDelegate
function WeaponAPI.OnKill(owner, weapon, callback) end
---订阅未击穿：回调参数是命中处的装甲值。
---Subscribe to shots that do not penetrate: the callback gets the armor value at the hit.
---@static
---@function [WeaponAPI.OnNotPierced]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaIntEventDelegate
function WeaponAPI.OnNotPierced(owner, weapon, callback) end
---订阅跳弹。
---Subscribe to ricochets.
---@static
---@function [WeaponAPI.OnRicochet]
---@return ShanghaiWindy.Core.API.LuaSubscription 订阅，失败时为 nil The subscription, nil on failure
---@param owner ShanghaiWindy.Core.Lua.LuaBehaviorProxy
---@param weapon ShanghaiWindy.Core.IBaseFireSystem
---@param callback ShanghaiWindy.Core.Delegate.OnLuaEventDelegate
function WeaponAPI.OnRicochet(owner, weapon, callback) end
return WeaponAPI

---@class ShanghaiWindy.Core.IVehicleController
local IVehicleController = {}

return IVehicleController

---@class ShanghaiWindy.Core.UIPoolComponent
local UIPoolComponent = {}

---@instance
---@function [UIPoolComponent:Get]
---@return UnityEngine.GameObject
---@param stayWorldPos System.Boolean
function UIPoolComponent:Get(stayWorldPos) end
---@instance
---@function [UIPoolComponent:Release]
---@return System.Void
---@param instance UnityEngine.GameObject
function UIPoolComponent:Release(instance) end
---@instance
---@function [UIPoolComponent:Clear]
---@return System.Void
function UIPoolComponent:Clear() end
---@instance
---@function [UIPoolComponent:Dispose]
---@return System.Void
function UIPoolComponent:Dispose() end
---@instance
---@function [UIPoolComponent:GetActiveCount]
---@return System.Int32
function UIPoolComponent:GetActiveCount() end
return UIPoolComponent

