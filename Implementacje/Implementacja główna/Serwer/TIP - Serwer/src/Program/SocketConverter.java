package Program;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

public class SocketConverter 
{
	public static void SendText(OutputStream os, String toSend) throws IOException
	{
		byte[] toSendBytes = toSend.getBytes();
		int toSendLen = toSendBytes.length;
		byte[] toSendLenBytes = new byte[4];
			
		toSendLenBytes[0] = (byte)(toSendLen & 0xff);
	    toSendLenBytes[1] = (byte)((toSendLen >> 8) & 0xff);
	    toSendLenBytes[2] = (byte)((toSendLen >> 16) & 0xff);
	    toSendLenBytes[3] = (byte)((toSendLen >> 24) & 0xff);
	    
	    os.write(toSendLenBytes);
        os.write(toSendBytes);
	}
	
	public static String receiveText(InputStream is) throws IOException
	{
		byte[] lenBytes = new byte[4];
        is.read(lenBytes, 0, 4);
        int len = (((lenBytes[3] & 0xff) << 24) | ((lenBytes[2] & 0xff) << 16) |
                  ((lenBytes[1] & 0xff) << 8) | (lenBytes[0] & 0xff));
        byte[] receivedBytes = new byte[len];
        is.read(receivedBytes, 0, len);
        String received = new String(receivedBytes, 0, len);
		
		return received;
	}
}
