﻿// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

namespace MergePdf.GUI.Models;

class OptionItem
{
    public OptionItem(int kind, string option)
    {
        Kind = kind;
        Option = option;
    }

    public int Kind { get; private set; }
    public string Option { get; private set; }
}