using HarmonyLib;
using SearchAndDestroy.Storage;
using System;
using Verse;

namespace SearchAndDestroy
{
    class Base : Mod
    {
        public static Base Instance { get; private set; }

        public ExtendedDataStorage ExtendedDataStorage { get; private set; }

        private HarmonyLib.Harmony harmony;

        public Base(ModContentPack content) : base(content)
        {
            Instance = this;
            harmony = new HarmonyLib.Harmony("MemeGoddess.SearchAndDestroy");
            try
            {
                harmony.PatchAll(GetType().Assembly);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to apply Harmony patches for Search & Destroy: {ex as object}");
            }

        }

        public static void LoadExtendedDataStorage()
        {
            Instance.ExtendedDataStorage = Find.World.GetComponent<ExtendedDataStorage>();
        }

        
    }

    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch("FinalizeInit")]
    [HarmonyPatch(new System.Type[] { })]
    internal static class ExtendedDataStorageLoader
    {
        [HarmonyPostfix]
        private static void Load() => Base.LoadExtendedDataStorage();
    }


}
