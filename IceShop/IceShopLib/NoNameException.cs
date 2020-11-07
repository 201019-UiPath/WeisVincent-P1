using System;
/// <summary>
/// A barebones customer exception intended to be used if a user of a program ends up with an empty name property.
/// </summary>
public class NoNameException : Exception
{

    public override string Message
    {
        get
        {
            return "The name cannot be blank.";
        }
    }

    public NoNameException()
    {

    }

    public NoNameException(string message) : base(message)
    {

    }
    public NoNameException(string message, Exception inner) : base(message, inner)
    {

    }
}