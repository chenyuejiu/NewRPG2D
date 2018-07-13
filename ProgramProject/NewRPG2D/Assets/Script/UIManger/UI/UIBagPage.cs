﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBagPage : TTUIPage
{
    private GameObject bagMenu_1 = null;
    private GameObject bagItem_1 = null;
    private GameObject bagMenu_2 = null;
    private GameObject bagItem_2 = null;
    private GameObject bagItem = null;
    private UIBagItem updateBagItem = null;
    private UIBagItem updateBagRole;

    public Button[] firstMenu;
    public int firstLoad = 2;

    private bool itemSort = true;

    private MenuData data;

    public UIBagPage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIBag";
    }

    public override void Awake(GameObject go)
    {
        //初始化
        AwakeInitialization();
    }

    public override void Refresh()
    {

    }

    private void AwakeInitialization()
    {
        this.bagMenu_1 = transform.Find("Menu_1").gameObject;
        this.bagMenu_2 = transform.Find("Menu_2").gameObject;
        this.bagItem_1 = bagMenu_1.transform.Find("btn_menu_1").gameObject;
        this.bagItem_2 = bagMenu_2.transform.Find("btn_menu_2").gameObject;

        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(CloseBagPage);
        this.gameObject.transform.Find("btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);

        bagMenu_2.SetActive(false);
        bagItem_1.SetActive(false);

        updateBagItem = transform.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();

        TTUIPage.ShowPage<UIBagItemMessage>();
        TTUIPage.ClosePage("UIBagItemMessage");

        //一级菜单创建
        CreateMenu(0, bagItem_1, bagMenu_1.transform);

        firstMenu = bagMenu_1.GetComponentsInChildren<Button>();

        bagMenu_1.transform.GetChild(firstLoad).GetComponent<Button>().onClick.Invoke();
    }

    /// <summary>
    /// 菜单创建
    /// </summary>
    /// <param name="parentName"></param>
    /// <param name="obj"></param>
    /// <param name="objParent"></param>
    public void CreateMenu(int ParentNumber, GameObject obj, Transform objParent)
    {
        for (int i = 0; i < BagMenuData.Instance.menu.Count; i++)
        {
            if (BagMenuData.Instance.GetMenu(ParentNumber, i) != null)
            {
                if (objParent.childCount - 1 > i)
                {
                    objParent.GetChild(i + 1).gameObject.SetActive(true);
                    objParent.GetChild(i + 1).name = BagMenuData.Instance.GetMenu(ParentNumber, i).ParentNumber.ToString();
                    objParent.GetChild(i + 1).GetComponentInChildren<Text>().text = BagMenuData.Instance.GetMenu(ParentNumber, i).Name;
                    objParent.GetChild(i + 1).GetComponent<Button>().onClick.AddListener(OnClickMenu);
                }
                else
                {
                    GameObject bagItem = GameObject.Instantiate(obj, objParent) as GameObject;
                    bagItem.SetActive(true);
                    bagItem.name = BagMenuData.Instance.GetMenu(ParentNumber, i).ParentNumber.ToString();
                    bagItem.GetComponentInChildren<Text>().text = BagMenuData.Instance.GetMenu(ParentNumber, i).Name;
                    bagItem.GetComponent<Button>().onClick.AddListener(OnClickMenu);
                }
            }
            else
            {
                return;
            }
        }
    }
    /// <summary>
    /// 菜单点击事件
    /// </summary>
    public void OnClickMenu()
    {

        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name != "btn_Pack")
        {
            string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
            int index = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            data = BagMenuData.Instance.GetMenu(name, index);
        }
        else
        {
            data = BagMenuData.Instance.GetMenu(0, firstLoad - 1);
        }

        if (data.ParentNumber == 0)
        {
            updateBagItem.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            for (int i = 0; i < firstMenu.Length; i++)
            {
                firstMenu[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                firstMenu[i].GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UITexture/BagMenuTexture/" + data.ParentNumber + "/0");
                firstMenu[i].GetComponent<Button>().interactable = true;
            }
            GameObject go;
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name != "btn_Pack")
            {
                go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            }
            else
            {
                go = bagMenu_1.transform.GetChild(firstLoad).gameObject;
            }
            go.GetComponentInChildren<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.down * 18.0f;
            go.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UITexture/BagMenuTexture/" + data.ParentNumber + "/1");
            go.GetComponent<Button>().interactable = false;


            bagMenu_2.SetActive(true);
            bagItem_2.SetActive(false);
            for (int i = 0; i < bagMenu_2.transform.childCount; i++)
            {
                bagMenu_2.transform.GetChild(i).gameObject.SetActive(false);
            }
            CreateMenu(data.Id + 1, bagItem_2, bagMenu_2.transform);

            //通知读取当前点击的菜单信息
            LoadItemEvene(data);
        }
        else
        {
            //底层选项触发排序
            if (data.ParentNumber >= 10)
            {
                ItemSortEvent(data, ItemType.Role, EventSystem.current.currentSelectedGameObject.transform.parent.parent.transform);
            }
            else
                ItemSortEvent(data);
        }
    }

    /// <summary>
    /// 排序事件
    /// </summary>
    public void ItemSortEvent()
    {
        itemSort = !itemSort;
        if (data.ParentNumber >= 10)
        {
            return;
        }
        else
            ItemSortEvent(data);
    }
    public void ItemSortEvent(Transform tr)
    {
        itemSort = !itemSort;
        ItemSortEvent(data, ItemType.Role, tr);
    }
    public void ItemSortEvent(MenuData data)
    {
        switch (data.MenuType)
        {
            case MenuType.Nothing:
                break;
            case MenuType.stars:
                if (itemSort)
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.stars, y.stars));
                else
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.stars, y.stars));
                break;
            case MenuType.hatchingTime:
                if (itemSort)
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.hatchingTime, y.hatchingTime));
                else
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.hatchingTime, y.hatchingTime));
                break;
            case MenuType.quality:
                updateBagItem.UpdateProp();
                if (itemSort)
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.quality, y.quality));
                else
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.quality, y.quality));
                break;
            case MenuType.isUse:
                updateBagItem.UpdateProp(PropType.OnlyUse, PropType.AllOn);
                if (itemSort)
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.propType, y.propType));
                else
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.propType, y.propType));
                break;
            case MenuType.equipType:
                updateBagItem.UpdateEquip((EquipType)data.Id + 1);
                if (itemSort)
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.quality, y.quality));
                else
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.quality, y.quality));
                break;
            default:
                break;
        }

        //排序后刷新格子内容
        for (int i = 0; i < updateBagItem.grids.Count; i++)
        {
            updateBagItem.grids[i].transform.SetSiblingIndex(i + 1);
        }
    }
    public void ItemSortEvent(MenuData data, ItemType type, Transform go)
    {
        updateBagRole = go.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();
        switch (data.Id)
        {
            case 0:
                if (itemSort)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.level, y.level));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.level, y.level));
                break;
            case 1:
                if (itemSort)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.stars, y.stars));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.stars, y.stars));
                break;
            case 2:
                if (itemSort)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.grow, y.grow));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.grow, y.grow));
                break;
            case 3:
                if (itemSort)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.goodFeeling, y.goodFeeling));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.goodFeeling, y.goodFeeling));
                break;
            default:
                break;
        }
        //排序后刷新格子内容
        for (int i = 0; i < updateBagRole.grids.Count; i++)
        {
            updateBagRole.grids[i].transform.SetSiblingIndex(i + 1);
        }
    }

    /// <summary>
    /// 加载该选项背包物品
    /// </summary>
    public void LoadItemEvene(MenuData data)
    {
        ItemType itemType = (ItemType)data.Id + 1;
        switch (itemType)
        {
            case ItemType.Egg:
                //背包加载蛋
                updateBagItem.itemType = itemType;
                updateBagItem.UpdateEggs();
                itemSort = true;
                break;
            case ItemType.Prop:
                //背包加载道具
                updateBagItem.itemType = itemType;
                updateBagItem.UpdateProp();
                itemSort = true;
                break;
            case ItemType.Equip:
                //背包加载装备
                updateBagItem.itemType = itemType;
                updateBagItem.UpdateEquip();
                itemSort = true;
                break;
            default:
                break;
        }
    }

    public void CloseBagPage()
    {
        TTUIPage.ClosePage("UIBagItemMessage");
        TTUIPage.ClosePage("UIBagPage");
    }

    public class BagGridConparer : IComparer<int>
    {
        //倒序
        public int Compare(int x, int y)
        {
            if (x == 0) return 1;
            if (y == 0) return -1;


            if (x > y) return -1;
            if (x < y) return 1;

            return 0;
        }
        //顺序
        public int Compare1(int x, int y)
        {
            if (x == 0) return 1;
            if (y == 0) return -1;


            if (x > y) return 1;
            if (x < y) return -1;

            return 0;
        }
    }


}
