using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;

namespace CombatOverhaul.Blueprints.BuffsMovement
{
    [AutoRegister]
    internal static class Movement15Tweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                //BuffsGuids.ElementalBodyIFireBuff,
                //BuffsGuids.ElementalBodyIIFireBuff,
                //BuffsGuids.ElementalBodyIIIFireBuff,
                BuffsGuids.ElementalBodyIVFireBuff,
                //BuffsGuids.WildShapeElementalFireHugeBuff,
                //BuffsGuids.WildShapeElementalFireMediumBuff,
                //BuffsGuids.WildShapeElementalFireLargeBuff,
                //BuffsGuids.WildShapeElementalFireSmallBuff,
                //BuffsGuids.WildShapeIWolfBuff,


            };

            foreach (var id in buffs)
            {
                BuffConfigurator.For(id)
                  .EditComponent<BuffMovementSpeed>(c =>
                  {
                      c.Value = 15;
                  })
                  .Configure();
            }
        }
    }
}
