﻿namespace EnergyCostTool.Models.Exceptions;

public class FileException : Exception
{
    public FileException(string message)
        : base(message)
    {
    }

    public FileException(string message, Exception inner)
        : base(message, inner)
    {
    }
}