using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RetroarchKeyboardOverlayEditor
{
    public partial class FormMain : Form
    {
        Dictionary<string, Overlay> overlays;
        Dictionary<Button, Button> buttons;
        Dictionary<Button, ListViewItem> assignedOverlays;
        Dictionary<Button, string> defaultTexts;
        Button selectedButton;
        bool indexReallyChanged;
        string previousOverlay;

        public FormMain()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            overlays = new Dictionary<string, Overlay>();
            buttons = new Dictionary<Button, Button>();
            assignedOverlays = new Dictionary<Button, ListViewItem>();
            indexReallyChanged = true;
            Reset();

            defaultTexts = new Dictionary<Button, string>();
            defaultTexts[buttonA] = "A";
            defaultTexts[buttonB] = "B";
            defaultTexts[buttonC] = "X";
            defaultTexts[buttonD] = "Y";
            defaultTexts[buttonE] = "¤";
            defaultTexts[buttonX] = "<:";
            defaultTexts[buttonY] = ">:";
            defaultTexts[buttonZ] = "=";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (panelEdit.BackColor == Color.Red)
                panelEdit.BackColor = Color.White;
            else
                panelEdit.BackColor = Color.Red;
            foreach (ListViewItem item in listOverlay.Items)
                item.Selected = false;
        }

        private void comboBoxIcons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (indexReallyChanged)
            {
                selectedButton.Text = comboBoxIcons.Text;
                indexReallyChanged = false;
                comboBoxIcons.SelectedIndex = -1;
            }
            else
            {
                indexReallyChanged = true;
            }
        }

        private void labelIcon_Click(object sender, EventArgs e)
        {
            comboBoxIcons.DroppedDown = true;
        }


        // CLICK //


        void DiselectButton()
        {
            if (selectedButton != null && buttons[selectedButton] == null && assignedOverlays[selectedButton] == null && selectedButton.Width >= 32)
                selectedButton.Text = "";
            selectedButton = null;
            panelEdit.Location = new Point(-100, -100);
            comboBoxIcons.Visible = false;
        }

        void SelectButton(Button button)
        {
            comboBoxIcons.Visible = false;
            if (selectedButton != null && buttons[selectedButton] == null && assignedOverlays[selectedButton] == null && selectedButton.Width >= 32)
                selectedButton.Text = "";
            selectedButton = button;
            bool arrow = button.Width < 32;
            panelEdit.Location = new Point(selectedButton.Location.X - 9 - (arrow ? 4 : 0), selectedButton.Location.Y - 9 - (arrow ? 4 : 0));
            if (!arrow)
            {
                comboBoxIcons.Location = new Point(selectedButton.Location.X > 300 ? 620 : 64, selectedButton.Location.Y + 5);
                comboBoxIcons.Visible = true;
                if (button.Text == "")
                    button.Text = defaultTexts[selectedButton];
            }
        }

        private void panelBackgroundTop_Click(object sender, EventArgs e)
        {
            DiselectButton();
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (selectedButton != (Button)sender)
            {
                SelectButton((Button)sender);
                if (buttons[selectedButton] != null)
                    buttons[selectedButton].BackColor = selectedButton.BackColor;
            }
            else
                DiselectButton();
        }

        private void key_Click(object sender, EventArgs e)
        {
            ActiveControl = selectedButton ?? (Control)panelBackgroundTop;
            if (selectedButton == null)
                return;
            if (assignedOverlays[selectedButton] != null)
            {
                assignedOverlays[selectedButton].BackColor = SystemColors.Window;
                assignedOverlays[selectedButton] = null;
            }
            if (((Button)sender).BackColor != SystemColors.Control)
            {
                if (buttons[selectedButton] != null)
                    buttons[selectedButton].BackColor = SystemColors.Control;
                buttons[selectedButton] = null;
                return;
            }
            if (buttons[selectedButton] != null)
                buttons[selectedButton].BackColor = SystemColors.Control;
            buttons[selectedButton] = (Button)sender;
            buttons[selectedButton].BackColor = selectedButton.BackColor;
            DiselectButton();
        }


        void RefreshKeyColor(Button name)
        {
            if (buttons[name] != null)
                buttons[name].BackColor = name.BackColor;
            if (assignedOverlays[name] != null)
                assignedOverlays[name].BackColor = name.BackColor;
        }

        private void listOverlay_MouseDown(object sender, MouseEventArgs e)
        {
            previousOverlay = labelOverlayName.Text;
        }

        Overlay GenerateEmptyOverlay()
        {
            Overlay newOverlay = new Overlay(false);
            newOverlay.SetButton(Overlay.ButtonName.UP, null, null);
            newOverlay.SetButton(Overlay.ButtonName.DOWN, null, null);
            newOverlay.SetButton(Overlay.ButtonName.LEFT, null, null);
            newOverlay.SetButton(Overlay.ButtonName.RIGHT, null, null);
            newOverlay.SetButton(Overlay.ButtonName.A, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.B, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.C, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.D, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.E, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.X, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.Y, "", null, null);
            newOverlay.SetButton(Overlay.ButtonName.Z, "", null, null);
            return newOverlay;
        }

        Overlay GenerateOverlay()
        {
            Overlay newOverlay = new Overlay(checkBox8.Checked);
            newOverlay.SetButton(Overlay.ButtonName.UP, buttons[buttonUp], assignedOverlays[buttonUp]);
            newOverlay.SetButton(Overlay.ButtonName.DOWN, buttons[buttonDown], assignedOverlays[buttonDown]);
            newOverlay.SetButton(Overlay.ButtonName.LEFT, buttons[buttonLeft], assignedOverlays[buttonLeft]);
            newOverlay.SetButton(Overlay.ButtonName.RIGHT, buttons[buttonRight], assignedOverlays[buttonRight]);
            newOverlay.SetButton(Overlay.ButtonName.A, buttonA.Text, buttons[buttonA], assignedOverlays[buttonA]);
            newOverlay.SetButton(Overlay.ButtonName.B, buttonB.Text, buttons[buttonB], assignedOverlays[buttonB]);
            newOverlay.SetButton(Overlay.ButtonName.C, buttonC.Text, buttons[buttonC], assignedOverlays[buttonC]);
            newOverlay.SetButton(Overlay.ButtonName.D, buttonD.Text, buttons[buttonD], assignedOverlays[buttonD]);
            newOverlay.SetButton(Overlay.ButtonName.E, buttonE.Text, buttons[buttonE], assignedOverlays[buttonE]);
            newOverlay.SetButton(Overlay.ButtonName.X, buttonX.Text, buttons[buttonX], assignedOverlays[buttonX]);
            newOverlay.SetButton(Overlay.ButtonName.Y, buttonY.Text, buttons[buttonY], assignedOverlays[buttonY]);
            newOverlay.SetButton(Overlay.ButtonName.Z, buttonZ.Text, buttons[buttonZ], assignedOverlays[buttonZ]);
            return newOverlay;
        }

        void ApplyOverlay(Overlay overlay)
        {
            checkBox8.Checked = overlay.Is8Way;
            buttons[buttonUp] = overlay.GetAssignedKey(Overlay.ButtonName.UP);
            buttons[buttonDown] = overlay.GetAssignedKey(Overlay.ButtonName.DOWN);
            buttons[buttonLeft] = overlay.GetAssignedKey(Overlay.ButtonName.LEFT);
            buttons[buttonRight] = overlay.GetAssignedKey(Overlay.ButtonName.RIGHT);
            buttonA.Text = overlay.GetText(Overlay.ButtonName.A);
            buttons[buttonA] = overlay.GetAssignedKey(Overlay.ButtonName.A);
            assignedOverlays[buttonA] = overlay.GetAssignedOverlay(Overlay.ButtonName.A);
            buttonB.Text = overlay.GetText(Overlay.ButtonName.B);
            buttons[buttonB] = overlay.GetAssignedKey(Overlay.ButtonName.B);
            assignedOverlays[buttonB] = overlay.GetAssignedOverlay(Overlay.ButtonName.B);
            buttonC.Text = overlay.GetText(Overlay.ButtonName.C);
            buttons[buttonC] = overlay.GetAssignedKey(Overlay.ButtonName.C);
            assignedOverlays[buttonC] = overlay.GetAssignedOverlay(Overlay.ButtonName.C);
            buttonD.Text = overlay.GetText(Overlay.ButtonName.D);
            buttons[buttonD] = overlay.GetAssignedKey(Overlay.ButtonName.D);
            assignedOverlays[buttonD] = overlay.GetAssignedOverlay(Overlay.ButtonName.D);
            buttonE.Text = overlay.GetText(Overlay.ButtonName.E);
            buttons[buttonE] = overlay.GetAssignedKey(Overlay.ButtonName.E);
            assignedOverlays[buttonE] = overlay.GetAssignedOverlay(Overlay.ButtonName.E);
            buttonX.Text = overlay.GetText(Overlay.ButtonName.X);
            buttons[buttonX] = overlay.GetAssignedKey(Overlay.ButtonName.X);
            assignedOverlays[buttonX] = overlay.GetAssignedOverlay(Overlay.ButtonName.X);
            buttonY.Text = overlay.GetText(Overlay.ButtonName.Y);
            buttons[buttonY] = overlay.GetAssignedKey(Overlay.ButtonName.Y);
            assignedOverlays[buttonY] = overlay.GetAssignedOverlay(Overlay.ButtonName.Y);
            buttonZ.Text = overlay.GetText(Overlay.ButtonName.Z);
            buttons[buttonZ] = overlay.GetAssignedKey(Overlay.ButtonName.Z);
            assignedOverlays[buttonZ] = overlay.GetAssignedOverlay(Overlay.ButtonName.Z);
            RefreshKeyColor(buttonUp);
            RefreshKeyColor(buttonDown);
            RefreshKeyColor(buttonLeft);
            RefreshKeyColor(buttonRight);
            RefreshKeyColor(buttonA);
            RefreshKeyColor(buttonB);
            RefreshKeyColor(buttonC);
            RefreshKeyColor(buttonD);
            RefreshKeyColor(buttonE);
            RefreshKeyColor(buttonX);
            RefreshKeyColor(buttonY);
            RefreshKeyColor(buttonZ);
        }

        private void listOverlay_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listOverlay.Items)
                item.Focused = false;

            if (selectedButton != null)
            {
                foreach (ListViewItem item in listOverlay.Items)
                    if (item.Selected)
                    {
                        if (buttons[selectedButton] != null)
                            buttons[selectedButton].BackColor = SystemColors.Control;
                        buttons[selectedButton] = null;
                        if (item.BackColor != SystemColors.Window)
                        {
                            if (item.BackColor == selectedButton.BackColor)
                            {
                                item.BackColor = SystemColors.Window;
                                assignedOverlays[selectedButton] = null;
                            }
                            return;
                        }
                        if (assignedOverlays[selectedButton] != null)
                            assignedOverlays[selectedButton].BackColor = SystemColors.Window;
                        assignedOverlays[selectedButton] = item;
                        assignedOverlays[selectedButton].BackColor = selectedButton.BackColor;
                        DiselectButton();
                        foreach (ListViewItem items in listOverlay.Items)
                            if (items.Text == previousOverlay)
                                labelOverlayName.Text = previousOverlay;
                        break;
                    }
                return;
            }

            if (labelOverlayName.Text != "")
                overlays[labelOverlayName.Text] = GenerateOverlay();

            foreach (ListViewItem item in listOverlay.Items)
                if (item.Selected)
                {
                    labelOverlayName.Text = item.Text;
                    if (overlays.ContainsKey(labelOverlayName.Text))
                    {
                        Reset();
                        ApplyOverlay(overlays[labelOverlayName.Text]);
                    }
                }
        }


        // RESET //


        void ResetOverlays()
        {
            overlays.Clear();
            listOverlay.Clear();
            labelOverlayName.Text = "";
        }

        void Reset()
        {
            if (selectedButton != null && buttons[selectedButton] != null)
                buttons[selectedButton].BackColor = SystemColors.Control;
            foreach (Control control in Controls)
                if (control is Button && control.Tag != null)
                    control.BackColor = SystemColors.Control;
            checkBox8.Checked = false;
            DiselectButton();

            buttons[buttonDown] = null;
            buttons[buttonUp] = null;
            buttons[buttonLeft] = null;
            buttons[buttonRight] = null;
            buttons[buttonA] = null;
            buttons[buttonB] = null;
            buttons[buttonC] = null;
            buttons[buttonD] = null;
            buttons[buttonE] = null;
            buttons[buttonX] = null;
            buttons[buttonY] = null;
            buttons[buttonZ] = null;

            assignedOverlays[buttonDown] = null;
            assignedOverlays[buttonUp] = null;
            assignedOverlays[buttonLeft] = null;
            assignedOverlays[buttonRight] = null;
            assignedOverlays[buttonA] = null;
            assignedOverlays[buttonB] = null;
            assignedOverlays[buttonC] = null;
            assignedOverlays[buttonD] = null;
            assignedOverlays[buttonE] = null;
            assignedOverlays[buttonX] = null;
            assignedOverlays[buttonY] = null;
            assignedOverlays[buttonZ] = null;

            buttonA.Text = "";
            buttonB.Text = "";
            buttonC.Text = "";
            buttonD.Text = "";
            buttonE.Text = "";
            buttonX.Text = "";
            buttonY.Text = "";
            buttonZ.Text = "";

            foreach (ListViewItem item in listOverlay.Items)
                item.BackColor = SystemColors.Window;
        }

        private void menuItemReset_Click(object sender, EventArgs e)
        {
            ResetOverlays();
            Reset();
        }


        // LOAD //

        Button FindButtonByTag(string tag)
        {
            if (tag.Contains("overlay_next"))
                return null;
            foreach (Control control in Controls)
                if (control is Button && control.Tag != null && control.Tag.ToString() == tag)
                    return (Button)control;
            return null;
        }

        ListViewItem FindOverlayByName(string name)
        {
            foreach (ListViewItem item in listOverlay.Items)
                if (item.Text == name)
                    return item;
            return null;
        }

        string IconToButton(string name)
        {
            switch (name)
            {
                case "a": return "A";
                case "b": return "B";
                case "x": return "X";
                case "y": return "Y";
                case "ok": return "OK";
                case "menu": return "=";
                case "retroarch": return "¤";
                case "switchleft": return "<:";
                case "switchright": return ">:";
            }
            return null;
        }

        void ReadButton(Overlay.ButtonName buttonName, ref Overlay.ButtonName? buttonThatNeedsText, ref Overlay.ButtonName? buttonThatNeedsTarget, string button, Overlay currentOverlay)
        {
            buttonThatNeedsText = buttonName;
            if (button.Contains("overlay_next"))
                buttonThatNeedsTarget = buttonName;
            currentOverlay.SetButton(buttonName, FindButtonByTag(button), null);
        }

        Overlay.ButtonName WhichButtonOnRight(string y)
        {
            switch (y)
            {
                case "0.1": return Overlay.ButtonName.E;
                case "0.3": return Overlay.ButtonName.D;
                case "0.5": return Overlay.ButtonName.C;
                case "0.7": return Overlay.ButtonName.B;
            }
            return Overlay.ButtonName.A; //"0.9"
        }

        Overlay.ButtonName WhichButtonOnLeft(string y)
        {
            switch (y)
            {
                case "0.1": return Overlay.ButtonName.Z;
                case "0.3": return Overlay.ButtonName.Y;
            }
            return Overlay.ButtonName.X; //"0.5"
        }

        Overlay.ButtonName WhichButtonArrow8(string xyw)
        {
            switch (xyw)
            {
                case "0.185,0.8,0.05": return Overlay.ButtonName.RIGHT;
                case "0.112,0.67,0.12": return Overlay.ButtonName.UP;
                case "0.112,0.93,0.12": return Overlay.ButtonName.DOWN;
            }
            return Overlay.ButtonName.LEFT; //"0.039,0.8,0.05"
        }

        Overlay.ButtonName WhichButtonArrow(string xy)
        {
            switch (xy)
            {
                case "0.185,0.8": case "0.185,0.93": return Overlay.ButtonName.RIGHT;
                case "0.112,0.67": return Overlay.ButtonName.UP;
                case "0.112,0.93": return Overlay.ButtonName.DOWN;
            }
            return Overlay.ButtonName.LEFT; //"0.039,0.8" "0.039,0.93"
        }

        void ReadButtonArrow(Overlay.ButtonName buttonName, ref Overlay.ButtonName? buttonThatNeedsTarget, string button, Overlay currentOverlay)
        {
            if (button.Contains("overlay_next"))
                buttonThatNeedsTarget = buttonName;
            currentOverlay.SetButton(buttonName, FindButtonByTag(button), null);
        }

        private void menuItemLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Overlay config file (*.cfg)|*.cfg|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ResetOverlays();
                Reset();
                Overlay.ButtonName? buttonThatNeedsText = null;
                Overlay.ButtonName? buttonThatNeedsTarget = null;
                string[] lines = File.ReadAllLines(dialog.FileName);
                int numberOfOverlays = Convert.ToInt32(lines[0].Split(' ')[2]);
                foreach (string line in lines)
                {
                    for (int o = 0; o < numberOfOverlays; o++)
                        if (line.Contains("overlay" + o + "_name"))
                        {
                            ListViewItem newItem = new ListViewItem(line.Split('"')[1]);
                            listOverlay.Items.Add(newItem);
                            overlays[newItem.Text] = GenerateEmptyOverlay();
                        }
                }

                Overlay namelessOverlay = null;
                if (listOverlay.Items.Count == 0)
                    namelessOverlay = GenerateEmptyOverlay();

                foreach (string line in lines)
                {
                    for (int o = 0; o < numberOfOverlays; o++)
                    {
                        if (!line.Contains("descs") && line.Contains("overlay" + o + "_desc"))
                        {
                            Overlay currentOverlay = namelessOverlay ?? overlays[listOverlay.Items[o].Text];
                            if (line.Contains("_overlay"))
                            {
                                if (buttonThatNeedsText != null)
                                {
                                    currentOverlay.ChangeText((Overlay.ButtonName)buttonThatNeedsText, IconToButton(line.Split(' ')[2].Split('.')[0]));
                                    buttonThatNeedsText = null;
                                }
                            }
                            else
                            {
                                if (line.Contains("_next_target"))
                                {
                                    if (buttonThatNeedsTarget != null)
                                    {
                                        currentOverlay.ChangeAssignedOverlay((Overlay.ButtonName)buttonThatNeedsTarget, FindOverlayByName(line.Split('"')[1]));
                                        buttonThatNeedsTarget = null;
                                    }
                                }
                                else
                                {
                                    string desc = line.ToLower().Split('"')[1];
                                    string button = desc.Split(',')[0];
                                    if (button.Contains("nul"))
                                        continue;
                                    if (button.Contains("retrok"))
                                        button = button.Replace("retrok_", "");
                                    else
                                        button = "!!!" + button;
                                    string[] descSplit = desc.Split(',');
                                    string x = descSplit[1];
                                    string y = descSplit[2];
                                    string w = descSplit[4];
                                    string h = descSplit[5];
                                    if (x == "0.94") //Right
                                        ReadButton(WhichButtonOnRight(y), ref buttonThatNeedsText, ref buttonThatNeedsTarget, button, currentOverlay);
                                    else if (x == "0.05") //Left
                                        ReadButton(WhichButtonOnLeft(y), ref buttonThatNeedsText, ref buttonThatNeedsTarget, button, currentOverlay);
                                    else //Arrow
                                    {
                                        if (w == "0.12" || h == "0.22")
                                        {
                                            ReadButtonArrow(WhichButtonArrow8(x + "," + y + "," + w), ref buttonThatNeedsTarget, button, currentOverlay);
                                            currentOverlay.SetEightWay();
                                        }
                                        else
                                            ReadButtonArrow(WhichButtonArrow(x + "," + y), ref buttonThatNeedsTarget, button, currentOverlay);
                                    }
                                }
                            }
                        }
                    }
                }
                if (namelessOverlay != null)
                    ApplyOverlay(namelessOverlay);
                else
                    listOverlay.Items[0].Selected = true;
            }

        }


        // SAVE //


        string ButtonToIcon(string name)
        {
            switch (name)
            {
                case "A": return "a";
                case "B": return "b";
                case "X": return "x";
                case "Y": return "y";
                case "OK": return "ok";
                case "=": return "menu";
                case "¤": return "retroarch";
                case "<:": return "switchleft";
                case ">:": return "switchright";
            }
            return null;
        }

        string KeyName(Button key)
        {
            string name = key.Tag.ToString();
            if (name.Contains("!!!"))
                return name.Replace("!!!", "");
            else
                return "retrok_" + name;
        }

        void AddArrowButton(List<string> lines, int o, Overlay overlay, Overlay.ButtonName buttonName, ref int i, string x, string y, string w = "05", string h = "0875")
        {
            string command = "";
            if (overlay.GetAssignedKey(buttonName) != null)
                command = KeyName(overlay.GetAssignedKey(buttonName));
            else if (overlay.GetAssignedOverlay(buttonName) != null)
                command = "overlay_next";
            if (command != "")
            {
                lines.Add("overlay" + o + "_desc" + i + " = \"" + command + ",0." + x + ",0." + y + ",radial,0." + w + ",0." + h + "\"");
                if (command == "overlay_next")
                    lines.Add("overlay" + o + "_desc" + i + "_next_target = \"" + overlay.GetAssignedOverlay(buttonName).Text + "\"");
                i++;
            }
        }

        void AddLeftOrRightButton(List<string> lines, int o, Overlay overlay, Overlay.ButtonName buttonName, ref int i, int j, bool right)
        {
            string command = "";
            if (overlay.GetAssignedKey(buttonName) != null)
                command = KeyName(overlay.GetAssignedKey(buttonName));
            else if (overlay.GetAssignedOverlay(buttonName) != null)
                command = "overlay_next";
            if (command != "")
            {
                lines.Add("overlay" + o + "_desc" + i + " = \"" + command + ",0." + (right ? "94" : "05") + ",0." + (2 * j + 1) + ",rect,0.056,0.096\"");
                lines.Add("overlay" + o + "_desc" + i + "_overlay = " + ButtonToIcon(overlay.GetText(buttonName)) + ".png");
                if (command == "overlay_next")
                    lines.Add("overlay" + o + "_desc" + i + "_next_target = \"" + overlay.GetAssignedOverlay(buttonName).Text + "\"");
                i++;
            }
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Overlay config file (*.cfg)|*.cfg";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (labelOverlayName.Text != "")
                    overlays[labelOverlayName.Text] = GenerateOverlay();

                List<string> lines = new List<string>();
                int numberOfOverlays = listOverlay.Items.Count == 0 ? 1 : listOverlay.Items.Count;
                lines.Add("overlays = " + numberOfOverlays + Environment.NewLine);
                for (int o = 0; o < numberOfOverlays; o++)
                {
                    if (o > 0)
                        lines.Add("");
                    Overlay currentOverlay = null;
                    string currentOverlayName = "";
                    if (listOverlay.Items.Count == 0)
                        currentOverlay = GenerateOverlay();
                    else
                    {
                        currentOverlayName = listOverlay.Items[o].Text;
                        currentOverlay = overlays[currentOverlayName];
                    }
                    lines.Add("overlay" + o + "_full_screen = true");
                    lines.Add("overlay" + o + "_normalized = true");
                    if (currentOverlayName != "")
                        lines.Add("overlay" + o + "_name = \"" + currentOverlayName + "\"");
                    lines.Add("overlay" + o + "_range_mod = 1.5");
                    lines.Add("overlay" + o + "_alpha_mod = 2.0" + Environment.NewLine);
                    lines.Add("overlay" + o + "_descs = " + currentOverlay.NumberOfDescs());
                    int i = 0;

                    if (currentOverlay.Is8Way)
                    {
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.LEFT, ref i, "039", "8", h: "22");
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.RIGHT, ref i, "185", "8", h: "22");
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.UP, ref i, "112", "67", w: "12");
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.DOWN, ref i, "112", "93", w: "12");
                    }
                    else
                    {
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.LEFT, ref i, "039", currentOverlay.NeedTwoWayArrows() ? "93" : "8");
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.RIGHT, ref i, "185", currentOverlay.NeedTwoWayArrows() ? "93" : "8");
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.UP, ref i, "112", "67");
                        AddArrowButton(lines, o, currentOverlay, Overlay.ButtonName.DOWN, ref i, "112", "93");
                    }

                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.E, ref i, 0, right: true);
                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.D, ref i, 1, right: true);
                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.C, ref i, 2, right: true);
                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.B, ref i, 3, right: true);
                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.A, ref i, 4, right: true);

                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.Z, ref i, 0, right: false);
                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.Y, ref i, 1, right: false);
                    AddLeftOrRightButton(lines, o, currentOverlay, Overlay.ButtonName.X, ref i, 2, right: false);

                    if (currentOverlay.NeedArrows())
                    {
                        lines.Add("overlay" + o + "_desc" + i + " = \"nul,0.11,0.80,rect,0.12,0.21\"");
                        lines.Add("overlay" + o + "_desc" + i + "_overlay = arrows" + (currentOverlay.NeedTwoWayArrows() ? "leftright" : "") + ".png");
                    }
                }
                File.WriteAllLines(dialog.FileName, lines.ToArray());
            }
        }


        // CREATE //


        bool CheckOverlayExists(string name)
        {
            foreach (ListViewItem item in listOverlay.Items)
                if (item.Text == name)
                {
                    MessageBox.Show("Overlay name already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return true;
                }
            return false;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAddOverlay formAddOverlay = new FormAddOverlay();
            formAddOverlay.ShowDialog();
            if (formAddOverlay.returnText != "" && !CheckOverlayExists(formAddOverlay.returnText))
            {
                ListViewItem newItem = new ListViewItem(formAddOverlay.returnText);
                listOverlay.Items.Add(newItem);
                if (labelOverlayName.Text != "")
                    overlays[formAddOverlay.returnText] = GenerateEmptyOverlay();
                newItem.Selected = true;
            }
        }


        // REMOVE //


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            ListViewItem selectedItem = null;
            ListViewItem nextItem = null;
            foreach (ListViewItem item in listOverlay.Items)
            {
                if (selectedItem != null)
                {
                    nextItem = item;
                    break;
                }
                else if (item.Text == labelOverlayName.Text)
                    selectedItem = item;
            }
            if (selectedItem != null)
                listOverlay.Items.Remove(selectedItem);
            if (nextItem != null)
                nextItem.Selected = true;
            else
            {
                if (listOverlay.Items.Count > 0)
                    listOverlay.Items[listOverlay.Items.Count - 1].Selected = true;
                else
                {
                    ResetOverlays();
                    Reset();
                }
            }
        }


        // RENAME //


        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (listOverlay.Items.Count == 0)
                return;
            FormAddOverlay formRename = new FormAddOverlay(labelOverlayName.Text);
            formRename.ShowDialog();
            if (formRename.returnText != "" && formRename.returnText != labelOverlayName.Text && !CheckOverlayExists(formRename.returnText))
            {
                foreach (ListViewItem item in listOverlay.Items)
                    if (item.Text == labelOverlayName.Text)
                    {
                        item.Text = formRename.returnText;
                        break;
                    }
                if (overlays.ContainsKey(labelOverlayName.Text))
                    overlays[formRename.returnText] = overlays[labelOverlayName.Text];
                labelOverlayName.Text = formRename.returnText;
            }
        }


        // OTHER //


        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemHow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Create and edit specific RetroArch keyboard overlay files using this program.\n\nOverlay (top half of window): Select a button you wish to remap by clicking on it. Click again to deselect it. Change the icon by choosing one from the list next to the button. If you wish, you can manage layers in the middle of the screen.\n\nKeyboard (bottom half of window): After you selected an overlay button, click on a keyboard key. It will change color, showing you which button you assigned it to. To remove the connection, select the same button-key combination again.\n\nWarning: This program generates a specific overlay that requires certain PNG files in order to work. Also, if you modify button positions manually or anything like that, the program might not be able to load it anymore.", "How to use");
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RetroArch Keyboard Overlay Editor 1.0\n© 2019 SeriousCsaba", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
