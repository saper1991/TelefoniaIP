package Program;

import java.util.LinkedList;
import java.util.List;

public class ThreadCommunicator extends Thread
{
	protected static List<Integer> plugged = new LinkedList<Integer>();
	protected static List<Integer> dialed = new LinkedList<Integer>();
}
