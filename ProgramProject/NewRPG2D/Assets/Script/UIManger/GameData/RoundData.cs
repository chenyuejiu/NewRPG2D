﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string spriteName;
    [SerializeField]
    private bool unLock;
    [SerializeField]
    private LessonData[] lessonData;

    public int Id
    {
        get
        {
            return id;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public bool UnLock
    {
        get
        {
            return unLock;
        }

        set
        {
            unLock = value;
        }
    }

    public LessonData[] LessonData
    {
        get
        {
            return lessonData;
        }

        set
        {
            lessonData = value;
        }
    }

    public RoundData() { }
}

[System.Serializable]
public class LessonData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string spriteName;
    [SerializeField]
    private bool unLock;
    [SerializeField]
    private DifficultyType difficultyType;//难度
    [SerializeField]
    private int needFatigue;
    [SerializeField]
    private LessonDropData[] lessonDrop;

    public int Id
    {
        get
        {
            return id;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public bool UnLock
    {
        get
        {
            return unLock;
        }

        set
        {
            unLock = value;
        }
    }

    public DifficultyType DifficultyType
    {
        get
        {
            return difficultyType;
        }

        set
        {
            difficultyType = value;
        }
    }

    public int NeedFatigue
    {
        get
        {
            return needFatigue;
        }
    }

    public LessonDropData[] LessonDrop
    {
        get
        {
            return lessonDrop;
        }
    }
}
[System.Serializable]
public class LessonDropData
{
    [SerializeField]
    private int[] dropPropId;//掉落物品的ID
    [SerializeField]
    private int dropBoxId;//掉落包的ID

    public int[] DropPropId
    {
        get
        {
            return dropPropId;
        }
    }

    public int DropBoxId
    {
        get
        {
            return dropBoxId;
        }
    }
}

