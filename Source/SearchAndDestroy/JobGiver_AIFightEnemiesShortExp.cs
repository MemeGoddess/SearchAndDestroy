using RimWorld;
using SearchAndDestroy.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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
                    Log.Warning($"{target.Label}: Returning false");
                    return false;
                }
            }

            if (target is Building targetBuilding && (pawn.equipment.Primary?.def.IsRangedWeapon ?? false))
            {
                // Disable targeting for range, there's some weird stuff further down the tree that causes it to ignore for shooting
                return false;
            }
            
            var baseResult = base.ExtraTargetValidator(pawn, target);
            return baseResult;
        }
    }
}
