using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;

namespace Cuemon.Net.Mail;
/// <summary>
/// Provides a way for applications to distribute one or more e-mails in batches by using the Simple Mail Transfer Protocol (SMTP).
/// </summary>
public class MailDistributor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MailDistributor" /> class.
    /// </summary>
    /// <param name="carrier">The function delegate that will instantiate a new mail carrier per delivery.</param>
    /// <param name="deliverySize">The maximum number of mails a <paramref name="carrier"/> can deliver at a time. Default is a size of 20.</param>
    /// <remarks>
    /// A delivery is determined by the <paramref name="deliverySize"/>. This means, that if you are to send 100 e-mails and you have a <paramref name="deliverySize"/> of 20, 
    /// these 100 e-mails will be distributed to 5 invoked instances of <paramref name="carrier"/> shipping up till 20 e-mails each (depending if you have a filter or not).
    /// </remarks>
    public MailDistributor(Func<SmtpClient> carrier, int deliverySize = 20)
    {
        Validator.ThrowIfNull(carrier);
        Validator.ThrowIfLowerThan(deliverySize, 1, nameof(deliverySize));
        Carrier = carrier;
        DeliverySize = deliverySize;
    }

    private Func<SmtpClient> Carrier { get; }

    private int DeliverySize { get; }

    /// <summary>
    /// Sends the specified <paramref name="mail"/> to an SMTP server.
    /// </summary>
    /// <param name="mail">The e-mail to send to an SMTP server.</param>
    /// <param name="filter">The function delegate that defines the conditions for the sending of <paramref name="mail"/>.</param>
    /// <remarks>The function delegate <paramref name="filter"/> will only include the <paramref name="mail"/> if that evaluates to <c>true</c>.</remarks>
    public Task SendOneAsync(MailMessage mail, Func<MailMessage, bool> filter = null)
    {
        Validator.ThrowIfNull(mail);
        return SendAsync(Arguments.Yield(mail), filter);
    }

    /// <summary>
    /// Sends the specified sequence of <paramref name="mails"/> to an SMTP server.
    /// </summary>
    /// <param name="mails">The e-mails to send to an SMTP server.</param>
    /// <param name="filter">The function delegate that defines the conditions for sending of the <paramref name="mails"/> sequence.</param>
    /// <remarks>The function delegate <paramref name="filter"/> will only include those <paramref name="mails"/> that evaluates to <c>true</c>.</remarks>
    public Task SendAsync(IEnumerable<MailMessage> mails, Func<MailMessage, bool> filter = null)
    {
        Validator.ThrowIfNull(mails);
        var carriers = PrepareShipment(this, mails, filter);
        var shipments = new List<Task>();
        foreach (var shipment in carriers)
        {
            var filteredMails = shipment.Arg2;
            if (filteredMails.Count == 0) { continue; }
            var carrier = shipment.Arg1;
            shipments.Add(SendAsync(carrier, filteredMails));
        }
        return Task.WhenAll(shipments);
    }

    private static async Task SendAsync(Func<SmtpClient> carrierCallback, List<MailMessage> mails)
    {
        var carrier = carrierCallback();
        try
        {
            foreach (var mail in mails)
            {
                try
                {
                    await carrier.SendMailAsync(mail).ConfigureAwait(false);
                }
                finally
                {
                    mail?.Dispose();
                }
            }
        }
        finally
        {
            carrier?.Dispose();
        }
    }

    private static List<MutableTuple<Func<SmtpClient>, List<MailMessage>>> PrepareShipment(MailDistributor distributor, IEnumerable<MailMessage> mails, Func<MailMessage, bool> filter)
    {
        var partitionedMails = new PartitionerEnumerable<MailMessage>(mails, distributor.DeliverySize);
        var carriers = new List<MutableTuple<Func<SmtpClient>, List<MailMessage>>>();
        while (partitionedMails.HasPartitions)
        {
            carriers.Add(new MutableTuple<Func<SmtpClient>, List<MailMessage>>(distributor.Carrier, new List<MailMessage>(filter == null
                ? partitionedMails
                : partitionedMails.Where(filter)
            )));
        }
        return carriers;
    }
}
