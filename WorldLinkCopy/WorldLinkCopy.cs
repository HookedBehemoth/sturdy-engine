using System;
using System.Collections;
using UnityEngine.UI;
using MelonLoader;
using UnityEngine;
using VRC.UI;

[assembly: MelonInfo(typeof(WorldLinkCopy), nameof(WorldLinkCopy), "1.0.0", "Behemoth")]
[assembly: MelonGame("VRChat", "VRChat")]

public class WorldLinkCopy : MelonMod {
    public override void OnApplicationStart() {
        MelonCoroutines.Start(AttachButton());
    }

    private static PageWorldInfo info_page = null;

    private static IEnumerator AttachButton() {
        GameObject world_node = null;
        do {
            yield return new WaitForSeconds(1f);
            world_node = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo");
        } while (world_node == null);

        info_page = world_node.GetComponent<PageWorldInfo>();
        var api_world = info_page.field_Private_ApiWorld_0;

        var report_button = world_node.transform.Find("ReportButton");
        var copy_button = GameObject.Instantiate(report_button, world_node.transform);
        copy_button.name = "CopyLinkButton";
        copy_button.localPosition = new Vector3(570f, -185f);
        copy_button.GetChild(0).GetComponent<Text>().text = "Copy Link";
        var on_click = new Button.ButtonClickedEvent();
        on_click.AddListener(new Action(CopyWorldLinkToClipboard));
        copy_button.GetComponent<Button>().onClick = on_click;
        copy_button.gameObject.SetActive(true);
    }

    private static void CopyWorldLinkToClipboard() {
        var world = info_page.field_Private_ApiWorld_0;
        GUIUtility.systemCopyBuffer = $"https://vrchat.com/home/launch?worldId={world.id}&instanceId={world.instanceId}";
    }
}
