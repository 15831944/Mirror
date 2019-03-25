﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportMH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var content = this.txtName.Text;
            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("请输入要举报的名称再进行提交");
                return;
            }
            string toEmail = "chinaimba1314@163.com";
            //var toEmail = "51758018@qq.com";
            var fromEmail = "q51758018@163.com";
            var subject = "举报";

            SendEmail(toEmail, fromEmail, subject, content);
            MessageBox.Show("发送成功");
        }

        private void SendEmail(string toEmail, string fromEmail, string subject, string content)
        {
            var smtpCode = "shouquan163";
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress(fromEmail);
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress(toEmail));
            //邮件标题。
            mailMessage.Subject = subject;
            //邮件内容。
            mailMessage.Body = content;
            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = "smtp.163.com";
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential(fromEmail, smtpCode);
            //发送
            client.Send(mailMessage);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtName.TabIndex = 0;
        }
    }
}
