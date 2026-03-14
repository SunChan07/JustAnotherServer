using Content.Shared.Damage.Systems;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.GameObjects;
using Robust.Shared.Localization;
using Robust.Shared.IoC;
using System;

namespace Content.Server.Chemistry;

[UsedImplicitly]
public sealed partial class TakeStaminaDamage : EntityEffect
{
    [DataField]
    public int Amount = 10;

    [DataField]
    public bool Immediate;

    public override void RaiseEvent(EntityUid target, IEntityEffectRaiser raiser, float scale, EntityUid? user)
    {
        if (scale != 1f)
            return;

#pragma warning disable CS0618
        var entMan = IoCManager.Resolve<IEntityManager>();
#pragma warning restore CS0618

        entMan.System<SharedStaminaSystem>()
            .TakeStaminaDamage(target, Amount, visual: false);
    }
}
