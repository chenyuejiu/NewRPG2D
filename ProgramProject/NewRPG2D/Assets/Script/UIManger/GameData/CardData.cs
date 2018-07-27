﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    [SerializeField]
    private string name;//名字

    [SerializeField]
    private int id;//物品id

    [SerializeField]
    private TeamType teamType; //状态信息

    [SerializeField]
    private int teamPos;

    [SerializeField]
    private bool fighting; //战斗中

    [SerializeField]
    private int level;//等级

    [SerializeField]
    private int quality;//稀有度

    [SerializeField]
    private int exp;//经验

    [SerializeField]
    private EquipData[] equipdata; //装备

    [SerializeField]
    private int health; //生命值

    [SerializeField]
    private int healthGrow; //生命值成长

    [SerializeField]
    private int attack;//攻击

    [SerializeField]
    private int attackGrow;//攻击成长

    [SerializeField]
    private int agile;//敏捷

    [SerializeField]
    private int agileGrow;//敏捷成长

    [SerializeField]
    private int defense;//防御

    [SerializeField]
    private int defenseGrow;//防御成长

    [SerializeField]
    private int goodFeeling;//好感度

    [SerializeField]
    private int stars;//星级

    [SerializeField]
    private ItemType itemType;//物品类型

    [SerializeField]
    private string attribute;//属性

    public string Name
    {
        get
        {
            return name;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public TeamType TeamType
    {
        get
        {
            return teamType;
        }
        set
        {
            teamType = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public int Exp
    {
        get
        {
            return exp;
        }
    }

    public EquipData[] Equipdata
    {
        get
        {
            return equipdata;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
    }

    public int HealthGrow
    {
        get
        {
            return healthGrow;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }
    }

    public int AttackGrow
    {
        get
        {
            return attackGrow;
        }
    }

    public int Agile
    {
        get
        {
            return agile;
        }
    }

    public int AgileGrow
    {
        get
        {
            return agileGrow;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }
    }

    public int DefenseGrow
    {
        get
        {
            return defenseGrow;
        }
    }

    public int GoodFeeling
    {
        get
        {
            return goodFeeling;
        }
    }

    public int Stars
    {
        get
        {
            return stars;
        }
    }

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public string Attribute
    {
        get
        {
            return attribute;
        }
    }

    public ItemType ItemType
    {
        get
        {
            return itemType;
        }
    }

    public bool Fighting
    {
        get
        {
            return fighting;
        }
        set
        {
            bool index = fighting;
            if (index != value)
            {
                fighting = value;
            }
        }
    }

    public int TeamPos
    {
        get
        {
            return teamPos;
        }

        set
        {
            teamPos = value;
        }
    }

    public CardData() { }

    public CardData(int id)
    {
        this.id = id;
    }
    public CardData(CardData data)
    {
        this.agile = data.agile;
        this.agileGrow = data.agileGrow;
        this.attack = data.attack;
        this.attackGrow = data.attackGrow;
        this.attribute = data.attribute;
        this.defense = data.defense;
        this.defenseGrow = data.defenseGrow;
        this.equipdata = data.equipdata;
        this.exp = data.exp;
        this.fighting = data.fighting;
        this.goodFeeling = data.goodFeeling;
        this.health = data.health;
        this.healthGrow = data.healthGrow;
        this.id = data.id;
        this.itemType = data.itemType;
        this.level = data.level;
        this.name = data.name;
        this.quality = data.quality;
        this.stars = data.stars;
        this.teamType = data.teamType;
        this.teamPos = data.teamPos;
    }
    public CardData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
    public CardData(int id, string name, ItemType type)
    {
        this.id = id;
        this.name = name;
        this.itemType = type;
    }
}
