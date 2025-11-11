using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;


namespace CombatOverhaul.Blueprints.BuffsMovement
{
    [AutoRegister]
    internal static class MovementReduce5Tweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                //BuffsGuids.ElementalBodyIEarthBuff,
                //BuffsGuids.ElementalBodyIIEarthBuff,
                //BuffsGuids.ElementalBodyIIIEarthBuff,
                BuffsGuids.ElementalBodyIVEarthBuff,
                //BuffsGuids.WildShapeElementalEarthHugeBuff,
                //BuffsGuids.WildShapeElementalEarthLargeBuff,
                //BuffsGuids.WildShapeElementalEarthMediumlBuff,
                //BuffsGuids.WildShapeElementalEarthSmallBuff,
                //BuffsGuids.ElementalBodyIWaterBuff,
                //BuffsGuids.ElementalBodyIIWaterBuff,
                //BuffsGuids.ElementalBodyIIIWaterBuff,
                BuffsGuids.ElementalBodyIVWaterBuff,
                //BuffsGuids.WildShapeElementalWaterHugeBuff,
                //BuffsGuids.WildShapeElementalWaterLargeBuff,
                //BuffsGuids.WildShapeElementalWaterMediumBuff,
                //BuffsGuids.WildShapeElementalWaterSmallBuff,
                BuffsGuids.BeastShapeIVWyvernBuff,
                BuffsGuids.BeastShapeIVShamblingMoundBuff,
                BuffsGuids.WildShapeIVShamblingMoundBuff,
            };

            foreach (var id in buffs)
            {
                BuffConfigurator.For(id)
                  .EditComponent<BuffMovementSpeed>(c =>
                  {
                      c.Value = -5;
                  })
                  .Configure();
            }
        }
    }
}
