﻿using System;
using System.Threading.Tasks;

namespace ExampleApp.Tasks.WebhookSubscriptions
{
    [Task("dws", "Delete a Webhook Subscription")]
    internal class Delete : BaseTask
    {
        public override async Task Run()
        {
            Write("Webhook subscription ID to delete: ");
            var input = ReadLineAsGuid();

            await Service.DeleteWebhookSubscriptionAsync(input);
            WriteLine($"Deleted Subscription {input}");
        }
    }
}
