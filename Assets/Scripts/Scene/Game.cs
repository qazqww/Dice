using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : BaseScene
{
    protected override void Register()
    {
        AddChannel(Channel.C1, Scene.Title);
    }
}
