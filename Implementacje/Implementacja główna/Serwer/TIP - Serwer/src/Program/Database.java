package Program;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class Database
{
	private Connection conn = null;
	private Statement statement = null;
	private ResultSet rs = null;
	private String DBServer = null;
	private int DBPort;
	private String DBUser = null;
	private String DBPass = null;
	private String DBCommand = null;
	
	private String writeResultSet(ResultSet rs)
	{
		String result = "";
		
		try 
		{
			String UserName = null;
			
			while(rs.next())
			{
				UserName = rs.getString("UserName");
				int UserNumber = rs.getInt("UserNumber");
				
				result += (UserName + " " + UserNumber + "\n");
			}
			return result;
		} 
		catch (SQLException e) 
		{
			System.out.println("Nast¹pi³ problem z pobraniem wartoœci z Bazy danych!");
			return result;
		}
	}
	
	private String writeUserNumber(ResultSet rs)
	{
		String result = "";
		
		try 
		{
			while(rs.next())
			{
				int UserNumber = rs.getInt("UserNumber");
				
				result += (UserNumber);
			}
			return result;
		} 
		catch (SQLException e) 
		{
			System.out.println("Nast¹pi³ problem z pobraniem wartoœci z Bazy danych!");
			return result;
		}
	}
	
	private String CreateDBCommand(String address, int MySQLPort)
	{
		return "jdbc:mysql://"+ address +":" + MySQLPort + "/TIP";
	}	
	public Database(String server,int port, String user, String pass)
	{
		this.DBServer = server;
		this.DBPort = port;
		this.DBUser = user;
		this.DBPass = pass;
		
		try 
		{
			Class.forName("com.mysql.jdbc.Driver");
			DBCommand = CreateDBCommand(DBServer, DBPort);
			
			conn = DriverManager.getConnection(DBCommand, DBUser, DBPass);
		} 
		catch (ClassNotFoundException e) 
		{
			System.out.println("B³¹d ³adowania sterownika Bazy danych! Serwer koñczy dzia³anie!");
			System.exit(1);
		} 
		catch (SQLException e) 
		{
			System.out.println("Nie mo¿na po³¹czyæ siê z baz¹ danych! Serwer koñczy dzia³anie!");
			e.printStackTrace();
			System.exit(1);
		}
	}
	public String readDatabase(String query)
	{
		String results = "";
		
		try 
		{
			statement = conn.createStatement();
			rs = statement.executeQuery(query);
			results = writeResultSet(rs);
		} 
		catch (SQLException e) 
		{
			System.out.println("B³¹d podczas realizacji zapytania SQL");
			return results;
		}
		finally
		{
			try 
			{
				rs.close();
				statement.close();
			} 
			catch (SQLException e) 
			{
				e.printStackTrace();
			}
			
		}
		return results;
	}
	public boolean addUser(String userName, int userNumber)
	{
		boolean result = false;
		try 
		{
			String query = "insert into USERS values('" + userName + "', " + userNumber +")"; 
			
			statement = conn.createStatement();
			int count = statement.executeUpdate(query);
			if(count != 0)
			{
				result =  true;
			}
			else
			{
				result =  false;
			}
		} 
		catch (SQLException e) 
		{
			System.out.println("Nast¹pi³ problem z dodaniem nowego u¿ytkownika: " + userName + " " + userNumber);
			e.printStackTrace();
		}
		finally
		{
			try 
			{
				statement.close();
			}
			catch (SQLException e) 
			{
				System.out.println("Nast¹pi³ problem z przetwarzaniem danych wprowadzaj¹cych do Bazy danych!");
			}
		}	
		
		return result;
	}
	private String readUserNumber(int userNumber)
	{
		String results = "";
		
		try 
		{
			statement = conn.createStatement();
			rs = statement.executeQuery("select UserNumber from USERS where UserNumber = " + userNumber);
			results = writeUserNumber(rs);
			
			rs.close();
			statement.close();
		} 
		catch (SQLException e) 
		{
			System.out.println("B³¹d podczas realizacji zapytania SQL");
			return results;
		}
		finally
		{
			try 
			{
				rs.close();
				statement.close();
			} 
			catch (SQLException e) 
			{
				e.printStackTrace();
			}
			
		}
		return results;
	}
	
	public boolean checkUser(int userNumber)
	{
		String recData = readUserNumber(userNumber);
		if(recData.isEmpty())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
