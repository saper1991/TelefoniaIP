package Program;

public class PluggedUser 
{
	public String userName = null;
	public int userNumber = 0;
	public String userIPAddress = null;
	public int userPort;
	
	public PluggedUser(String userName, int userNumber, String userIPAddress, int userPort)
	{
		this.userName = userName;
		this.userNumber = userNumber;
		this.userIPAddress = userIPAddress;
		this.userPort = userPort;
	}
}
