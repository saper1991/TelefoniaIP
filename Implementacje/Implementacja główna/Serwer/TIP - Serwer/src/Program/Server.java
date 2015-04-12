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
				if(!db.checkUser(Integer.parseInt(requests[2])))
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
			else if(requests[0].equals("HELLO"))
			{
				if(db.checkUser(Integer.parseInt(requests[1])))
				{
					String IPAddress = server.getInetAddress().toString();
					
					synchronized(this)
					{
						plugged.add(new PluggedUser(requests[1], Integer.parseInt(requests[2]), IPAddress));
					}
					SocketConverter.SendText(ostream, "OK " + requests[1]);
				}
				else
				{
					SocketConverter.SendText(ostream, "NOT " + requests[1]);
				}
			}
			else if(requests[0].equals("EXIT"))
			{
				PluggedUser selectedUser = getUser(Integer.parseInt(requests[1]));
				
				synchronized(this)
				{
					plugged.remove(selectedUser);
				}
				SocketConverter.SendText(ostream, "OK " + requests[1]);
			}
			else if(requests[0].equals("INVITE"))
			{
				if(!db.checkUser(Integer.parseInt(requests[2])))
				{
					SocketConverter.SendText(ostream, "NEX " + requests[1] + " " + requests[2] + " " + requests[3]);
				}
				else if(isBusy(Integer.parseInt(requests[2])))
				{
					SocketConverter.SendText(ostream, "BUSY " + requests[1] + " " + requests[2] + " " + requests[3]);
				}
				else if(isPlugged(Integer.parseInt(requests[2])))
				{
					SocketConverter.SendText(ostream, "NAC " + requests[1] + " " + requests[2] + " " + requests[3]);
				}
				else
				{
					String IP = getIPAddress(Integer.parseInt(requests[2]));
					Socket toClient = new Socket(IP, server.getLocalPort() + 1);
					OutputStream out = toClient.getOutputStream();
					InputStream in = toClient.getInputStream();
					
					SocketConverter.SendText(out, recText);
					SocketConverter.SendText(ostream, "TRANSFER " + requests[1] + " " + requests[2] + " " + requests[3]);
					String receive = SocketConverter.receiveText(in);
					String[] parsedReceive = receive.split(" ");
					SocketConverter.SendText(ostream, receive);
					if(parsedReceive[0].equals("OK"))
					{
						synchronized(this)
						{
							moveUsers(Integer.parseInt(requests[1]), Integer.parseInt(requests[2]));
						}
					}
					else if(parsedReceive[0].equals("BUSY"))
					{
						
					}
					
					in.close();
					out.close();
					toClient.close();
				}
			}
		}
	}
}
