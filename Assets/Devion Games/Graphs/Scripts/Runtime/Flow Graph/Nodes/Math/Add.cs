﻿using UnityEngine;

namespace DevionGames.Graphs
{
    [System.Serializable]
    [NodeStyle("Icons/Add", false, "Math")]
    public class Add : FlowNode
    {
        [Input(false,true)]
        public float a;
        [Input(false,true)]
        public float b;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return GetInputValue("a", a) + GetInputValue("b", b);
        }
    }
}
