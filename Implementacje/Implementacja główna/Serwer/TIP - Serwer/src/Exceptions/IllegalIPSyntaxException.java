package Exceptions;

public class IllegalIPSyntaxException extends Exception
{

	private static final long serialVersionUID = 3L;

	public IllegalIPSyntaxException(String exMessage)
	{
		System.out.println(exMessage);
	}
}
