using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ButtonEx : Button
{
    [SerializeField] private Image[] subGraphics =  new Image[0];
    [SerializeField] private Text[] subTexts =  new Text[0];
    [SerializeField] private TextMeshProUGUI[] subTMPros = new TextMeshProUGUI[0];
    private Dictionary<Object, Color> defaultColors = new Dictionary<Object, Color>();
    
    protected override void Awake()
    {
        base.Awake();

        foreach (Image subGraphic in subGraphics)
            defaultColors.Add(subGraphic, subGraphic.color);
        
        foreach (Text subText in subTexts)
            defaultColors.Add(subText, subText.color);
        
        foreach (TextMeshProUGUI subTMPro in subTMPros)
            defaultColors.Add(subTMPro, subTMPro.color);
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
        if (subGraphics != null && subGraphics.Length > 0)
        {
            ColorBlock colorBlock = colors;
            Color tint;
            
            switch (state)
            {
                case SelectionState.Normal:
                    tint = colorBlock.normalColor;
                    break;
                case SelectionState.Highlighted:
                    tint = colorBlock.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    tint = colorBlock.pressedColor;
                    break;
                case SelectionState.Selected:
                    tint = colorBlock.selectedColor;
                    break;
                case SelectionState.Disabled:
                    tint = colorBlock.disabledColor;
                    break;
                default:
                    tint = Color.magenta; //Invalid state colour
                    break;
            }

            foreach (Image subGraphic in subGraphics)
                subGraphic.color = defaultColors[subGraphic] * tint;
            
            foreach (Text subText in subTexts)
                subText.color = defaultColors[subText] * tint;
        
            foreach (TextMeshProUGUI subTMPro in subTMPros)
                subTMPro.color = defaultColors[subTMPro] * tint;
        }
    }
}
