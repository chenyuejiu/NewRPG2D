﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ExploreData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string spriteId;
    [SerializeField]
    private int needTime;
    [SerializeField]
    private int needFatigue;
    [SerializeField]
    private int captainLevel;
    [SerializeField]
    private int otherLevel;
    [SerializeField]
    private int progressLimit;//进度上限
    [SerializeField]
    private int droppingBoxId;//掉落包ID
    [SerializeField]
    private int[] droppingId;//掉落ID

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string SpriteId
    {
        get
        {
            return spriteId;
        }
    }

    public int NeedTime
    {
        get
        {
            return needTime;
        }
    }

    public int CaptainLevel
    {
        get
        {
            return captainLevel;
        }
    }

    public int OtherLevel
    {
        get
        {
            return otherLevel;
        }
    }

    public int NeedFatigue
    {
        get
        {
            return needFatigue;
        }
    }

    public int ProgressLimit
    {
        get
        {
            return progressLimit;
        }
    }

    public int DroppingBoxId
    {
        get
        {
            return droppingBoxId;
        }
    }

    public int[] DroppingId
    {
        get
        {
            return droppingId;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }
}
