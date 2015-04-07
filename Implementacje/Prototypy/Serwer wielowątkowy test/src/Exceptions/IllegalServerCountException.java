package Exceptions;

public class IllegalServerCountException extends Exception
{
	private static final long serialVersionUID = 2L;

	public IllegalServerCountException(String exMessage)
	{
		System.out.println(exMessage);
		System.exit(1);
	}
}
