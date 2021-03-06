﻿using System.Threading.Tasks;

namespace ExampleApp.Tasks.BeneficialOwners
{
    [Task("rbo", "Remove Beneficial Owner")]
    internal class Remove : BaseTask
    {
        public override async Task Run()
        {
            Write("Beneficial Owner ID to remove: ");
            var input = ReadLineAsGuid();

            await Service.DeleteBeneficialOwnerAsync(input);

            WriteLine("Removed");
        }
    }
}
