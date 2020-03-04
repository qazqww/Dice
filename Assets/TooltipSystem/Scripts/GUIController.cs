using UnityEngine;
using System.Collections.Generic;

public class GUIController : MonoBehaviour {

    ToolTipHandler tooltip_handler;
    List<Icon> icons = new List<Icon>();
    bool Generated = false;
    bool IconPriority_GUI = false;
    bool IconPriority_Transform = false;

    Transform previouslyHovered = null;

    public Texture2D IconBase;
    public Texture2D TooltipBGTexture;
    public Font TooltipFont;
    public bool TooltipMouseFollow = false;
    public int[] TooltipFontSizes = new int[3];
    public Color[] colors;

    void Start()
    {
        tooltip_handler = new ToolTipHandler(TooltipBGTexture, TooltipFont, TooltipFontSizes, colors);
        
        // Create Initial Icons here
        Icon item_potion = new Icon(IconType.Skill, IconBase, null, 
            new Rect(379, 674, 150, 150), "Potion");

        Icon item_concentrate = new Icon(IconType.Skill, IconBase, null, 
            new Rect(579, 674, 150, 150), "+1 Spot on a Dice");

        Icon item_dicePlus = new Icon(IconType.Skill, IconBase, null, 
            new Rect(779, 674, 150, 150), "+1 Dice");

        Icon item_kickEnemy = new Icon(IconType.Skill, IconBase, null, 
            new Rect(979, 674, 150, 150), "Enemy Back");

        Icon state_injured = new Icon(IconType.Item, IconBase, null,
            new Rect(972, 527, 100, 100), "Injured");

        //(Texture2D)Resources.Load("Images/16-Cripple")

        // Only need one Icon with IconType.GameObject
        Icon objecthandler_tooltip = new Icon(IconType.GameObject, null, new Rect(), "");

        // Add icons to the icons list
        icons.Add(item_potion);
        icons.Add(item_concentrate);
        icons.Add(item_dicePlus);
        icons.Add(item_kickEnemy);
        icons.Add(state_injured);
        icons.Add(objecthandler_tooltip);

        ShowItemIcon(true);
        ShowSkillIcon(false);
    }

    public void ShowItemIcon(bool value)
    {
        for(int i=0; i<icons.Count; i++)
        {
            if (icons[i].getIconType() == IconType.Skill)
                icons[i].UseIcon = value;
        }
    }

    public void ShowSkillIcon(bool value)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if (icons[i].getIconType() == IconType.Item)
                icons[i].UseIcon = value;
        }
    }

    void OnGUI()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        foreach (Icon icon in icons)
        {
            icon.DrawIcon();

            // active mouse hover
            if (icon.Name != "" && (icon.getIconType() != IconType.GameObject && icon.ActiveHover()))
            {
                if (!icon.UseIcon)
                    return;

                // if hovering over a GameObject and now hovering over a GUI Icon
                if (IconPriority_Transform && !IconPriority_GUI)
                {
                    IconPriority_GUI = true;
                    IconPriority_Transform = false;
                    previouslyHovered = null;
                    Generated = false;
                }

                if (!Generated)
                {
                    tooltip_handler.CreateNewTooltip(icon.getIconDimensions(), icon.getIconType(), icon.ToString());
                    IconPriority_GUI = true;
                    Generated = true;
                }

                if (icon.ToString() != tooltip_handler.ActiveTooltip())
                {
                    Generated = false;
                }

                tooltip_handler.RenderTooltipGUI();
            }
            else if (icon.getIconType() == IconType.GameObject && icon.ActiveHover(ray))
            {
                if (!IconPriority_GUI && icon.getTargetData().transform.name.Contains("Object"))
                {
                    ObjectHandler obj_handler = icon.getTargetData().transform.GetComponent<ObjectHandler>();
                    icon.Name = obj_handler.Object_Name;

                    if (!Generated)
                    {
                        if (!TooltipMouseFollow)
                            tooltip_handler.CreateNewTooltip(new Rect(GetComponent<Camera>().WorldToScreenPoint(icon.getTargetData().transform.position + (icon.getTargetData().transform.localScale / 2)).x, Screen.height - GetComponent<Camera>().WorldToScreenPoint(icon.getTargetData().transform.position).y, icon.getIconDimensions().width, icon.getIconDimensions().height), icon.getIconType(), icon.ToString());
                        else
                            tooltip_handler.CreateNewTooltip(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, icon.getIconDimensions().width, icon.getIconDimensions().height), icon.getIconType(), icon.ToString());

                        previouslyHovered = obj_handler.transform;

                        IconPriority_Transform = true;
                        Generated = true;
                    }
                    else if (Generated && (previouslyHovered == null || !previouslyHovered.Equals(obj_handler.transform)))
                        Generated = false;
                    else if (TooltipMouseFollow && previouslyHovered.Equals(obj_handler.transform))
                    {
                        tooltip_handler.setActiveDetailsDimensionPosition(15 + Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    }


                    tooltip_handler.RenderTooltipGUI();
                }
            }

            if (icon.ToString() == tooltip_handler.ActiveTooltip())
            {
                if (!icon.ActiveHover())
                    IconPriority_GUI = false;
            }
        }
    }
}
