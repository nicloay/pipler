using UnityEngine;
using System.Collections;
using Slash.Unity.DataBind.Core.Data;

public class GameData : Context {
    public static GameData Instance;
    public GameData(){
        Instance = this;
    }

    readonly Property<int> pointProperty = new Property<int>();

    public int Point
    {
        get { return this.pointProperty.Value; }
        set { this.pointProperty.Value = value; }
    }
}
