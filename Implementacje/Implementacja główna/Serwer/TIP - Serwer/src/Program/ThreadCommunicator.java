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
	protected boolean isPlugged(int input)
	{
		if(plugged.contains(input))
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
		if(dialed.contains(input))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
