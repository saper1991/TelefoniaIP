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
	
	private static int DBPort;
	private static String DBServer, DBUser, DBPass;
	private static Database db = null;
	
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
	private static boolean validateIPAddress(String address)
	{
		if(address.length() >= 7 && address.length() <= 15)
		{
			byte count = 0;
			int[] oct = new int[4];
			String[] soct = new String[4];
			
			for(byte i = 0 ; i < 4 ; i++)
			{
				soct[i] = "";
			}
			
			for(int i = 0 ; i < address.length() ; i++)
			{
				if(address.charAt(i) == '.')
				{
					count++;
				}
				else
				{
					soct[count] += address.charAt(i);
				}
			}
			
			for(int i = 0 ; i < 4 ; i++)
			{
				oct[i] = Integer.parseInt(soct[i]);
				if(!(oct[i] >= 0) && !(oct[i] <= 255))
				{
					return false;
				}
			}
			
			return true;
		}
		else
		{
			return false;
		}
	}
	
	private static void Init() throws IllegalPortRangeException, IllegalServerCountException, IllegalIPSyntaxException 
	{
		initialFile = new File(initialFileName);
		Scanner sc;
		try 
		{
			sc = new Scanner(initialFile);
			initialPort = sc.nextInt();
			SERVERSNUM = sc.nextInt();
			DBServer = sc.next();
			DBPort = sc.nextInt();
			DBUser = sc.next();
			DBPass = sc.next();
			
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
		
		if(!validateIPAddress(DBServer))
		{
			throw new IllegalIPSyntaxException("Niew³aœciwy format adresu IP u¿yty w pliku konfiguracyjnym!");
		}
		if(!validatePort(DBPort))
		{
			throw new IllegalPortRangeException("Wartoœæ portu Bazy danych ma wartoœæ niedopuszczaln¹!");
		}
	}
	
	public static void main(String[] args) 
	{
		try 
		{
			Init();
			server = new ServerSocket(initialPort);
			db = new Database(DBServer, DBPort, DBUser, DBPass);
			
			for(int i = 0 ; i < SERVERSNUM ; i++)
			{
				new Server(server, db);
			}
		} 
		catch (IllegalPortRangeException | IllegalServerCountException | IOException | IllegalIPSyntaxException e) 
		{
			e.printStackTrace();
			System.out.println("Serwer koñczy dzia³anie!");
			System.exit(1);
		}
	}
}
