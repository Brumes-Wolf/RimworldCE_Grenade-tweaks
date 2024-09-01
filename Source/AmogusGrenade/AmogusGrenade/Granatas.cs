using CombatExtended;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AmogusGrenade
{
    public class GizmoForNades : CombatExtended.CompRangedGizmoGiver
    {
        float overheadSpread = 120;
        float overheadGravity = 10;
        public override void Notify_Equipped(Pawn pawn)
        {
            VerbPropertiesCE compEquip1 = this.parent.TryGetComp<CompEquippable>().PrimaryVerb.verbProps as VerbPropertiesCE;
            VerbPropertiesCE compEquip2 = compEquip1.MemberwiseClone() as VerbPropertiesCE;
            compEquip2.range = compEquip1.range * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);
            //Log.Message((compEquip1.range * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation)).ToString() + "before");
            this.parent.TryGetComp<CompEquippable>().PrimaryVerb.verbProps = compEquip2;
            //Log.Message(this.parent.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.range.ToString());
            base.Notify_Equipped(pawn);
        }
        public override void Notify_UsedWeapon(Pawn pawn)
        {
            VerbPropertiesCE compEquip1 = this.parent.TryGetComp<CompEquippable>().PrimaryVerb.verbProps as VerbPropertiesCE;
            VerbPropertiesCE compEquip2 = compEquip1.MemberwiseClone() as VerbPropertiesCE;
            compEquip2.range = compEquip1.range * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);
            //Log.Message((compEquip1.range * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation)).ToString() + "before");
            this.parent.TryGetComp<CompEquippable>().PrimaryVerb.verbProps = compEquip2;
            //Log.Message(this.parent.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.range.ToString());
            base.Notify_UsedWeapon(pawn);
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            //bool isPlayerControlled = (bool)((this.parent.TryGetComp<CompEquippable>().parent.ParentHolder as Pawn_InventoryTracker)?.pawn?.IsColonistPlayerControlled);

            //if (isPlayerControlled) 
            //{
                Verb_ShootCEOneUse ammouser = this.parent.TryGetComp<CompEquippable>().PrimaryVerb as Verb_ShootCEOneUse;
                ProjectilePropertiesCE projectilePropsCE = ammouser.verbProps.defaultProjectile.projectile as ProjectilePropertiesCE;

                yield return new Command_Action
                {
                    defaultDesc = "",
                    icon = ContentFinder<Texture2D>.Get(checkIcon(projectilePropsCE.flyOverhead)),
                    defaultLabel = checkLabel(projectilePropsCE.flyOverhead),
                    action = delegate
                    {
                        bool toggledOnce = false;
                        if (projectilePropsCE.spreadMult == overheadSpread)
                        {
                            toggledOnce = true;
                        }

                    //float smth = projectilePropsCE.speed;
                    bool overheadMode = projectilePropsCE.flyOverhead;
                        if (overheadMode && toggledOnce)
                        {
                            projectilePropsCE.gravityFactor /= overheadGravity;
                            projectilePropsCE.spreadMult /= overheadSpread;
                            projectilePropsCE.flyOverhead = false;
                            //Log.Message("2. OffOverhead Gravity set to:" + projectilePropsCE.gravityFactor.ToString() + ". Spread is: " + projectilePropsCE.spreadMult.ToString() + "Overhead is " + overheadMode.ToString() + " Once: " + toggledOnce.ToString() + overheadSpread.ToString());
                        }
                        else
                        {
                            projectilePropsCE.gravityFactor = overheadGravity;
                            projectilePropsCE.spreadMult = overheadSpread;
                            projectilePropsCE.flyOverhead = true;
                            //Log.Message("3. OnOverhead Gravity set to:" + projectilePropsCE.gravityFactor.ToString() + ". Spread is: " + projectilePropsCE.spreadMult.ToString() + "Overhead is " + overheadMode.ToString() + " Once: " + toggledOnce.ToString() + overheadSpread.ToString());
                        }
                    }
                };
            //}
        }
        public string checkLabel(bool overheadMode)
        {
            if (overheadMode)
            {
                return "Throw over walls";
            }
            else
            {
                return "Throw normally";
            }
        }

        public string checkIcon(bool overheadMode)
        {
            if (overheadMode)
            {
                return "UI/Gizmos/Gizmo_ThrowOverWall";
            }
            else
            {
                return "UI/Gizmos/Gizmo_ThrowNormally";
            }
        }
    }


}
