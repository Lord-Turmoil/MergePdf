﻿namespace MergePdf.Core;

public interface IMergeProgress
{
    bool OnMergeFile(string file);
    bool OnMergePage(int current, int total);
}

public class DefaultMergeProgress : IMergeProgress
{
    public bool OnMergeFile(string file)
    {
        return true;
    }

    public bool OnMergePage(int current, int total)
    {
        return true;
    }
}