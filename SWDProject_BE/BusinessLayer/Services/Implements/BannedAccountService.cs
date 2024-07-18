using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataLayer.UnitOfWork;

namespace BusinessLayer.Services.Implements
{
    public class BannedAccountServices : IBannedAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BannedAccountServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task BanUser(int id, string description)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user != null)
            {
                user.Status = false;
                await _unitOfWork.Repository<User>().Update(user, id);
                await _unitOfWork.CommitAsync();

                var bannedAccount = new BannedAccount
                {
                    UserId = user.Id,
                    Description = description,
                    Date = DateTime.Now,
                    Status = true,
                };
                await _unitOfWork.Repository<BannedAccount>().InsertAsync(bannedAccount);
                await _unitOfWork.CommitAsync();

                // Send email notification to the user
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("studentexchangeweb@gmail.com", "fwpl wpkw zhqe peyh")
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("studentexchangeweb@gmail.com"),
                    Subject = "Account Banned",
                    Body = $@"
                        <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f4;
                                    color: #333333;
                                }}
                                .container {{
                                    background-color: #ffffff;
                                    padding: 20px;
                                    margin: 0 auto;
                                    width: 80%;
                                    max-width: 600px;
                                    border-radius: 10px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                }}
                                h2 {{
                                    color: #cc0000;
                                }}
                                p {{
                                    line-height: 1.6;
                                }}
                                .footer {{
                                    margin-top: 20px;
                                    font-size: 0.9em;
                                    color: #777777;
                                }}
                                @media (max-width: 600px) {{
                                    .container {{
                                        width: 100%;
                                        padding: 10px;
                                    }}
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <h2>Account Banned</h2>
                                <p>Dear {user.Email},</p>
                                <p>We regret to inform you that your account has been banned for the following reason:</p>
                                <p><strong>{description}</strong></p>
                                <p>If you believe this is a mistake, please contact our support team.</p>
                                <br>
                                <p>Best regards,</p>
                                <p>Student Exchange Web Team</p>
                                <div class='footer'>
                                    <p>This is an automated message, please do not reply.</p>
                                </div>
                            </div>
                        </body>
                        </html>
                    ",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(user.Email);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public async Task UnBanUser(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user != null)
            {
                user.Status = true;
                await _unitOfWork.Repository<User>().Update(user, id);

                var bannedAccount = await _unitOfWork.Repository<BannedAccount>().FindAsync(ba => ba.UserId == id && ba.Status == true);
                if (bannedAccount != null)
                {
                    bannedAccount.Status = false;
                    bannedAccount.ModifiedDate = DateTime.Now;
                    await _unitOfWork.Repository<BannedAccount>().Update(bannedAccount, bannedAccount.Id);
                }
                await _unitOfWork.CommitAsync();

                // Send email notification to the user
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("studentexchangeweb@gmail.com", "fwpl wpkw zhqe peyh")
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("studentexchangeweb@gmail.com"),
                    Subject = "Account Unbanned",
                    Body = $@"
                        <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f4;
                                    color: #333333;
                                }}
                                .container {{
                                    background-color: #ffffff;
                                    padding: 20px;
                                    margin: 0 auto;
                                    width: 80%;
                                    max-width: 600px;
                                    border-radius: 10px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                }}
                                h2 {{
                                    color: #006600;
                                }}
                                p {{
                                    line-height: 1.6;
                                }}
                                .footer {{
                                    margin-top: 20px;
                                    font-size: 0.9em;
                                    color: #777777;
                                }}
                                @media (max-width: 600px) {{
                                    .container {{
                                        width: 100%;
                                        padding: 10px;
                                    }}
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <h2>Account Unbanned</h2>
                                <p>Dear {user.Email},</p>
                                <p>We are pleased to inform you that your account has been unbanned.</p>
                                <p>You can now access your account as usual. If you have any questions or need further assistance, please feel free to contact our support team.</p>
                                <br>
                                <p>Best regards,</p>
                                <p>Student Exchange Web Team</p>
                                <div class='footer'>
                                    <p>This is an automated message, please do not reply.</p>
                                </div>
                            </div>
                        </body>
                        </html>
                    ",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(user.Email);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
