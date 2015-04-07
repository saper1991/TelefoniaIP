package Program;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.net.ServerSocket;
import java.util.NoSuchElementException;
import java.util.Scanner;

import Exceptions.*;

public class Main 
{
	private static File initialFile = null;
	private static String initialFileName = "Config.txt";
	private static int initialPort = 0;
	private static int SERVERSNUM = 0;
	private static ServerSocket server = null; 

	private static boolean validatePort(int port)
	{
		if(port >= 0 && port <= 65535)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private static boolean validateServerCount(int serverCount)
	{
		if(serverCount > 0 && serverCount <= 100)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private static void Init() throws IllegalPortRangeException, IllegalServerCountException 
	{
		initialFile = new File(initialFileName);
		Scanner sc;
		try 
		{
			sc = new Scanner(initialFile);
			initialPort = sc.nextInt();
			SERVERSNUM = sc.nextInt();
			
			sc.close();
		} 
		catch (FileNotFoundException e) 
		{
			System.out.println("Plik konfiguracyjny nie istnieje!");
			e.printStackTrace();
		}
		catch (NoSuchElementException e)
		{
			System.out.println("Plik konfiguracyjny jest niekomplentny!");
			e.printStackTrace();
		}
		
		if(!validatePort(initialPort))
		{
			throw new IllegalPortRangeException("Wartoœci portu s¹ niedopuszczalne!");
		}
		
		if(!validateServerCount(SERVERSNUM))
		{
			throw new IllegalServerCountException("Iloœæ w¹tków przekracza dopuszczaln¹ wartoœæ");
		}
	}
	
	public static void main(String[] args) 
	{
		try 
		{
			Init();
			server = new ServerSocket(initialPort);
			
			for(int i = 0 ; i < SERVERSNUM ; i++)
			{
				new Server(server);
			}
		} 
		catch (IllegalPortRangeException | IllegalServerCountException | IOException e) 
		{
			e.printStackTrace();
			System.exit(1);
		}
	}
}
