
public void calcPaddingAmount()
{
  int imageWidthInBytes = imageWidth*3;
  if (imageWidthInBytes%4 != 4)
  {
    for (int i = 1; i < 4; i++)
    {
      if ((imageWidthInBytes + i)%4 == 0)
      {
        paddingPerLine = i;
        break;
      }
    }
  } else
  {
    paddingPerLine = 0;
  }
}


public void calcWidthHeightAndOffset()
{
  ByteBuffer bb = ByteBuffer.wrap(headerData).order(ByteOrder.LITTLE_ENDIAN);

  bb.position(0xa);
  pixelDataOffset = bb.getInt();
  bb.position(0x12);
  imageWidth=bb.getInt();
  imageHeight=bb.getInt();
}

public void loadImageByName(String imageName)
{
   fileName = imageName;
  headerData = loadBytes(fileName + extension);
}








//void setup()
//{
  //int k = pixelDataOffset;
  //for (int i = imageHeight; i > 0; i--)
  //{
  //  for (int j = 0; j < imageWidth; j++)
  //  {
  //    int blue = (0xff & headerData[k] );
  //    int green = ((0xff & headerData[k+1]));
  //    int red = ((0xff & headerData[k+2]));   

  //    color current = color(red, green, blue);
  //    stroke(current);
  //    set(j, i, current);
  //    k+=3;
  //}

  //Skip padding bytes.
  //k += paddingPerLine;
  //}
//}
