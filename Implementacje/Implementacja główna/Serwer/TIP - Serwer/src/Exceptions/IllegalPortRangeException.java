package Exceptions;

public class IllegalPortRangeException extends Exception
{
	private static final long serialVersionUID = 1L;

	public IllegalPortRangeException(String exMessage)
	{
		System.out.println(exMessage);
		System.exit(1);
	}
}