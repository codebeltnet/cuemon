using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net.Mail
{
    public class MailDistributorTest : Test
    {
        public MailDistributorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task SendAsync_ShouldBatchMessagesAndRespectFilter()
        {
            var pickupDirectory = Path.Combine(Environment.CurrentDirectory, "MailPickup", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(pickupDirectory);
            try
            {
                var carrierInvocations = 0;
                var sut = new MailDistributor(() =>
                {
                    carrierInvocations++;
                    return new SmtpClient()
                    {
                        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                        PickupDirectoryLocation = pickupDirectory
                    };
                }, 2);
                var mails = Enumerable.Range(1, 5).Select(CreateMailMessage).ToList();

                await sut.SendAsync(mails, message => message.Subject != "3");

                Assert.Equal(4, Directory.GetFiles(pickupDirectory).Length);
                Assert.Equal(3, carrierInvocations);
            }
            finally
            {
                Directory.Delete(pickupDirectory, true);
            }
        }

        [Fact]
        public async Task SendAsync_ShouldSkipRejectedShipmentsAndValidateArguments()
        {
            var pickupDirectory = Path.Combine(Environment.CurrentDirectory, "MailPickup", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(pickupDirectory);
            try
            {
                var carrierInvocations = 0;
                var sut = new MailDistributor(() =>
                {
                    carrierInvocations++;
                    return new SmtpClient()
                    {
                        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                        PickupDirectoryLocation = pickupDirectory
                    };
                }, 1);

                await sut.SendAsync(new[] { CreateMailMessage(1), CreateMailMessage(2) }, _ => false);

                Assert.Empty(Directory.GetFiles(pickupDirectory));
                Assert.Equal(0, carrierInvocations);
                Assert.Throws<ArgumentNullException>(() => new MailDistributor(null));
                Assert.Throws<ArgumentOutOfRangeException>(() => new MailDistributor(() => new SmtpClient(), 0));
                await Assert.ThrowsAsync<ArgumentNullException>(() => sut.SendAsync((IEnumerable<MailMessage>)null));
                await Assert.ThrowsAsync<ArgumentNullException>(() => sut.SendOneAsync(null));
            }
            finally
            {
                Directory.Delete(pickupDirectory, true);
            }
        }

        private static MailMessage CreateMailMessage(int id)
        {
            return new MailMessage("sender@example.com", $"receiver{id}@example.com", id.ToString(), $"body-{id}");
        }
    }
}
