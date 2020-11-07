using System;
using System.Collections.Generic;

namespace XB.Astrea.Client.Assessment
{
    public static class RequestHelper
    {
        public static Request ParseMtToAstreaRequest(string mt)
        {
            var request = new Request
            {
                Mt = mt,
                PaymentInstructions = new List<PaymentInstruction>(),
                Actor = new Actor(),
                Principal = new Principal(),
                TargetState = "requested",
                Tags = new Tags()
            };

            SetupPaymentInstruction(request.PaymentInstructions);
            SetupActor(request.Actor);
            SetupPrincipal(request.Principal);

            return request;
        }

        private static void SetupPaymentInstruction(List<PaymentInstruction> requestPaymentInstructions)
        {
            requestPaymentInstructions.Add(new PaymentInstruction()
            {
                Identity = Guid.NewGuid().ToString(),
                PaymentType = "seb.payment.se.swift",
                RegisteringParty = new RegisteringParty()
                {
                    AuthId = "52041500480009",
                    SebId = "52041500480009"
                },
                RegistrationTime = DateTime.Now,
                InstructedDate = DateTime.Now,
                Amount = 199,
                Currency = "SEK",
                DebitAccount = new List<Account>()
                {
                    new Account()
                    {
                        Type = "iban",
                        BankIdentity = "ESSESESS",
                        Identity = "SE2750000000056970162486"
                    }
                },
                CreditAccount = new List<Account>()
                {

                },
                RemittanceInfo = new List<RemittanceInfo>()

                {

                },
                InstructionContext = new InstructionContext()
                {

                }
            });
        }

        private static void SetupActor(Actor requestActor)
        {

        }

        private static void SetupPrincipal(Principal requestPrincipal)
        {

        }
    }
}
