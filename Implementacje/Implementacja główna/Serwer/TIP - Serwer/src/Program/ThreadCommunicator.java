package Program;

import java.util.LinkedList;
import java.util.List;

public class ThreadCommunicator extends Thread
{
	protected static List<PluggedUser> plugged = new LinkedList<PluggedUser>();
	protected static List<PluggedUser> dialed = new LinkedList<PluggedUser>();
	
	protected void moveUsers(int user1, int user2)
	{
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == user1)
			{
				dialed.add(user);
			}
		}
		
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == user2)
			{
				dialed.add(user);
			}
		}
	}
	
	protected void deleteUsers(int user1, int user2)
	{
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == user1)
			{
				dialed.remove(user);
			}
		}
		
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == user2)
			{
				dialed.remove(user);
			}
		}
	}
	protected PluggedUser getUser(int userNumber)
	{
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == userNumber)
			{
				return user;
			}
		}
		return null;
	}
	protected String getIPAddress(int userNumber)
	{
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == userNumber)
			{
				return user.userIPAddress;
			}
		}
		return null;
	}
	
	protected int getUserPort(int number)
	{
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == number)
			{
				return user.userPort;
			}
		}
		return 0;
	}
	protected boolean isPlugged(int input)
	{
		for(PluggedUser user : plugged)
		{
			if(user.userNumber == input)
			{
				return true;
			}
		}
		return false;
	}
	
	protected boolean isDialed(int number1, int number2)
	{
		boolean result1 = false, result2 = false;
		
		for(PluggedUser user : dialed)
		{
			if(user.userNumber == number1)
			{
				result1 = true;
				break;
			}
		}
		

		for(PluggedUser user : dialed)
		{
			if(user.userNumber == number2)
			{
				result2 = true;
				break;
			}
		}

		
		if(result1 == result2 == true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	protected boolean isBusy(int input)
	{
		for(PluggedUser user : dialed)
		{
			if(user.userNumber == input)
			{
				return true;
			}
		}
		return false;
	}
}
