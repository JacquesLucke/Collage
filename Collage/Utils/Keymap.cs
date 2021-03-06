﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Collage
{
    public class Keymap
    {
        Dictionary<string, KeyCombination> combinations;

        public Keymap() 
        {
            combinations = new Dictionary<string, KeyCombination>();

            Set("empty", new KeyCombination(false, false, false));
        }

        public void Set(string alias, KeyCombination keyCombination)
        {
            if (!combinations.ContainsKey(alias)) combinations.Add(alias, keyCombination);
            else combinations[alias] = keyCombination;
        }

        public KeyCombination this[string alias]
        {
            get
            {
                string lower = alias.ToLower();
                if (combinations.ContainsKey(lower)) return combinations[lower];
                else return combinations["empty"];
            }
        }
    }

    public struct KeyCombination
    {
        bool isStrg, isAlt, isShift;
        Keys[] keys;

        public KeyCombination(bool isStrg, bool isAlt, bool isShift, params Keys[] keys)
        {
            this.keys = keys;
            this.isStrg = isStrg;
            this.isAlt = isAlt;
            this.isShift = isShift;
        }

        public bool IsStrg { get { return isStrg; } set { isStrg = value; } }
        public bool IsAlt { get { return isAlt; } set { isAlt = value; } }
        public bool IsShift { get { return isShift; } set { isShift = value; } }

        public Keys[] Keys { get { return keys; } set { keys = value; } }

        public bool IsDown(Input input)
        {
            return isStrg == input.IsStrg && isAlt == input.IsAlt && isShift == input.IsShift && input.AreKeysDown(keys);
        }
        public bool IsPressed(Input input)
        {
            bool extraKeys = isStrg == input.IsStrg && isAlt == input.IsAlt && isShift == input.IsShift;
            bool keysOK = false;
            foreach(Keys key in keys)
            {
                if (!input.IsKeyDown(key)) { keysOK = false; break; }
                if (input.IsKeyPressed(key)) keysOK = true;
            }

            return extraKeys && keysOK;
        }

        public override string ToString()
        {
            string output = "";
            if (isStrg && isShift && isAlt) output = "Strg + Shift + Alt";
            if (isStrg && isShift && !isAlt) output = "Strg + Shift";
            if (isStrg && !isShift && isAlt) output = "Strg + Alt";
            if (!isStrg && isShift && isAlt) output = "Shift + Alt";
            if (isStrg && !isShift && !isAlt) output = "Strg";
            if (!isStrg && isShift && !isAlt) output = "Shift";
            if (!isStrg && !isStrg && isAlt) output = "Alt";
            output += " ";

            for (int i = 0; i < Keys.Length; i++)
            {
                output += Enum.GetName(typeof(Keys), Keys[i]) + " ";
            }

            return output.Trim();
        }
    }
}
