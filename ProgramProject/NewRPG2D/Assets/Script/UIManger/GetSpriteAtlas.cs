﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System;

[ExecuteInEditMode]
public class GetSpriteAtlas : MonoBehaviour
{
    public static GetSpriteAtlas insatnce = null;

    [SerializeField]
    private SpriteAtlas Icons;
    [SerializeField]
    private SpriteAtlas RoomIcons;
    [SerializeField]
    private SpriteAtlas EnemyIcons;
    [SerializeField]
    private CaptureScreen manCamera;

    private void Awake()
    {
        insatnce = this;
    }

    public Sprite GetRoomSp(string name)
    {
        Sprite sp = RoomIcons.GetSprite(name);
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }
    public Sprite GetIcon(string name)
    {
        Sprite sp = Icons.GetSprite(name);
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }
    public Sprite GetQuality(string name)
    {
        Sprite sp = Icons.GetSprite("Quality_" + name);
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }

    public Sprite GetRoleIcon(string name)
    {
        Sprite sp = null;

        return sp;
    }

    public Sprite GetEnemyIcon(string name)
    {
        Sprite sp = EnemyIcons.GetSprite(name);
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }

    public Sprite GetSkilIcon(string name)
    {
        Sprite sp = Icons.GetSprite("技能1");
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }
    public Sprite GetWorkIcon(string name)
    {
        Sprite sp = Icons.GetSprite("Work_" + name);
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }
    public Sprite GetWotkType(string name, int type)
    {
        Sprite sp = Icons.GetSprite("WorkType_" + name + type);
        if (sp != null)
        {
            return sp;
        }
        Debug.LogError("没有找到图片: " + name);
        return null;
    }

    public Sprite GetRoleIcon(HallRoleData data, bool ChangeSkill = false)
    {
        Debug.Log("性别: " + data.sexType);
        Sprite sp = manCamera.CaptureScreenToIcon(data.id, data.sexType, data.Equip[(int)EquipTypeEnum.Armor], ChangeSkill);
        return sp;
    }

    public void SetImage(Image[] images)
    {
        StartCoroutine(SetSprite(images, Icons));
    }

    public void SetImage(Image[] images, SpriteAtlas sprites)
    {
        StartCoroutine(SetSprite(images, sprites, Icons));
    }

    public Sprite GetLevelIconToAtr(RoleAttribute atr)
    {
        string st = "";
        switch (atr)
        {
            case RoleAttribute.Fight:
                st = "Fight";
                break;
            case RoleAttribute.Gold:
                st = "Mint";
                break;
            case RoleAttribute.Food:
                st = "Kitchen";
                break;
            case RoleAttribute.Mana:
                st = "Laboratory";
                break;
            case RoleAttribute.ManaSpeed:
                st = "Crafting";
                break;
            case RoleAttribute.Wood:
                st = "Crafting";
                break;
            case RoleAttribute.Iron:
                st = "Foundry";
                break;
            default:
                break;
        }
        return GetIcon(st);
    }
    /// <summary>
    /// 获取房间内角色对应ICon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetLevelIconToRoom(BuildRoomName name)
    {
        string st = "";
        switch (name)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                st = "Mint";
                break;
            case BuildRoomName.Food:
                st = "Kitchen";
                break;
            case BuildRoomName.Mana:
                st = "Laboratory";
                break;
            case BuildRoomName.Wood:
                st = "Crafting";
                break;
            case BuildRoomName.Iron:
                st = "Foundry";
                break;
            case BuildRoomName.FighterRoom:
                st = "Fight";
                break;
            case BuildRoomName.Mint:
                st = "Mint";
                break;
            case BuildRoomName.Kitchen:
                st = "Kitchen";
                break;
            case BuildRoomName.Laboratory:
                st = "Laboratory";
                break;
            case BuildRoomName.Crafting:
                st = "Crafting";
                break;
            case BuildRoomName.Foundry:
                st = "Foundry";
                break;
            case BuildRoomName.Barracks:
                st = "Fight";
                break;
            default:
                st = "Laboratory";
                break;
        }
        return GetIcon(st);
    }

    IEnumerator SetSprite(Image[] images, SpriteAtlas currencyImage)
    {
        yield return images;

        foreach (var item in images)
        {
            var sprite = currencyImage.GetSprite(item.name);
            if (sprite != null)
            {
                item.sprite = sprite;
            }
        }
    }

    IEnumerator SetSprite(Image[] images, SpriteAtlas currentImages, SpriteAtlas currencyImage)
    {
        yield return images;

        foreach (var item in images)
        {
            var sprite = currencyImage.GetSprite(item.name);
            var sprite_2 = currentImages.GetSprite(item.name);
            if (sprite != null)
            {
                item.sprite = sprite;
                Debug.Log("1");
            }
            else if (sprite_2 != null)
            {
                item.sprite = sprite_2;
                Debug.Log("2");
            }
        }
    }
}
