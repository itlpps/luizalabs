using System.Net;
using System.Net.Mail;
using System.ComponentModel;

using LuizaLabsApi.Configuration;

namespace LuizaLabsApi.Service;

public class MailService
{
    private static void SendMail(string to, string subject, string body)
    {
        if (Params.IsTest)
            return;

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(Params.Email, Params.EmailPassword),
            EnableSsl = true
        };
        client.Send(Params.Email, to, subject, body);
    }

    private static void SendMailAsync(string to, string subject, string body)
    {
        var bw = new BackgroundWorker();
        bw.DoWork += (s, e) => SendMail(to, subject, body);
        bw.RunWorkerAsync();
    }

    public static void SendConfirmationMail(string to, string userId, string token)
    {
        var subject = "Confirmação de cadastro";
        var body =
            $@"Olá, para confirmar seu cadastro clique no link: {Params.BaseUrlFrontEnd}/user/{userId}/confirm/{token}";
        SendMailAsync(to, subject, body);
    }

    public static void TwoFactorAuth(string to, string token)
    {
        var subject = "Autenticação de dois fatores";
        var body = $@"Olá, para confirmar seu login utilize o código: {token}";
        SendMailAsync(to, subject, body);
    }

    public static void PasswordRecovery(string to, string userId, string token)
    {
        var subject = "Recuperação de senha";
        var body =
            $@"Olá, para recuperar sua senha clique no link: {Params.BaseUrlFrontEnd}/user/{userId}/password/reset/{token}";
        SendMailAsync(to, subject, body);
    }
}
