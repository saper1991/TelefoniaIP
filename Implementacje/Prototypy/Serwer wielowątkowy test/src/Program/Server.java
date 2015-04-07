package Program;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;

public class Server extends Thread
{
	@SuppressWarnings("unused")
	private int port = 0;
	private ServerSocket server = null;
	private volatile boolean isRunning = true;
	
	
	public Server(ServerSocket ss)
	{
		this.server = ss;
		
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
					serviceRequest(is, os);
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
	public void serviceRequest(InputStream istream, OutputStream ostream) throws IOException
	{
		String recText, senText;
		while(true)
		{
			recText = SocketConverter.receiveText(istream);
			if(!recText.equals("exit"))
			{
				senText = recText.toUpperCase();
				SocketConverter.SendText(ostream, senText);
			}
			else
			{
				break;
			}
		}
	}
}
