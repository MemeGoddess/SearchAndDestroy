using RimWorld;
using SearchAndDestroy.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace SearchAndDestroy
{
    class JobGiver_AIFightEnemiesShortExp : JobGiver_AIFightEnemies
    {
		protected override Job TryGiveJob(Pawn pawn)
        {
            var originalResponse = pawn.playerSettings.hostilityResponse;
            pawn.playerSettings.hostilityResponse = HostilityResponseMode.Attack;
			var job = base.TryGiveJob(pawn);
            if (job != null)
            {
				job.expiryInterval = 30;
            }
            pawn.playerSettings.hostilityResponse = originalResponse;
            return job;
		}
        protected override bool ExtraTargetValidator(Pawn pawn, Thing target)
        {
            if (target is Pawn targetPawn)
            {
                if (targetPawn.NonHumanlikeOrWildMan() && !targetPawn.IsAttacking())
                {
                    return false;
                }
            }

            if (target is Building targetBuilding)
            {
                // Check it's attackable?
            }
            
            return base.ExtraTargetValidator(pawn, target);
        }
    }
}
