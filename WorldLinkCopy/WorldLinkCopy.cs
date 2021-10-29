/*
 * Copyright (c) 2021 HookedBehemoth
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms and conditions of the GNU General Public License,
 * version 3, as published by the Free Software Foundation.
 *
 * This program is distributed in the hope it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
 * more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections;
using UnityEngine.UI;
using MelonLoader;
using UnityEngine;
using VRC.UI;

[assembly: MelonInfo(typeof(WorldLinkCopy), nameof(WorldLinkCopy), "1.0.1", "Behemoth")]
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
        copy_button.localPosition = new Vector3(548f, -185f);
        copy_button.GetChild(0).GetComponent<Text>().text = "Copy Link";
        var on_click = new Button.ButtonClickedEvent();
        on_click.AddListener(new Action(CopyWorldLinkToClipboard));
        copy_button.GetComponent<Button>().onClick = on_click;
        copy_button.gameObject.SetActive(true);
    }

    private static void CopyWorldLinkToClipboard() {
        var instance = info_page.field_Public_ApiWorldInstance_0;
        var world = info_page.field_Private_ApiWorld_0;
        GUIUtility.systemCopyBuffer = $"https://vrchat.com/home/launch?worldId={world.id}&instanceId={instance.instanceId}";
    }
}
