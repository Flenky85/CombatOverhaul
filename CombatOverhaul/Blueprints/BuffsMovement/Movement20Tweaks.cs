using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;


namespace CombatOverhaul.Blueprints.BuffsMovement
{
    [AutoRegister]
    internal static class Movement20Tweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                //BuffsGuids.ElementalBodyIAirBuff,
                //BuffsGuids.ElementalBodyIIAirBuff,
                //BuffsGuids.ElementalBodyIIIAirBuff,
                BuffsGuids.ElementalBodyIVAirBuff,
                //BuffsGuids.WildShapeElementalAirHugeBuff,
                //BuffsGuids.WildShapeElementalAirLargeBuff,
                //BuffsGuids.WildShapeElementalAirMediumBuff,
                //BuffsGuids.WildShapeElementalAirSmallBuff,
            };

            foreach (var id in buffs)
            {
                BuffConfigurator.For(id)
                  .EditComponent<BuffMovementSpeed>(c =>
                  {
                      c.Value = 20;
                  })
                  .Configure();
            }
        }
    }
}
