using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JSMinify
{
  public class Minify
  {
    private string mFileName = "";              // file to process
    private string mOriginalData = "";           // data from original file
    private string mModifiedData = "";          // processed data
    private bool mIsError = false;               // becomes true if any error happens
    private string mErr = "";                    // error message 
    private BinaryReader mReader = null;         // stream to process the file byte by byte

    private const int EOF = -1;                 // for end of file

    /// <summary>
    /// Constructor - does all the processing
    /// </summary>
    /// <param name="f">file path</param>
    public Minify(string f)
    {

      try
      {
        if (File.Exists(f))
        {
          mFileName = f;

          //read contents completely. This is only for test purposes. The actual processing is done by another stream
          StreamReader rdr = new StreamReader(mFileName);
          mOriginalData = rdr.ReadToEnd();
          rdr.Close();

          mReader = new BinaryReader(new FileStream(mFileName, FileMode.Open));
          doProcess();
          mReader.Close();

          //write modified data
          //string outFile = mFileName + ".min";
          //StreamWriter wrt = new StreamWriter(outFile);
          //wrt.Write(mModifiedData);
          //wrt.Close();

        }
        else
        {
          mIsError = true;
          mErr = "File does not exist";
        }

      }
      catch (Exception ex)
      {
        mIsError = true;
        mErr = ex.Message;
      }
    }

    /// <summary>
    /// Main process
    /// </summary>
    private void doProcess()
    {
      int lastChar = 1;                   // current byte read
      int thisChar = -1;                  // previous byte read
      int nextChar = -1;                  // byte read in peek()
      bool endProcess = false;            // loop control
      bool ignore = false;                // if false then add byte to final output
      bool inComment = false;             // true when current bytes are part of a comment
      bool isDoubleSlashComment = false;  // '//' comment


      // main processing loop
      while (!endProcess)
      {
        endProcess = (mReader.PeekChar() == -1);    // check for EOF before reading
        if (endProcess)
          break;

        ignore = false;
        thisChar = mReader.ReadByte();

        if (thisChar == '\t')
          thisChar = ' ';
        else if (thisChar == '\t')
          thisChar = '\n';
        else if (thisChar == '\r')
          thisChar = '\n';

        if (thisChar == '\n')
          ignore = true;

        if (thisChar == ' ')
        {
          if ((lastChar == ' ') || isDelimiter(lastChar) == 1)
            ignore = true;
          else
          {
            endProcess = (mReader.PeekChar() == -1); // check for EOF
            if (!endProcess)
            {
              nextChar = mReader.PeekChar();
              if (isDelimiter(nextChar) == 1)
                ignore = true;
            }
          }
        }


        if (thisChar == '/')
        {
          nextChar = mReader.PeekChar();
          if (nextChar == '/' || nextChar == '*')
          {
            ignore = true;
            inComment = true;
            if (nextChar == '/')
              isDoubleSlashComment = true;
            else
              isDoubleSlashComment = false;
          }


        }

        // ignore all characters till we reach end of comment
        if (inComment)
        {
          while (true)
          {
            thisChar = mReader.ReadByte();
            if (thisChar == '*')
            {
              nextChar = mReader.PeekChar();
              if (nextChar == '/')
              {
                thisChar = mReader.ReadByte();
                inComment = false;
                break;
              }
            }
            if (isDoubleSlashComment && thisChar == '\n')
            {
              inComment = false;
              break;
            }

          } // while (true)
          ignore = true;
        } // if (inComment) 


        if (!ignore)
          addToOutput(thisChar);

        lastChar = thisChar;
      } // while (!endProcess) 
    }


    /// <summary>
    /// Add character to modified data string
    /// </summary>
    /// <param name="c">char to add</param>
    private void addToOutput(int c)
    {
      mModifiedData += (char)c;
    }


    /// <summary>
    /// Original data from file
    /// </summary>
    /// <returns></returns>
    public string getOriginalData()
    {
      return mOriginalData;
    }

    /// <summary>
    /// Modified data after processing
    /// </summary>
    /// <returns></returns>
    public string getModifiedData()
    {
      if (mModifiedData.Contains("ï»¿"))
        mModifiedData = mModifiedData.Replace("ï»¿", string.Empty);
      return mModifiedData;
    }

    /// <summary>
    /// Check if a byte is alphanumeric
    /// </summary>
    /// <param name="c">byte to check</param>
    /// <returns>retval - 1 if yes. else 0</returns>
    private int isAlphanumeric(int c)
    {
      int retval = 0;

      if ((c >= 'a' && c <= 'z') ||
          (c >= '0' && c <= '9') ||
          (c >= 'A' && c <= 'Z') ||
          c == '_' || c == '$' || c == '\\' || c > 126)
        retval = 1;

      return retval;

    }

    /// <summary>
    /// Check if a byte is a delimiter 
    /// </summary>
    /// <param name="c">byte to check</param>
    /// <returns>retval - 1 if yes. else 0</returns>
    private int isDelimiter(int c)
    {
      int retval = 0;

      if (c == '(' || c == ',' || c == '=' || c == ':' ||
          c == '[' || c == '!' || c == '&' || c == '|' ||
          c == '?' || c == '+' || c == '-' || c == '~' ||
          c == '*' || c == '/' || c == '{' || c == '\n' ||
          c == ','
      )
      {
        retval = 1;
      }

      return retval;

    }



  }
}