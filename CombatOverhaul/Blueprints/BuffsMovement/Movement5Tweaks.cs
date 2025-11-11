using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;

namespace CombatOverhaul.Blueprints.BuffsMovement
{
    [AutoRegister]
    internal static class Movement5Tweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                BuffsGuids.FormOfTheDragonIBlackBuff,
                BuffsGuids.FormOfTheDragonIBlueBuff,
                BuffsGuids.FormOfTheDragonIBrassBuff,
                BuffsGuids.FormOfTheDragonIBronzeBuff,
                BuffsGuids.FormOfTheDragonICopperBuff,
                BuffsGuids.FormOfTheDragonIGoldBuff,
                BuffsGuids.FormOfTheDragonIGreenBuff,
                BuffsGuids.FormOfTheDragonIRedBuff,
                BuffsGuids.FormOfTheDragonISilverBuff,
                BuffsGuids.FormOfTheDragonIWhiteBuff,
                BuffsGuids.FormOfTheDragonIIBlackBuff,
                BuffsGuids.FormOfTheDragonIIBlueBuff,
                BuffsGuids.FormOfTheDragonIIBrassBuff,
                BuffsGuids.FormOfTheDragonIIBronzeBuff,
                BuffsGuids.FormOfTheDragonIICopperBuff,
                BuffsGuids.FormOfTheDragonIIGoldBuff,
                BuffsGuids.FormOfTheDragonIIGreenBuff,
                BuffsGuids.FormOfTheDragonIIRedBuff,
                BuffsGuids.FormOfTheDragonIISilverBuff,
                BuffsGuids.FormOfTheDragonIIWhiteBuff,
                BuffsGuids.FormOfTheDragonIIIBlackBuff,
                BuffsGuids.FormOfTheDragonIIIBlueBuff,
                BuffsGuids.FormOfTheDragonIIIBrassBuff,
                BuffsGuids.FormOfTheDragonIIIBronzeBuff,
                BuffsGuids.FormOfTheDragonIIICopperBuff,
                BuffsGuids.FormOfTheDragonIIIGoldBuff,
                BuffsGuids.FormOfTheDragonIIIGreenBuff,
                BuffsGuids.FormOfTheDragonIIIRedBuff,
                BuffsGuids.FormOfTheDragonIIISilverBuff,
                BuffsGuids.FormOfTheDragonIIIWhiteBuff,
                BuffsGuids.WildShapeIIISmilodonBuff,
                BuffsGuids.WildShapeIVBearBuff,
                BuffsGuids.BeastShapeIVSmilodonBuff,
            };

            foreach (var id in buffs)
            {
                BuffConfigurator.For(id)
                  .EditComponent<BuffMovementSpeed>(c =>
                  {
                      c.Value = 5;
                  })
                  .Configure();
            }
        }
    }
}
