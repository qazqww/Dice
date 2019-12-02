using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : BaseScene
{
    protected override void Register()
    {
        AddChannel(Channel.C1, Scene.Help);
        AddChannel(Channel.C2, Scene.Game);
    }
}
