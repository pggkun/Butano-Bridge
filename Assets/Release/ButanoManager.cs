using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;

public class ButanoManager : MonoBehaviour
{
    [SerializeField] private ButanoSettings settings;
    public ButanoSettings Settings => settings;

    const string header = ""+
        "#ifndef BN_REGULAR_BG_ITEMS_INFO_H\n"+
        "#define BN_REGULAR_BG_ITEMS_INFO_H\n\n"+

        "#include \"bn_regular_bg_item.h\"\n";

    const string body = ""+
        "namespace bn::regular_bg_items_info\n"+
        "{\n" +
        "    constexpr inline pair<regular_bg_item, string_view> array[] = {\n";

    const string footer = ""+
        "    };\n\n"+
        "    constexpr inline span<const pair<regular_bg_item, string_view>> span(array);\n"+
        "}\n\n"+
        "#endif";

    public void ExportAll()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        List<string> tilemapNames = new List<string>();
        foreach(var so in sceneObjects)
        {
            if(so.GetComponent<TilemapToBitmap>() != null)
            {
                string origName = so.name.Replace("(", "").Replace(")","").Replace(" ", "");
                string resName = string.Concat(
                         origName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString().ToLower())
                     );
                tilemapNames.Add(resName); 
                so.GetComponent<TilemapToBitmap>().ExportTilemap();
            }
        }
        string bgItems = header;
        foreach(var tname in tilemapNames)
        {
            bgItems += "#include \"bn_regular_bg_items_"+tname+".h\"\n";
        }
        bgItems += body;
        foreach(var tname in tilemapNames)
        {
            bgItems += "        make_pair(bn::regular_bg_items::"+tname+", string_view(\""+tname+"\")),\n";
        }
        bgItems += footer;
        string jsonName = Path.Combine(Settings.ProjectIncludeFolder, "regular_bg_items_info.h");
        using (StreamWriter sw = File.CreateText(jsonName))
        {
            sw.WriteLine(bgItems);
        }	
    }
}
