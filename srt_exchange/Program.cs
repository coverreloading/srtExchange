using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace srt_exchange
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the current directory.获得程序当前路径
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine("The current directory is {0}", path);

            //DirectoryInfo dicInfo = new DirectoryInfo(@"D:\User\desktop\srt");
            DirectoryInfo dicInfo = new DirectoryInfo(path);
            Directory.CreateDirectory(path + "\\new\\");
            foreach (var fi in dicInfo.GetFiles("*.srt"))
            {
                Console.WriteLine(fi.Name);

                using (StreamReader fs = new StreamReader(path + "\\" + fi.Name, Encoding.Default))
                {
                    //string data = fs.ReadToEnd();
                    String data = "";

                    while (!fs.EndOfStream)
                    {

                        string buf = fs.ReadLine();

                        if (buf != "WEBVTT")
                        {
                            data += buf + "\n";
                        }
                    }

                    Regex regex = new Regex("\n[0-9][0-9]:[0-9][0-9]:[0-9][0-9].[0-9][0-9][0-9]");

                    var matches = regex.Matches(data);

                    int i = 1;

                    foreach (Match match in matches)
                    {
                        var oldValue = match.Groups["0"].Value;

                        var newValue = "\r\n" + i + oldValue;
                        //Console.WriteLine(oldValue+"   "+newValue+"\n");
                        i++;

                        data = data.Replace(oldValue, newValue);
                    }

                    Console.WriteLine(data);

                    using (FileStream fs2 = new FileStream(path + "\\new\\" + fi.Name, FileMode.OpenOrCreate))
                    {
                        byte[] bytes = Encoding.Default.GetBytes(data);

                        fs2.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }
    }
}