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
    class JobGiver_GoWithinRangeOfHostile : ThinkNode_JobGiver
	{
        private bool ignoreNonCombatants;
        private bool humanlikesOnly;
        private int overrideExpiryInterval = -1;
        private int overrideInstancedExpiryInterval = -1;

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_GoWithinRangeOfHostile gotoNearestHostile = (JobGiver_GoWithinRangeOfHostile)base.DeepCopy(resolve);
            gotoNearestHostile.ignoreNonCombatants = this.ignoreNonCombatants;
            gotoNearestHostile.humanlikesOnly = this.humanlikesOnly;
            gotoNearestHostile.overrideExpiryInterval = this.overrideExpiryInterval;
            gotoNearestHostile.overrideInstancedExpiryInterval = this.overrideInstancedExpiryInterval;
            return (ThinkNode)gotoNearestHostile;
        }

        protected override Job TryGiveJob(Pawn pawn)
		{
            var num = float.MaxValue;
            var targetA = (Thing)null;
            var potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);

            // Get extra targets (like mechanoids)
            var extraTargets = GetTargets(pawn);
            var newTargets = extraTargets.Where(x => !potentialTargetsFor.Contains(x)).ToList();
            potentialTargetsFor.AddRange(newTargets);

            foreach (var target in potentialTargetsFor)
            {
                if (target.ThreatDisabled(pawn))
                    continue;
                if(!AttackTargetFinder.IsAutoTargetable(target))
                    continue;
                if((humanlikesOnly && target is Pawn pawn1 && !pawn1.RaceProps.Humanlike))
                    continue;
                if((target.Thing is Pawn thing && !thing.IsCombatant() && 
                    (this.ignoreNonCombatants || !GenSight.LineOfSightToThing(pawn.Position, (Thing)thing, pawn.Map))))
                    continue;

                var dest = (Thing)target;
                var squared = dest.Position.DistanceToSquared(pawn.Position);
                if (!((double)squared < (double)num) ||
                    !pawn.CanReach((LocalTargetInfo)dest, PathEndMode.OnCell, Danger.Deadly)) continue;
                num = (float)squared;
                targetA = dest;
            }

            if (targetA == null)
                return null;

            var job = JobMaker.MakeJob(JobDefOf.Goto, (LocalTargetInfo)targetA);
            job.checkOverrideOnExpire = true;
            job.collideWithPawns = true;
            job.expiryInterval = 30;
			return job;
		}

        private List<IAttackTarget> GetTargets(Pawn pawn)
        {
            var targetList = new List<IAttackTarget>();
            var th = (IAttackTargetSearcher)pawn;
            var thing = th.Thing;
            var faction = thing.Faction;
            if (faction == null) 
                return targetList;
            
            targetList.AddRange(
                pawn.Map.attackTargetsCache.TargetsHostileToFaction(faction)
                .Where(attackTarget => thing.HostileTo(attackTarget.Thing)));
            return targetList;
        }
    }
}
