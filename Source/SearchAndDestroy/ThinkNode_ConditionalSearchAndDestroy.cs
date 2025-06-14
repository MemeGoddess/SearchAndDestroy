using SearchAndDestroy.Storage;
using Verse;
using Verse.AI;

namespace SearchAndDestroy
{
    class ThinkNode_ConditionalSearchAndDestroy : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            ExtendedPawnData pawnData = Base.Instance.ExtendedDataStorage.GetExtendedDataFor(pawn);
            return pawn.Drafted && pawnData.SD_enabled;
        }
    }
}
