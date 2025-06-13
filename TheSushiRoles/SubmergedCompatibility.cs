using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;
namespace TheSushiRoles
{
    public static class SubmergedCompatibility
    {
        public static class Classes
        {
            public const string ElevatorMover = "ElevatorMover";
        }

        public const string SUBMERGED_GUID = "Submerged";
        public const ShipStatus.MapType SUBMERGED_MAP_TYPE = (ShipStatus.MapType)6;

        public static SemanticVersioning.Version Version { get; private set; }
        public static bool Loaded { get; private set; }
        public static bool LoadedExternally { get; private set; }
        public static BasePlugin Plugin { get; private set; }
        public static Assembly Assembly { get; private set; }
        public static Type[] Types { get; private set; }
        public static Dictionary<string, Type> InjectedTypes { get; private set; }

        public static MonoBehaviour SubmarineStatus { get; private set; }
        public static bool IsSubmerged { get; private set; }

        private static Type SubmarineStatusType;
        private static MethodInfo CalculateLightRadiusMethod;

        private static MethodInfo RpcRequestChangeFloorMethod;
        private static Type FloorHandlerType;
        private static MethodInfo GetFloorHandlerMethod;

        private static Type VentPatchDataType;
        private static PropertyInfo InTransitionField;

        private static Type CustomTaskTypesType;
        private static FieldInfo RetrieveOxigenMaskField;
        public static TaskTypes RetrieveOxygenMask;
        private static Type SubmarineOxygenSystemType;
        private static MethodInfo SubmarineOxygenSystemInstanceField;
        private static MethodInfo RepairDamageMethod;

        private static Type CustomPlayerData;
        private static FieldInfo hasMap;

        private static FieldInfo SubmergedInstance;
        private static FieldInfo SubmergedElevators;

        private static FieldInfo getSubElevatorSystem;
        private static Type SubmarineElevator;
        private static MethodInfo GetInElevator;

        private static Type SubmarineElevatorSystem;
        private static FieldInfo UpperDeckIsTargetFloor;

        private static Type SpawnInState;
        private static FieldInfo currentState;

        public static void SetupMap(ShipStatus map)
        {
            if (map == null)
            {
                IsSubmerged = false;
                SubmarineStatus = null;
                return;
            }

            IsSubmerged = map.Type == SUBMERGED_MAP_TYPE;
            if (!IsSubmerged) return;

            SubmarineStatus = map.GetComponent(Il2CppType.From(SubmarineStatusType))?.TryCast(SubmarineStatusType) as MonoBehaviour;
        }

        public static bool TryLoadSubmerged()
        {
            try
            {
                TheSushiRoles.Logger.LogMessage("Trying to load Submerged...");
                var thisAsm = Assembly.GetCallingAssembly();
                var resourceName = thisAsm.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith("Submerged.dll"));
                if (resourceName == default) return false;

                using var submergedStream = thisAsm.GetManifestResourceStream(resourceName)!;
                byte[] assemblyBuffer = new byte[submergedStream.Length];
                submergedStream.Read(assemblyBuffer, 0, assemblyBuffer.Length);
                Assembly = Assembly.Load(assemblyBuffer);

                var pluginType = Assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(BasePlugin)));
                Plugin = (BasePlugin)Activator.CreateInstance(pluginType!);
                Plugin.Load();

                Version = pluginType.GetCustomAttribute<BepInPlugin>().Version.BaseVersion();

                IL2CPPChainloader.Instance.Plugins[SUBMERGED_GUID] = new();
                return true;
            }
            catch (Exception e)
            {
                TheSushiRoles.Logger.LogError(e);
            }
            return false;
        }

        public static void Initialize()
        {
            Loaded = IL2CPPChainloader.Instance.Plugins.TryGetValue(SUBMERGED_GUID, out var plugin);

            if (!Loaded)
            {
                if (TryLoadSubmerged()) Loaded = true;
                else return;
            }
            else
            {
                LoadedExternally = true;
                Plugin = plugin!.Instance as BasePlugin;
                Version = plugin.Metadata.Version.BaseVersion();
                Assembly = Plugin!.GetType().Assembly;
            }

            Types = AccessTools.GetTypesFromAssembly(Assembly);

            InjectedTypes = (Dictionary<string, Type>)AccessTools.PropertyGetter(Types.FirstOrDefault(t => t.Name == "ComponentExtensions"), "RegisteredTypes")
                .Invoke(null, Array.Empty<object>());

            SubmarineStatusType = Types.First(t => t.Name == "SubmarineStatus");
            CalculateLightRadiusMethod = AccessTools.Method(SubmarineStatusType, "CalculateLightRadius");

            FloorHandlerType = Types.First(t => t.Name == "FloorHandler");
            GetFloorHandlerMethod = AccessTools.Method(FloorHandlerType, "GetFloorHandler", new Type[] { typeof(PlayerControl) });
            RpcRequestChangeFloorMethod = AccessTools.Method(FloorHandlerType, "RpcRequestChangeFloor");

            VentPatchDataType = Types.First(t => t.Name == "VentPatchData");

            SubmergedInstance = AccessTools.Field(SubmarineStatusType, "instance");
            SubmergedElevators = AccessTools.Field(SubmarineStatusType, "elevators");

            SubmarineElevator = Types.First(t => t.Name == "SubmarineElevator");
            GetInElevator = AccessTools.Method(SubmarineElevator, "GetInElevator", new Type[] { typeof(PlayerControl) });
            getSubElevatorSystem = AccessTools.Field(SubmarineElevator, "system");

            SubmarineElevatorSystem = Types.First(t => t.Name == "SubmarineElevatorSystem");
            UpperDeckIsTargetFloor = AccessTools.Field(SubmarineElevatorSystem, "upperDeckIsTargetFloor");

            InTransitionField = AccessTools.Property(VentPatchDataType, "InTransition");

            CustomTaskTypesType = Types.First(t => t.Name == "CustomTaskTypes");
            RetrieveOxigenMaskField = AccessTools.Field(CustomTaskTypesType, "RetrieveOxygenMask");
            var RetrieveOxigenMaskTaskTypeField = AccessTools.Field(CustomTaskTypesType, "taskType");
            object OxygenMaskCustomTaskType = RetrieveOxigenMaskField.GetValue(null);
            RetrieveOxygenMask = (TaskTypes)RetrieveOxigenMaskTaskTypeField.GetValue(OxygenMaskCustomTaskType);

            SubmarineOxygenSystemType = Types.First(t => t.Name == "SubmarineOxygenSystem" && t.Namespace == "Submerged.Systems.Oxygen");
            SubmarineOxygenSystemInstanceField = AccessTools.PropertyGetter(SubmarineOxygenSystemType, "Instance");
            RepairDamageMethod = AccessTools.Method(SubmarineOxygenSystemType, "RepairDamage");

            CustomPlayerData = InjectedTypes.FirstOrDefault(t => t.Key == "CustomPlayerData").Value;
            hasMap = AccessTools.Field(CustomPlayerData, "_hasMap");

            SpawnInState = Types.First(t => t.Name == "SpawnInState");

            var subSpawnSystem = Types.First(t => t.Name == "SubmarineSpawnInSystem");
            var GetReadyPlayerAmount = AccessTools.Method(subSpawnSystem, "GetReadyPlayerAmount");
            currentState = AccessTools.Field(subSpawnSystem, "currentState");

            TheSushiRoles.Singleton.Harmony.Patch(GetReadyPlayerAmount, new(AccessTools.Method(typeof(SubmergedCompatibility), nameof(ReadyPlayerAmount))));
        }

        public static bool ReadyPlayerAmount(dynamic __instance, ref int __result)
        {
            if (!Loaded) return true;

            if (TheSushiRoles.DebuggerLoaded)
            {
                __result = __instance.GetTotalPlayerAmount();
                Enum.TryParse(SpawnInState, "Done", true, out var e);
                currentState.SetValue(__instance, e);
                return false;
            }

            return true;
        }

        public static void ImpartSub(PlayerControl bot)
        {
            var comp = bot.gameObject.AddComponent(Il2CppType.From(CustomPlayerData)).TryCast(CustomPlayerData);
            hasMap.SetValue(comp, true);
        }
        public static object TryCast(this Il2CppObjectBase self, Type type) => AccessTools.Method(self.GetType(), nameof(Il2CppObjectBase.TryCast)).MakeGenericMethod(type).Invoke(self, null);

        public static MonoBehaviour AddSubmergedComponent(this GameObject obj, string typeName)
        {
            if (!Loaded) return obj.AddComponent<MissingSubmergedBehaviour>();
            bool validType = InjectedTypes.TryGetValue(typeName, out Type type);
            return validType ? obj.AddComponent(Il2CppType.From(type)).TryCast<MonoBehaviour>() : obj.AddComponent<MissingSubmergedBehaviour>();
        }

        public static float GetSubmergedNeutralLightRadius(bool isImpostor)
        {
            if (!Loaded) return 0;
            return (float)CalculateLightRadiusMethod.Invoke(SubmarineStatus, new object[] { null, true, isImpostor });
        }

        public static void ChangeFloor(bool toUpper)
        {
            if (!Loaded) return;
            MonoBehaviour _floorHandler = ((Component)GetFloorHandlerMethod.Invoke(null, new object[] { PlayerControl.LocalPlayer })).TryCast(FloorHandlerType) as MonoBehaviour;
            RpcRequestChangeFloorMethod.Invoke(_floorHandler, new object[] { toUpper });
        }
        public static void CheckOutOfBoundsElevator(PlayerControl player)
        {
            if (!Loaded) return;
            if (!IsSubmerged) return;

            Tuple<bool, object> elevator = GetPlayerElevator(player);
            if (!elevator.Item1) return;
            bool CurrentFloor = (bool)UpperDeckIsTargetFloor.GetValue(getSubElevatorSystem.GetValue(elevator.Item2)); //true is top, false is bottom
            bool PlayerFloor = player.transform.position.y > -7f; //true is top, false is bottom

            if (CurrentFloor != PlayerFloor)
            {
                ChangeFloor(CurrentFloor);
            }
        }

        public static Tuple<bool, object> GetPlayerElevator(PlayerControl player)
        {
            if (!IsSubmerged) return Tuple.Create(false, (object)null);
            IList elevatorlist = Utils.CreateList(SubmarineElevator);
            elevatorlist = (IList)SubmergedElevators.GetValue(SubmergedInstance.GetValue(null));
            foreach (object elevator in elevatorlist)
            {
                if ((bool)GetInElevator.Invoke(elevator, new object[] { player })) return Tuple.Create(true, elevator);
            }

            return Tuple.Create(false, (object)null);
        }

        public static bool GetInTransition()
        {
            if (!Loaded) return false;
            return (bool)InTransitionField.GetValue(null);
        }

        public static void RepairOxygen()
        {
            if (!Loaded) return;
            try
            {
                ShipStatus.Instance.RpcRepairSystem((SystemTypes)130, 64);
                RepairDamageMethod.Invoke(SubmarineOxygenSystemInstanceField.Invoke(null, Array.Empty<object>()), new object[] { PlayerControl.LocalPlayer, 64 });
            }
            catch (System.NullReferenceException)
            {
                TheSushiRoles.Logger.LogMessage("null reference in engineer oxygen fix");
            }
        }
    }

    public class MissingSubmergedBehaviour : MonoBehaviour
    {
        static MissingSubmergedBehaviour() => ClassInjector.RegisterTypeInIl2Cpp<MissingSubmergedBehaviour>();
        public MissingSubmergedBehaviour(IntPtr ptr) : base(ptr) { }
    }
}
