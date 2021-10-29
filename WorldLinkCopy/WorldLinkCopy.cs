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
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;

[assembly: MelonInfo(typeof(WorldLinkCopy), nameof(WorldLinkCopy), "1.0.1", "Behemoth")]
[assembly: MelonGame("VRChat", "VRChat")]

public class WorldLinkCopy : MelonMod {
    private static MelonPreferences_Entry<bool> UseShortLink;

    public override void OnApplicationStart() {
        MelonCoroutines.Start(AttachButton());

        MelonPreferences.CreateCategory(nameof(WorldLinkCopy));
        UseShortLink = MelonPreferences.CreateEntry<bool>(nameof(UseShortLink), nameof(UseShortLink), true);
    }

    private static IEnumerator AttachButton() {
        GameObject world_node = null;
        do {
            yield return new WaitForSeconds(1f);
            world_node = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo");
        } while (world_node == null);

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
        if (UseShortLink.Value) {
            CopyShortLink();
        } else {
            CopyFullLink();
        }
    }

    private static void CopyShortLink() {
        void CopyShortLinkInternal(string shortName) => GUIUtility.systemCopyBuffer = $"https://vrch.at/{shortName}";
        void OnError(string error) { MelonLogger.Error($"Failed to receive short link: {error}"); CopyFullLink(); }

        var instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
        if (instance.shortName != null) {
            CopyShortLinkInternal(instance.shortName);
        } else {
            instance.GetShortName(new Action<string>(CopyShortLinkInternal), new Action<string>(OnError));
        }
    }

    private static void CopyFullLink() {
        var instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
        GUIUtility.systemCopyBuffer = $"https://vrchat.com/home/launch?worldId={instance.worldId}&instanceId={instance.instanceId}";
    }
}
