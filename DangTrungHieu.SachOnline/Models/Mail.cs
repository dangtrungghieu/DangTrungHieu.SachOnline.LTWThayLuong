using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using DangTrungHieu.SachOnline.Models;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.ComponentModel;

namespace DangTrungHieu.SachOnline.Models
{
    public class Mail
    {
        [DisplayName("Người gửi: ")]
        public string From { get; set; }

        [DisplayName("Người nhận: ")]
        public string To { get; set; }

        [DisplayName("Tiêu đề: ")]
        public string Subject { get; set; }

        [DisplayName("Nội dung: ")]
        public string Notes { get; set; }

        [DisplayName("File đính kèm: ")]
        public string Attachment { get; set; }
    }
}