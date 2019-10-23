using System.Collections.Generic;
using System.Windows.Forms;

namespace RetroarchKeyboardOverlayEditor
{
    class Overlay
    {
        public enum ButtonName { UP, DOWN, LEFT, RIGHT, A, B, C, D, E, X, Y, Z }
        Dictionary<ButtonName, string> buttonText;
        Dictionary<ButtonName, Button> assignedKey;
        Dictionary<ButtonName, ListViewItem> assignedOverlay;
        bool eightWay;
        public bool Is8Way => eightWay;

        public Overlay(bool eightWay)
        {
            buttonText = new Dictionary<ButtonName, string>();
            assignedKey = new Dictionary<ButtonName, Button>();
            assignedOverlay = new Dictionary<ButtonName, ListViewItem>();
            this.eightWay = eightWay;
        }

        public void SetEightWay()
        {
            eightWay = true;
        }

        public void SetButton(ButtonName name, Button key, ListViewItem overlay)
        {
            assignedKey[name] = key;
            assignedOverlay[name] = overlay;
        }

        public void SetButton(ButtonName name, string text, Button key, ListViewItem overlay)
        {
            buttonText[name] = text;
            assignedKey[name] = key;
            assignedOverlay[name] = overlay;
        }

        public void ChangeText(ButtonName name, string text)
        {
            buttonText[name] = text;
        }

        public string GetText(ButtonName name)
        {
            return buttonText[name];
        }

        public Button GetAssignedKey(ButtonName name)
        {
            if (assignedKey.ContainsKey(name))
                return assignedKey[name];
            return null;
        }

        public void ChangeAssignedOverlay(ButtonName name, ListViewItem overlay)
        {
            assignedOverlay[name] = overlay;
        }

        public ListViewItem GetAssignedOverlay(ButtonName name)
        {
            if (assignedOverlay.ContainsKey(name))
                return assignedOverlay[name];
            return null;
        }

        public bool NeedArrows()
        {
            return (assignedKey[ButtonName.LEFT] != null || assignedKey[ButtonName.RIGHT] != null || assignedKey[ButtonName.DOWN] != null || assignedKey[ButtonName.UP] != null);
        }

        public bool NeedTwoWayArrows()
        {
            return assignedKey[ButtonName.DOWN] == null && assignedKey[ButtonName.UP] == null;
        }

        public int NumberOfDescs()
        {
            int num = 0;
            if (assignedKey[ButtonName.UP] != null || assignedOverlay[ButtonName.UP] != null) num++;
            if (assignedKey[ButtonName.RIGHT] != null || assignedOverlay[ButtonName.RIGHT] != null) num++;
            if (assignedKey[ButtonName.DOWN] != null || assignedOverlay[ButtonName.DOWN] != null) num++;
            if (assignedKey[ButtonName.LEFT] != null || assignedOverlay[ButtonName.LEFT] != null) num++;
            if (assignedKey[ButtonName.A] != null || assignedOverlay[ButtonName.A] != null) num++;
            if (assignedKey[ButtonName.B] != null || assignedOverlay[ButtonName.B] != null) num++;
            if (assignedKey[ButtonName.C] != null || assignedOverlay[ButtonName.C] != null) num++;
            if (assignedKey[ButtonName.D] != null || assignedOverlay[ButtonName.D] != null) num++;
            if (assignedKey[ButtonName.E] != null || assignedOverlay[ButtonName.E] != null) num++;
            if (assignedKey[ButtonName.X] != null || assignedOverlay[ButtonName.X] != null) num++;
            if (assignedKey[ButtonName.Y] != null || assignedOverlay[ButtonName.Y] != null) num++;
            if (assignedKey[ButtonName.Z] != null || assignedOverlay[ButtonName.Z] != null) num++;
            if (NeedArrows()) num++;
            return num;
        }
    }
}
