package Program;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;

public class Server extends ThreadCommunicator
{
	private ServerSocket server = null;
	private Database db = null;
	private volatile boolean isRunning = true;
	
	
	public Server(ServerSocket ss, Database db)
	{
		this.server = ss;
		this.db = db;
		start();
	}
	
	public void run()
	{
		try
		{
			while(isRunning)
			{
				Socket socket = server.accept();
				InputStream is = socket.getInputStream();
				OutputStream os = socket.getOutputStream();
				
				try
				{
					serviceRequest(is, os, db);
				}
				catch(SocketException ex)
				{
					is.close();
					os.close();
					socket.close();
					continue;
				}
				
				is.close();
				os.close();
				socket.close();
			}
		}
		catch(IOException ex)
		{
			ex.printStackTrace();
		}
		finally
		{
			try 
			{
				server.close();
			} 
			catch (IOException e) 
			{
				e.printStackTrace();
			}
		}
	}
	public void serviceRequest(InputStream istream, OutputStream ostream, Database db) throws IOException
	{
		String recText;
		while(true)
		{
			recText = SocketConverter.receiveText(istream);
			
			if(recText.equals("SHOWALL"))
			{
				SocketConverter.SendText(ostream, db.ReadDatabase());
			}
		}
	}
}
