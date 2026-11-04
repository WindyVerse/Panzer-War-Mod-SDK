using ShanghaiWindy.Core;
using ShanghaiWindy.Core.GameMode;
using UnityEngine;
using ShanghaiWindy.Core.API;

namespace Tools.RuntimeDebugger.Scripts.GameMode
{
    public class VehicleTestFieldGameMode : BaseGameMode
    {
        private OfflineMainBattlePlayer _localPlayer;
        private BaseInitSystem _playerVehicle => _localPlayer.Vehicle;

        public override void OnStartNode()
        {
            base.OnStartNode();
            var mapData = ScriptableObject.CreateInstance<MapData>();
            mapData.mapSupportVehicleTypes.Add(EMapSupportVehicleType.Ground);
            mapData.mapSupportVehicleTypes.Add(EMapSupportVehicleType.Ship);
            mapData.mapSupportVehicleTypes.Add(EMapSupportVehicleType.Aero);
            
            MapDataManager.Instance.currentMap = mapData;

            _localPlayer = BattlePlayerManager.Instance.CreateOfflineMainPlayer(1, null);
            _localPlayer.OnVehicleDestroyed.AddListener(ShowPickVehicle);
            GameModeManager.Instance.AddBattlePlayer(_localPlayer);

            GameModeManager.Instance.LoadBattleUI(() =>
            {
                GameModeManager.Instance.EnableCheat();
                ShowPickVehicle();
            });
        }


        public void ReloadVehicle()
        {
            var curVehicleInfo = VehicleInfoManager.Instance.GetVehicleInfoByName(_playerVehicle.VehicleName);
            var pos = _playerVehicle.GetRigidbody().position;
            var rot = _playerVehicle.GetRigidbody().rotation;

            KillCurVehicle();
            CreateVehicle(curVehicleInfo, pos, rot);
        }

        public void RespawnVehicle()
        {
            KillCurVehicle();
            ShowPickVehicle();
        }

        public void ShowPickVehicle()
        {
            ModeAPI.ShowPickVehicleUI(true);
        }

        public override void AddListeners()
        {
            base.AddListeners();
            SHWEventManager.AddListener<VehiclePickEvent>(HandlePickVehicle);
        }


        public override void RemoveListeners()
        {
            base.RemoveListeners();
            SHWEventManager.RemoveListener<VehiclePickEvent>(HandlePickVehicle);
        }

        public override void OnLogicUpdated(float deltaTime)
        {
        }

        public override void OnFixedUpdated()
        {
        }

        public override void OnFrameUpdated()
        {
        }

        public override bool IsOfflineMode()
        {
            return true;
        }


        private void HandlePickVehicle(VehiclePickEvent evtData)
        {
            PointManager.Instance.OnNewSceneLoaded();
            PointManager.Instance.AsyncSpawn(evtData.VehicleInfo.preferSpawnPoint, TeamManager.Team.red,
                res =>
                {
                    CreateVehicle(evtData.VehicleInfo, res.position, res.rotation);
                    Debug.Log($"Create vehicle for: {evtData.VehicleInfo.GetDisplayName()}");
                });
        }


        private void CreateVehicle(VehicleInfo vehicleInfo, Vector3 pos, Quaternion rot)
        {
            _localPlayer.CreateVehicle(vehicleInfo, pos, rot);
        }


        private void KillCurVehicle()
        {
            if (_playerVehicle != null)
            {
                _playerVehicle.basePawn.OnDeadActionCalled();
                Object.DestroyImmediate(_playerVehicle.gameObject);
            }
        }
    }
}