using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInventoryGridState : MenuBaseState
{
    public MenuInventoryGridState(InventoryManager inventoryManager, MenuStateFactory menuStateFactory)
    : base(inventoryManager, menuStateFactory)
    { }

    private const float MAXNAVCOOLDOWN = 0.3f;
    private int selection;
    private float navCooldown;

    public override void EnterState()
    {
        ctx.BackTitle.text = "[I] Back to Game";
        ctx.IsInventoryMenuInput = false;
        ctx.ToggleScrollView();
        selection = 0;
        navCooldown = 0;
        ToggleSelectionborder(selection);
        ctx.TitleHeader.SetActive(true);
    }
    public override void UpdateState()
    {
        if (navCooldown <= 0)
        {
            MoveSelection();
        }
        else 
        {
            navCooldown -= Time.deltaTime;
        }
        
        CheckSwitchState();
    }
    public override void ExitState()
    {
        if (selection < ctx.ItemHolders.Count)
        {
            ctx.ItemHolders[selection].transform.Find("SelectionBorder").gameObject.SetActive(false);
        }
        ctx.ToggleScrollView();
    }
    public override void CheckSwitchState()
    {
        if (ctx.IsSelectInput && selection < ctx.ItemHolders.Count)
        {
            ctx.IsSelectInput = false;
            ctx.Select(selection);
            SwitchState(factory.MenuItemDetailState());
            return;
        }

        if (ctx.IsInventoryMenuInput)
        {
            ctx.IsInventoryMenuInput = false;
            SwitchState(factory.MenuIdleState());
            return;
        }

        if (ctx.IsNextTabInput)
        {
            ctx.IsNextTabInput = false;
            SwitchState(factory.MenuQuestState());
            return;
        }
    }

    private void MoveSelection()
    {
        if (ctx.MenuNavInput.magnitude < 0.1f) return;

        int newSelection = ctx.MenuNavInput.x > 0 ? 1 : -1;

        if (Mathf.Abs(ctx.MenuNavInput.y) > Mathf.Abs(ctx.MenuNavInput.x))
        {
            newSelection = 1 * ctx.MenuNavInput.y > 0 ? 5 : -5;
        }

        newSelection += selection;
        ToggleSelectionborder(newSelection);
        navCooldown = MAXNAVCOOLDOWN;
    }

    private void ToggleSelectionborder(int newSelection)
    {
        if (newSelection >= ctx.ItemHolders.Count || newSelection < 0) return;

        ctx.ItemHolders[selection].transform.Find("SelectionBorder").gameObject.SetActive(false);
        selection = newSelection;
        ctx.ItemHolders[selection].transform.Find("SelectionBorder").gameObject.SetActive(true);
    }
}