using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : Patterns.Singleton<GameApp>
{
    public bool TileMovementEnabled { get; set; } = false;
    public double SecondsSinceStart { get; set; } = 0;
    public int TotalTilesInCorrectPosition { get; set; } = 0;

    [SerializeField]
    List<string> jigsawImageNames = new List<string>();

    public string GetJigsawImgName(int num)
    {
        string imageName = jigsawImageNames[num];
        return imageName;
    }

}