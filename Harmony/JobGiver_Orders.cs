using HarmonyLib;
using SearchAndDestroy.Storage;
using Verse;
using Verse.AI;

namespace SearchAndDestroy.Harmony
{
    [HarmonyPatchCategory(nameof(Tacticowl.PatchCategories.SearchAndDestroy))]
    [HarmonyPatch(typeof(JobGiver_Orders), "TryGiveJob")]
    static class JobGiver_Orders_TryGiveJob
    {
        static void Postfix(Pawn pawn, Job __result)
        {
            if (__result != null)
            {
                ExtendedDataStorage store = ExtendedDataStorage.GetComp();
                if (store != null)
                {
                    ExtendedPawnData pawnData = store.GetExtendedDataFor(pawn);
                    if (pawnData.SD_enabled)
                    {
                        __result.expiryInterval = 60;
                    }
                }

            }
        }
    }
}
