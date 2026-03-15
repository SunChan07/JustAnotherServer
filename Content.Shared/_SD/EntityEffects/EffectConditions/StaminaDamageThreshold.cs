using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.EntityConditions;
using Content.Shared.EntityConditions.Conditions;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.GameObjects;
using Robust.Shared.Localization;

namespace Content.Server.EntityConditions;

public sealed partial class StaminaDamageThreshold : EntityConditionBase<StaminaDamageThreshold>
{
    [DataField]
    public float Max = float.PositiveInfinity;

    [DataField]
    public float Min = -1;

    public override string EntityConditionGuidebookText(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-stamina-damage-threshold",
            ("max", float.IsPositiveInfinity(Max) ? (float)int.MaxValue : Max),
            ("min", Min));
    }
}

public sealed partial class StaminaDamageThresholdSystem :
    EntityConditionSystem<StaminaComponent, StaminaDamageThreshold>
{
    [Dependency] private readonly SharedStaminaSystem _stamina = default!;
    [Dependency] private readonly IEntityManager _entMan = default!;

    protected override void Condition(
        Entity<StaminaComponent> ent,
        ref EntityConditionEvent<StaminaDamageThreshold> args)
    {
        var condition = args.Condition;

        var total = _stamina.GetStaminaDamage(ent.Owner, ent.Comp);

        if (total > condition.Min && total < condition.Max)
        {
            args.Result = !condition.Inverted;
        }
        else
        {
            args.Result = condition.Inverted;
        }
    }
}
