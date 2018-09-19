

using System;
using System.Collections;
using System.IO;
using System.Text;

namespace BiTech.Library.Marc
{
    public class FileMARCReaders : IEnumerable, IDisposable
    {
        //Member Variables and Properties
        #region Member Variables and Properties

        private string filename = null;
        private readonly FileStream reader = null;
        private readonly bool forceUTF8 = false;

        private readonly string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        #region Constructors

        public FileMARCReaders(string filename)
        {
            this.filename = filename;
            reader = new FileStream(this.filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public FileMARCReaders(string filename, bool forceUTF8)
        {
            this.forceUTF8 = forceUTF8;
            this.filename = filename;
            reader = new FileStream(this.filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        #endregion
        public IEnumerator GetEnumerator()
        {
            int bufferSize = 10 * 1024 * 1024; // Read 10 MB at a time
            if (bufferSize > reader.Length)
                bufferSize = Convert.ToInt32(reader.Length);

            while (reader.Position < reader.Length)
            {
                byte[] ByteArray = new byte[bufferSize];
                int DelPosition, RealReadSize;

                do
                {
                    RealReadSize = reader.Read(ByteArray, 0, bufferSize);

                    if (RealReadSize != bufferSize)
                        Array.Resize(ref ByteArray, RealReadSize);

                    DelPosition = Array.LastIndexOf(ByteArray, Convert.ToByte(FileMARC.END_OF_RECORD)) + 1;

                    if (DelPosition == 0 & RealReadSize == bufferSize)
                    {
                        bufferSize *= 2;
                        ByteArray = new byte[bufferSize];
                    }
                } while (DelPosition == 0 & RealReadSize == bufferSize);

                //Some files will have trailer characters, usually a hex code 1A. 
                //The record has to at least be longer than the leader length of a MARC record, so it's a good place to make sure we have enough to at least try and make a record
                //Otherwise we will relying error checking in the FileMARC class
                if (ByteArray.Length > FileMARC.LEADER_LEN)
                {
                    string encoded;

                    reader.Position = reader.Position - (RealReadSize - DelPosition);
                    char marc8utf8Flag = Convert.ToChar(ByteArray[9]);

                    if (marc8utf8Flag == ' ' && !forceUTF8)
                    {
                        Encoding encoding = new MARC8();
                        encoded = encoding.GetString(ByteArray, 0, DelPosition);
                    }
                    else
                    {
                        encoded = Encoding.UTF8.GetString(ByteArray, 0, DelPosition);

                        if (encoded.StartsWith(byteOrderMarkUtf8))
                            encoded = encoded.Remove(0, byteOrderMarkUtf8.Length); //remove UTF8 Byte Order Mark
                    }

                    FileMARC marc = new FileMARC(encoded);
                    foreach (Record marcRecord in marc)
                    {
                        yield return marcRecord;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (reader != null)
                reader.Dispose();
            //throw new NotImplementedException();
        }

        #endregion

    }
}