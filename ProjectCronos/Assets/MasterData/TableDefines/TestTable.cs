using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterMemory;
using MessagePack;

[MemoryTable("m_test"), MessagePackObject(true)]
public class Test
{
    [PrimaryKey] 
    public int Id { get; set; }
    public string Name { get; set; }
    public int Hp { get; set; }
    public float Attack { get; set; }

    public Test(int Id, string Name, int hp, float attack)
    {
        this.Id = Id;
        this.Name = Name;
        this.Hp = hp;
        this.Attack = attack;
    }
}