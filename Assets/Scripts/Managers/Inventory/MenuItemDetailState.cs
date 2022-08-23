using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemDetailState : MenuBaseState
{
    public MenuItemDetailState(InventoryManager inventoryManager, MenuStateFactory menuStateFactory)
    : base(inventoryManager, menuStateFactory)
    { }

    private GameObject itemGameObject;
    private Quaternion defaultRotation;
    public override void EnterState()
    {
        ctx.ToggleItemDetail();
        ctx.DisplayItemDetail();
        itemGameObject = ctx.SelectedObject;
        itemGameObject.SetActive(true);
        defaultRotation = itemGameObject.transform.rotation;
    }
    public override void UpdateState()
    {
        if (ctx.IsMouseLeftClickInput)
        {
            if (ctx.MouseItemLookInput.magnitude > 0.1f)
            {
                float multiplier = -10.0f;
                Vector2 input = new Vector2(ctx.MouseItemLookInput.x * multiplier, ctx.MouseItemLookInput.y * multiplier);
                RotateObject(input);
            }
        }
        else
        {
            RotateObject(ctx.ItemLookInput);
        }

        CheckSwitchState();
    }
    public override void ExitState()
    {
        itemGameObject.transform.rotation = defaultRotation;
        itemGameObject.SetActive(false);
        ctx.ToggleItemDetail();
    }
    public override void CheckSwitchState()
    {

        if (ctx.IsBackInput)
        {
            ctx.IsBackInput = false;
            SwitchState(factory.MenuInventoryGridState());
        }
    }

    private void RotateObject(Vector2 input)
    {
        if (input.magnitude < 0.1f) return;

        itemGameObject.transform.Rotate(input.y, 0.0f, input.x, Space.World);
    }
}
