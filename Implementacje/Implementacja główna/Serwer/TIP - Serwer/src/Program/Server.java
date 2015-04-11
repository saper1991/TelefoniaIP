package Program;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;

public class Server extends ThreadCommunicator implements Runnable
{
	private ServerSocket server = null;
	private Database db = null;
	private volatile boolean isRunning = true;
	
	
	public Server(ServerSocket ss, Database db)
	{
		this.server = ss;
		this.db = db;
		this.start();
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
		String[] requests = null;
		while(true)
		{
			recText = SocketConverter.receiveText(istream);
			requests = recText.split(" ");
			
			if(requests[0].equals("ADD"))
			{
				if(db.checkUser(Integer.parseInt(requests[2])))
				{
					boolean inserted = db.addUser(requests[1], Integer.parseInt(requests[2]));
					if(inserted)
					{
						SocketConverter.SendText(ostream, "OK" + requests[1] + " " + requests[2]);
					}
					else
					{
						SocketConverter.SendText(ostream, "NOK " + requests[1] + " " + requests[2]);
					}
					
				}
				else
				{
					SocketConverter.SendText(ostream, "NOK " + requests[1] + " " + requests[2]);
				}
			}
		}
	}
}
