using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ProjectNghiPhep.Email
{
    public class EmailHelper
    {
        //hàm gửi email
        public static bool SendMail(MailModel mailModel)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //Các email được gửi từ email này, nếu muốn gửi từ email khác thì sửa chỗ này là được (phải là gmail mới đc nha)
                //Muốn gửi mail từ ứng dụng  lạ thì phải vào đây: https://myaccount.google.com/lesssecureapps?pli=1 enable cho phép truy cập email từ ứng dụng lạ mới gửi email đc
                // Cái email này có bật sẵn rồi nên ko cần bật nữa
                var email = "hethongqlnghiphep@gmail.com";
                //Mật khẩu của email trên
                var password =  "Admin@123";
                client.Credentials = new NetworkCredential(email, password);
                client.EnableSsl = true;
                //Quá 6 giây nếu ko gửi đc mail thì báo gửi mail thất bại (do ko có mạng, hay do chưa enable cái ở trên)
                client.Timeout = 6000;

                //Mấy cái này là gán lại nội dung email từ model truyền vào
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email);
                if (mailModel.ListToEmail != null && mailModel.ListToEmail.Count > 0) { mailModel.ListToEmail.ForEach(x => mail.To.Add(x)); }
                if (mailModel.ListCCEmail != null && mailModel.ListCCEmail.Count > 0) { mailModel.ListCCEmail.ForEach(x => mail.CC.Add(x)); }
                mail.Subject = mailModel.EmailSubject;
                mail.Body = mailModel.Body;
                mail.IsBodyHtml = true;
                //Gửi mail
                client.Send(mail);
                //Nếu gửi thành công thì trả về  true
                return true;
            }
            catch (Exception ex)
            {
                //Gửi mail thất bại thì trả về false
                return false;
            }
        }
    }

    //Đây là model 1 email
    public class MailModel
    {
        //Chủ đề email
        public string EmailSubject { get; set; }
        //Danh sách địa chỉ nhận email,có thể gửi cho nhiều người cùng lúc
        public List<string> ListToEmail { get; set; }
        //Danh sách địa chỉ email CC
        public List<string> ListCCEmail { get; set; }
        //Nội dung email kiểu html
        public string Body { get; set; }
        //Hàm khởi tạo
        public MailModel()
        {
            ListCCEmail = new List<string>();
            ListToEmail = new List<string>();
        }
    }
}