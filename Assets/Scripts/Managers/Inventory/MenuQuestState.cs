using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuQuestState : MenuBaseState
{
    public MenuQuestState(InventoryManager inventoryManager, MenuStateFactory menuStateFactory)
    : base(inventoryManager, menuStateFactory)
    { }

    private List<(string, string)> questList;
    private List<GameObject> quests;
    public override void EnterState()
    {
        questList = new List<(string, string)>();
        quests = new List<GameObject>();
        ctx.ToggleInventoryMenu();
        ctx.QuestMenu.SetActive(true);

        questList = ctx.QuestManager.GetBasicQuestList();

        foreach ((string, string) item in questList)
        {
            GameObject newOption = GameObject.Instantiate(ctx.QuestPrefab);
            newOption.GetComponent<RectTransform>().localScale = Vector3.one;
            newOption.GetComponent<QuestUI>().UpdateText(item.Item1, "need a descriptor", item.Item2);
            newOption.transform.SetParent(ctx.QuestContentObject.transform);
            quests.Add(newOption);
        }
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        foreach (GameObject item in quests)
        {
            GameObject.Destroy(item);
        }
        ctx.QuestMenu.SetActive(false);
    }
    public override void CheckSwitchState()
    {
        if (ctx.IsNextTabInput)
        {
            ctx.IsNextTabInput = false;
            SwitchState(factory.MenuInventoryGridState());
            ctx.ToggleInventoryMenu();
            return;
        }

        if (ctx.IsBackInput)
        {
            ctx.IsBackInput = false;
            SwitchState(factory.MenuIdleState());
            return;
        }
    }
}
