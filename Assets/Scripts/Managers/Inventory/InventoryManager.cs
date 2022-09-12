using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("GameObject for displaying of item in the UI")]
    [SerializeField] private GameObject itemPrefab;
    [Tooltip("GameObject for displaying of quest in the UI")]
    [SerializeField] private GameObject questPrefab;
    [Header("GameOjects")]
    [Tooltip("GameObject of the InventoryMenu")]
    [SerializeField] private GameObject inventoryMenu;
    [Tooltip("GameObject of the Scroll View")]
    [SerializeField] private GameObject scrollView;
    [Tooltip("GameObject of the ItemContent")]
    [SerializeField] private GameObject contentObject;
    [Tooltip("GameObject of the ItemDetail")]
    [SerializeField] private GameObject itemDetail;
    [Tooltip("GameObject of ItemCamera")]
    [SerializeField] private GameObject itemCamera;
    [Tooltip("GameObject of the QuestMenu")]
    [SerializeField] private GameObject questMenu;
    [Tooltip("GameObject of the QuestContent")]
    [SerializeField] private GameObject questContentObject;

    private QuestManager questManager;
    private List<GameObject> itemHolders;
    private Dictionary<string, Item> items;
    private List<string> itemsHad;

    private Item selectedItem;
    private GameObject selectedObject;

    private MenuBaseState currentState;
    private MenuStateFactory states;
    private Controls controls;
    private Vector2 menuNavInput;
    private bool isInventoryMenuInput;
    private Vector2 itemLookInput;
    private bool isSelectInput;
    private bool isBackInput;
    private Vector2 mouseItemLookInput;
    private bool isMouseLeftClickInput;
    private bool isNextTabInput;

    private bool isFinished;

    public GameObject QuestPrefab { get => questPrefab; }

    public GameObject InventoryMenu { get => inventoryMenu; }
    public GameObject QuestMenu { get => questMenu; }
    public GameObject QuestContentObject { get => questContentObject; }

    public QuestManager QuestManager { get => questManager; }

    public MenuBaseState CurrentState { get => currentState; set => currentState = value; }

    public Vector2 MenuNavInput { get => menuNavInput; }
    public bool IsInventoryMenuInput { get => isInventoryMenuInput; set => isInventoryMenuInput = value; }
    public Vector2 ItemLookInput { get => itemLookInput; }
    public bool IsSelectInput { get => isSelectInput; set => isSelectInput = value; }
    public bool IsBackInput { get => isBackInput; set => isBackInput = value; }
    public Vector2 MouseItemLookInput { get => mouseItemLookInput; }
    public bool IsMouseLeftClickInput { get => isMouseLeftClickInput; }
    public bool IsNextTabInput { get => isNextTabInput; set => isNextTabInput = value; }

    public bool IsFinished { get => isFinished; set => isFinished = value; }
    public List<GameObject> ItemHolders { get => itemHolders; }
    public GameObject SelectedObject { get => selectedObject; }
    void Awake()
    {
        controls = new Controls();

        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();

        itemHolders = new List<GameObject>();
        items = new Dictionary<string, Item>();
        itemsHad = new List<string>();
        isFinished = false;

        states = new MenuStateFactory(this);
        currentState = states.MenuIdleState();
        currentState.EnterState();

        controls.Menu.MenuNavigate.started += OnMenuNavInput;
        controls.Menu.MenuNavigate.canceled += OnMenuNavInput;
        controls.Menu.MenuNavigate.performed += OnMenuNavInput;
        controls.Menu.InventoryMenu.started += OnInventoryMenuButton;
        controls.Menu.InventoryMenu.canceled += OnInventoryMenuButton;
        controls.Menu.ItemLook.started += OnItemLookInput;
        controls.Menu.ItemLook.canceled += OnItemLookInput;
        controls.Menu.ItemLook.performed += OnItemLookInput;
        controls.Menu.Select.started += OnSelectButton;
        controls.Menu.Select.canceled += OnSelectButton;
        controls.Menu.Back.started += OnBackButton;
        controls.Menu.Back.canceled += OnBackButton;
        controls.Menu.MouseItemLook.started += OnMouseItemLookInput;
        controls.Menu.MouseItemLook.canceled += OnMouseItemLookInput;
        controls.Menu.MouseItemLook.performed += OnMouseItemLookInput;
        controls.Menu.LeftClick.started += OnLeftClickButton;
        controls.Menu.LeftClick.canceled += OnLeftClickButton;
        controls.Menu.NextTab.started += OnNextTabButton;
        controls.Menu.NextTab.canceled += OnNextTabButton;
    }

    public void DisplayItems()
    {
        ToggleInventoryMenu();
        isFinished = false;
        List<string> names = new List<string>();
        foreach (var item in itemHolders)
        {
            names.Add(item.name);
        }

        foreach (KeyValuePair<string, Item> item in items)
        {
            if (!names.Contains(item.Key))
            {
                GameObject itemHolder = Instantiate(itemPrefab);
                itemHolder.transform.SetParent(contentObject.transform, false);
                itemHolder.transform.Find("Image").GetComponent<Image>().sprite = item.Value.Sprite;
                itemHolder.name = item.Key;
                itemHolders.Add(itemHolder);
            }
        }
        currentState = states.MenuInventoryGridState();
        currentState.EnterState();
    }

    public void ToggleInventoryMenu()
    {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
    }

    public void ToggleScrollView()
    {
        scrollView.SetActive(!scrollView.activeSelf);
    }

    public void ToggleItemDetail()
    {
        itemDetail.SetActive(!itemDetail.activeSelf);
        itemCamera.SetActive(!itemCamera.activeSelf);
    }

    public void DisplayItemDetail()
    {
        TextMeshProUGUI description = itemDetail.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        string descriptionText = "name: " + selectedItem.Name;
        descriptionText += "\n \n \n";
        descriptionText += "Description:\n" + selectedItem.Description;
        description.text = descriptionText;
    }

    public void AddItem(GameObject itemObject, Clue clue)
    {
        items.Add(clue.ObjectName, new Item(clue.ObjectName, clue.Description, itemObject, clue.Sprite));
        itemsHad.Add(clue.ObjectName);
        Transform itemLocation = itemCamera.transform.Find("ItemLocation");
        itemObject.transform.SetPositionAndRotation(itemLocation.position, itemLocation.rotation);
        itemObject.transform.SetParent(itemLocation);
        ItemInfo itemInfo = itemObject.GetComponent<ItemInfo>();
        itemObject.transform.localPosition = new Vector3(0, 0, itemInfo.zoomLevel);
        itemObject.transform.localRotation = Quaternion.Euler(itemInfo.localRotation);
        itemObject.SetActive(false);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void Select(int selection)
    {
        List<string> keys = new List<string>(items.Keys);
        selectedItem = items[keys[selection]];
        selectedObject = selectedItem.ItemObject;
    }

    public bool InPossesion(string itemName)
    {
        return items.ContainsKey(itemName);
    }

    public void DestroyItem(string itemName)
    {
        if (!items.ContainsKey(itemName)) return;

        Destroy(items[itemName].ItemObject);
        items.Remove(itemName);

        for (int i = 0; i < itemHolders.Count; i++)
        {
            if (itemHolders[i].name == itemName)
            {
                Destroy(itemHolders[i]);
                itemHolders.RemoveAt(i);
                return;
            }
        }
    }

    public bool Exist(string itemName)
    {
        return itemsHad.Contains(itemName);
    }

    public InventorySave GetItemList()
    {
        InventorySave inventorySave = new InventorySave();
        List<ItemSave> itemSaves = new List<ItemSave>();

        foreach (KeyValuePair<string, Item> item in items)
        {
            itemSaves.Add(new ItemSave(item.Value.Name, item.Value.Description));
        }

        inventorySave.itemsHave = itemSaves.ToArray();
        inventorySave.itemsHad = itemsHad.ToArray();

        return inventorySave;
    }

    public void LoadItems(InventorySave saveFile)
    {
        items = new Dictionary<string, Item>();
        GameObject itemLocationObject = itemCamera.transform.Find("ItemLocation").gameObject;
        int childCount = itemLocationObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(itemLocationObject.transform.GetChild(i).gameObject);
        }

        foreach (ItemSave item in saveFile.itemsHave)
        {
            GameObject itemObject = Instantiate(Resources.Load<GameObject>("ItemObject/" + item.itemName));
            itemObject.name = item.itemName;
            Destroy(itemObject.GetComponent<Clue>());
            Transform itemLocation = itemLocationObject.transform;
            itemObject.transform.SetPositionAndRotation(itemLocation.position, itemLocation.rotation);
            itemObject.transform.SetParent(itemLocation);
            ItemInfo itemInfo = itemObject.GetComponent<ItemInfo>();
            itemObject.transform.localPosition = new Vector3(0, 0, itemInfo.zoomLevel);
            itemObject.transform.localRotation = Quaternion.Euler(itemInfo.localRotation);
            itemObject.SetActive(false);

            Item newItem = new Item(
                item.itemName,
                item.itemDescription,
                itemObject,
                Resources.Load<Sprite>("Sprite/" + item.itemName));
            items.Add(item.itemName, newItem);
        }

        itemsHad = new List<string>(saveFile.itemsHad);
    }

    public void AddNewItem((string, string) item)
    {
        print("Hello");
        GameObject itemObject = Instantiate(Resources.Load<GameObject>("ItemObject/" + item.Item1));
        itemObject.name = item.Item1;
        Transform itemLocation = itemCamera.transform.Find("ItemLocation");
        itemObject.transform.SetPositionAndRotation(itemLocation.position, itemLocation.rotation);
        itemObject.transform.SetParent(itemLocation);
        itemObject.SetActive(false);

        Item newItem = new Item(
            item.Item1,
            item.Item2,
            itemObject,
            Resources.Load<Sprite>("Sprite/" + item.Item1));
        items.Add(item.Item1, newItem);
        itemsHad.Add(item.Item1);
    }

    #region Controls
    private void OnMenuNavInput(InputAction.CallbackContext context)
    {
        menuNavInput = context.ReadValue<Vector2>();
    }

    private void OnInventoryMenuButton(InputAction.CallbackContext context)
    {
        isInventoryMenuInput = context.ReadValueAsButton();
    }

    private void OnItemLookInput(InputAction.CallbackContext context)
    {
        itemLookInput = context.ReadValue<Vector2>();
    }

    private void OnSelectButton(InputAction.CallbackContext context)
    {
        isSelectInput = context.ReadValueAsButton();
    }

    private void OnBackButton(InputAction.CallbackContext context)
    {
        isBackInput = context.ReadValueAsButton();
    }

    private void OnMouseItemLookInput(InputAction.CallbackContext context)
    {
        mouseItemLookInput = context.ReadValue<Vector2>();
    }

    private void OnLeftClickButton(InputAction.CallbackContext context)
    {
        isMouseLeftClickInput = context.ReadValueAsButton();
    }

    private void OnNextTabButton(InputAction.CallbackContext context)
    {
        isNextTabInput = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        controls.Menu.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Disable();
    }

    #endregion
}
