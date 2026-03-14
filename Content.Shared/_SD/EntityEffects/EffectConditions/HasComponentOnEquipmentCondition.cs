using Content.Shared.EntityConditions;
using Content.Shared.Inventory;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.GameObjects;

namespace Content.Server.EntityConditions;

public sealed partial class HasComponentOnEquipmentCondition : EntityConditionBase<HasComponentOnEquipmentCondition>
{
    [DataField(required: true)]
    public ComponentRegistry Components = default!;

    public override string EntityConditionGuidebookText(IPrototypeManager prototype)
    {
        return "Проверяет наличие компонентов на экипировке";
    }
}

public sealed partial class HasComponentOnEquipmentConditionSystem :
    EntityConditionSystem<InventoryComponent, HasComponentOnEquipmentCondition>
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly IEntityManager _entMan = default!;

    protected override void Condition(
        Entity<InventoryComponent> ent,
        ref EntityConditionEvent<HasComponentOnEquipmentCondition> args)
    {
        var condition = args.Condition;

        if (condition.Components.Count == 0)
        {
            args.Result = condition.Inverted;
            return;
        }

        bool found = false;

        if (_inventory.TryGetContainerSlotEnumerator(ent.Owner, out var enumerator, SlotFlags.WITHOUT_POCKET))
        {
            while (enumerator.NextItem(out var item))
            {
                foreach (var comp in condition.Components)
                {
                    if (_entMan.HasComponent(item, comp.Value.Component.GetType()))
                    {
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
        }

        args.Result = condition.Inverted ? !found : found;
    }
}
