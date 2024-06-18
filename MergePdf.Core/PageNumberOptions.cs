// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

namespace MergePdf.Core;

public class PageNumberOptions
{
    public const string TopLeft = "tl";
    public const string TopMiddle = "tm";
    public const string TopRight = "tr";
    public const string BottomLeft = "bl";
    public const string BottomMiddle = "bm";
    public const string BottomRight = "br";

    public const string ValueCurrent = "$current";
    public const string ValueTotal = "$total";

    public string FontFamily { set; get; } = "Arial";
    public int FontSize { set; get; } = 8;
    public string Location { set; get; } = BottomMiddle;
    public string Format { set; get; } = $"Page {ValueCurrent} / {ValueTotal}";

    public void Validate()
    {
        if (string.IsNullOrEmpty(FontFamily))
        {
            throw new PdfMergeException("Font family must be specified");
        }

        if (FontSize <= 0)
        {
            throw new PdfMergeException("Font size must be greater than 0");
        }

        if (Location.Equals("tc", StringComparison.OrdinalIgnoreCase))
        {
            Location = TopMiddle;
        }

        if (Location.Equals("bc", StringComparison.OrdinalIgnoreCase))
        {
            Location = BottomMiddle;
        }

        if (Location != TopLeft && Location != TopMiddle && Location != TopRight &&
            Location != BottomLeft && Location != BottomMiddle && Location != BottomRight)
        {
            throw new PdfMergeException("Invalid location");
        }
    }
}