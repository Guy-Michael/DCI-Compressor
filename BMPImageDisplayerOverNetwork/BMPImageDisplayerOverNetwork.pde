import processing.net.*;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
byte[] headerData;
byte[] pixelData;
int imageWidth;
int imageHeight;
int pixelDataOffset;
int paddingPerLine;
Server server;
Client client;
String fileName;
final String extension =".bmp";

int i=0, j=0;

boolean finished = false;
void settings()
{
  //client = new Client(this, "127.0.0.1", 1234);
  server = new Server(this, 1234);
  //loadImageByName("New  BlueSmiley");


  while ((client = server.available()) == null)
  {
    //println("Waiting..");
    //Waiting for client to connect and send data.
  }
  
  ArrayList<Byte> list = new ArrayList<Byte>();
  
  while (list.size() < 54)
  {
    list.add(getByteOverNetwork());
  }
  headerData = convertArrayListToArray(list);
  calcWidthHeightAndOffset();
  calcPaddingAmount();  
  size(imageWidth, imageHeight); 
  println(paddingPerLine);
  println("imagewidth: " +imageWidth + "    imageHeight: " + imageHeight);

  i = imageHeight;
}



void draw()
{

  while (!finished)
  {
    byte[] pixel = readPixel();

    int blue = (0xff & pixel[0] );
    int green = ((0xff & pixel[1]));
    int red = ((0xff & pixel[2]));   

    color current = color(red, green, blue);
    stroke(current);
    set(j, i, current);
    //updatePixels();

    j++;

    if (j >= imageWidth)
    {
      j = 0;
      i--;
      disposePaddingBytes();
    }

    if (i==0)
    {
      finished = true;
    }
  }
}

byte[] readPixel()
{
  ArrayList<Byte> list = new ArrayList<Byte>();
  while (list.size()<3 )
  {
    if (client.available()>0)
    {
      byte b =client.readBytes(1)[0];
      list.add(b);
    }
  }

  byte[] arr = convertArrayListToArray(list);
  return arr;
}

byte[] convertArrayListToArray(ArrayList<Byte> list)
{
  byte[] array = new byte[list.size()];

  for (int i = 0; i < array.length; i++)
  {
    array[i] = list.get(i);
  }

  return array;
}


byte getByteOverNetwork()
{
  while(client.available()<1)
  {
    //wait
  }
  
  return client.readBytes(1)[0];
}


void disposePaddingBytes()
{
    int i = 0;
    
    while(i < paddingPerLine)
    {
      if (client.available()>0)
      {
        println("Skipped "+client.readBytes(1)[0]);
        i++;
      }
    }
    println(i +"bytes disposed");
}
